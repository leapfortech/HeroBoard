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
    long subtype = -1;

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
    PuzzleEventEvent onSelected = null;

    int currentPage = 1;
    int totalPages = 1;

    int filterStatus = -1, filterDifficulty = -1;
    long filterSubType = -1L;

    PuzzleService puzzleService = null;
    PuzzleAllRsp puzzleAllRsp = null;

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
        filterSubType = subtype;
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

        puzzleAllRsp = rsp;

        totalPages = puzzleAllRsp.TotalPages;
        currentPage = puzzleAllRsp.Page;

        UpdatePagination();

        lstPuzzle.ClearValues();
        txtEmpty.gameObject.SetActive(false);

        for (int i = 0; i < puzzleAllRsp.PuzzleInfos.Count; i++)
            lstPuzzle.AddValue(CreateValue(puzzleAllRsp.PuzzleInfos[i]));

        lstPuzzle.ApplyValues();

        StateManager.Instance.BoardLoadHide();
    }

    public void RefreshValue(int idx, PuzzleInfo puzzleInfo)
    {
        puzzleAllRsp.PuzzleInfos[idx] = puzzleInfo;

        lstPuzzle[idx] = CreateValue(puzzleInfo);

        lstPuzzle.RefreshVisibleValues();

        StateManager.Instance.BoardLoadHide();
    }

    public void UpdatePagination()
    {
        txtPage.TextValue = $"Página {currentPage} / {Mathf.Max(totalPages, 1)}";

        btnBack.Interactable = currentPage > 1;
        btnNext.Interactable = currentPage < totalPages;
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

    public void ShowEmpty()
    {
        txtEmpty.gameObject.SetActive(true);
        lstPuzzle.ApplyClearValues();

        StateManager.Instance.BoardLoadHide();
    }

    // Select
    public void Select(int idx)
    {
        onSelected.Invoke(idx, puzzleAllRsp.PuzzleInfos[idx]);
    }
}