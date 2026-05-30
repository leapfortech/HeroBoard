using System;
using UnityEngine;
using UnityEngine.Events;

using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.UI.Extensions;
using Leap.Data.Collections;

using Sirenix.OdinInspector;

public class PuzzleDisplayAction : MonoBehaviour
{
    [Serializable]
    public class PuzzleEventEvent : UnityEvent<int, PuzzleInfo> { }

    [Title("Params")]
    [SerializeField]
    long puzzleGameId = -1;

    [Title("Value")]
    [SerializeField]
    ValueList vllCountryFlag = null;
    [SerializeField]
    ValueList vllPuzzleDifficulty = null;
    [SerializeField]
    ValueList vllPuzzleStatus = null;

    [Title("Filters")]
    [SerializeField]
    ComboAdapter cmbStatus = null;
    [SerializeField]
    ComboAdapter cmbDifficulty = null;
    //[SerializeField]
    //Button btnFilter = null;

    [Title("List")]
    [SerializeField]
    ListScroller lstPuzzle = null;
    [SerializeField]
    Text txtEmpty = null;

    [Title("Navigation")]
    [SerializeField]
    Button btnNext = null;
    [SerializeField]
    Button btnBack = null;
    [SerializeField]
    Text txtPage = null;

    [Title("Config")]
    [SerializeField]
    int pageSize = 10;

    [Title("Event")]
    [SerializeField]
    PuzzleEventEvent onUpdateRequested = null;

    int currentPage = 1;
    int totalPages = 1;

    int filterStatus = -1, filterDifficulty = -1;
    long filterSubType = -1L;

    int idxRequest = -1;

    PuzzleService puzzleService = null;
    PuzzleAllRsp puzzlePage = null;

    private void Awake()
    {
        puzzleService = GetComponent<PuzzleService>();
    }

    private void Start()
    {
        btnNext?.AddAction(NextPage);
        btnBack?.AddAction(BackPage);
        //btnFilter?.AddAction(Filter);
    }

    // Filter
    public void Filter()
    {
        if (!cmbDifficulty.Combo.IsEmpty())
            filterDifficulty = Convert.ToInt32(cmbDifficulty.GetSelectedId());
        else
            filterDifficulty = -1;

        if (!cmbStatus.Combo.IsEmpty())
            filterStatus = Convert.ToInt32(cmbStatus.GetSelectedId());
        else
            filterStatus = -1;

        currentPage = 1;

        GetPaged(currentPage);
    }

    // Load
    public void LoadFirstPage()
    {
        currentPage = 1;
        filterStatus = -1;
        filterSubType = puzzleGameId;
        filterDifficulty = -1;

        cmbDifficulty.Clear();
        cmbStatus.Clear();

        GetPaged(currentPage);
    }

    public void NextPage()
    {
        if (currentPage >= totalPages)
            return;
        
        GetPaged(currentPage + 1);
    }

    public void BackPage()
    {
        if (currentPage <= 1)
            return;
        
        GetPaged(currentPage - 1);
    }

    public void GetPaged(int page)
    {
        ScreenDialog.Instance.Display();

        currentPage = page;

        btnNext.Interactable = false;
        btnBack.Interactable = false;

        PuzzleAllByDifficultyReq req = new PuzzleAllByDifficultyReq(page, pageSize, filterSubType, filterDifficulty, filterStatus);

        puzzleService.GetAllByDifficulty(req);
    }

    // Display
    public void FillPaged(PuzzleAllRsp rsp)
    {
        if (rsp == null || rsp.PuzzleInfos == null || rsp.PuzzleInfos.Count == 0)
        {
            ShowEmpty();
            return;
        }

        puzzlePage = rsp;

        totalPages = rsp.TotalPages;
        currentPage = rsp.Page;

        UpdatePagination();

        lstPuzzle.ClearValues();
        txtEmpty.gameObject.SetActive(false);

        for (int i = 0; i < rsp.PuzzleInfos.Count; i++)
            lstPuzzle.AddValue(CreateValue(rsp.PuzzleInfos[i]));

        lstPuzzle.ApplyValues();

        StateManager.Instance.BoardLoadHide();
    }

    private ListScrollerValue CreateValue(PuzzleInfo puzzleInfo)
    {
        ListScrollerValue value = new ListScrollerValue(12, true);

        value.SetText(0, puzzleInfo.Puzzle.CreateDateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm"));
        value.SetSprite(1, vllCountryFlag.FindRecordCellSprite(puzzleInfo.Puzzle.CountryId, "Flag"));
        value.SetText(2, vllCountryFlag.FindRecordCellString(puzzleInfo.Puzzle.CountryId, "Name"));
        value.SetText(3, puzzleInfo.Puzzle.Question);
        value.SetText(4, puzzleInfo.PuzzleAnswers[0].Description);
        value.SetText(5, puzzleInfo.PuzzleAnswers[1].Description);
        value.SetText(6, puzzleInfo.PuzzleAnswers[2].Description);
        value.SetText(7, vllPuzzleDifficulty.FindRecordCellString(puzzleInfo.Puzzle.Difficulty, "Name"));
        value.SetText(8, puzzleInfo.Puzzle.Status == 0 ? vllPuzzleStatus.FindRecordCellString(puzzleInfo.Puzzle.Status, "Name") : "");
        value.SetText(9, puzzleInfo.Puzzle.Status != 0 ? vllPuzzleStatus.FindRecordCellString(puzzleInfo.Puzzle.Status, "Name") : "");
        value.SetActive(10, puzzleInfo.Puzzle.Status != 0);
        value.SetActive(11, puzzleInfo.Puzzle.Status == 0);

        return value;
    }

    public void ApplyValue(int idx, PuzzleInfo puzzleInfo)
    {
        puzzlePage.PuzzleInfos[idx] = puzzleInfo;

        ListScrollerValue value = lstPuzzle[idx];

        value.SetText(0, puzzleInfo.Puzzle.CreateDateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm"));
        value.SetSprite(1, vllCountryFlag.FindRecordCellSprite(puzzleInfo.Puzzle.CountryId, "Flag"));
        value.SetText(2, vllCountryFlag.FindRecordCellString(puzzleInfo.Puzzle.CountryId, "Name"));
        value.SetText(3, puzzleInfo.Puzzle.Question);
        value.SetText(4, puzzleInfo.PuzzleAnswers[0].Description);
        value.SetText(5, puzzleInfo.PuzzleAnswers[1].Description);
        value.SetText(6, puzzleInfo.PuzzleAnswers[2].Description);
        value.SetText(7, vllPuzzleDifficulty.FindRecordCellString(puzzleInfo.Puzzle.Difficulty, "Name"));

        lstPuzzle.RefreshVisibleValues();

        StateManager.Instance.BoardLoadHide();
    }

    public void UpdatePagination()
    {
        txtPage.TextValue = $"Página {currentPage} / {Mathf.Max(totalPages, 1)}";

        btnBack.Interactable = currentPage > 1;
        btnNext.Interactable = currentPage < totalPages;
    }

    public void ShowEmpty()
    {
        txtEmpty.gameObject.SetActive(true);
        lstPuzzle.ApplyClearValues();

        StateManager.Instance.BoardLoadHide();
    }

    // UpdatePuzzle

    public void UpdatePuzzle(int idx)
    {
        onUpdateRequested.Invoke(idx, puzzlePage.PuzzleInfos[idx]);
    }

    // UpdateStatus
    public void Deactivate(int idx)
    {
        ChoiceDialog.Instance.Error("Eliminar reto", "¿Estás seguro que deseas eliminar el reto?", () => UpdateStatus(idx, 0), null, "Sí", "Regresar");
    }

    public void Activate(int idx)
    {
        ChoiceDialog.Instance.Info("Activar reto", "¿Estás seguro que deseas activar el reto?", () => UpdateStatus(idx, 1), null, "Sí", "Regresar");
    }

    private void UpdateStatus(int idx, int status)
    {
        idxRequest = idx;

        ScreenDialog.Instance.Display();

        puzzleService.UpdateStatus(puzzlePage.PuzzleInfos[idx].Post.Id, puzzlePage.PuzzleInfos[idx].Puzzle.Id, status);
    }

    public void ApplyUpdateStatus(bool response)
    {
        ListScrollerValue value = lstPuzzle[idxRequest];

        bool activated = value.GetActive(11);
        value.SetText(8, !activated ? vllPuzzleStatus.FindRecordCellString(0, "Name") : "");
        value.SetText(9, activated ? vllPuzzleStatus.FindRecordCellString(1, "Name") : "");
        value.SetActive(10, activated);
        value.SetActive(11, !activated);

        lstPuzzle.RefreshVisibleValues();

        StateManager.Instance.BoardLoadHide();
    }
}