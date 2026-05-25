using System;
using UnityEngine;

using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.UI.Extensions;
using Leap.Data.Collections;

using Sirenix.OdinInspector;
using System.Collections.Generic;


public class PuzzleDisplayAction : MonoBehaviour
{
    [Title("Params")]
    [SerializeField]
    long subtype = -1;

    [Title("Fields")]
    //[SerializeField]
    //Text txtContact = null;
    //[SerializeField]
    //Text txtNames = null;
    //[SerializeField]
    //Text txtBirthPlace = null;
    //[SerializeField]
    //Text txtAddress = null;

    [Title("Value")]
    [SerializeField]
    ValueList vllPuzzleSubtype = null;
    [SerializeField]
    ValueList vllCountry = null;
    //[SerializeField]
    //ValueList vllState = null;
    //[SerializeField]
    //ValueList vllCity = null;

    [Title("Filters")]
    [SerializeField]
    ComboAdapter cmbStatus = null;
    [SerializeField]
    ComboAdapter cmbDifficulty = null;
    //[SerializeField]
    //Button btnFilter = null;

    [SerializeField]
    ToggleGroup tggSort = null;

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

    // Navigation
    int currentPage = 1;
    int totalPages = 1;

    // Filters
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

    public void ClearElements()
    {
        //txtContact.TextValue = "-";
        //txtNames.TextValue = "-";
        //txtBirthPlace.TextValue = "-";
        //txtAddress.TextValue = "-";
    }

    public void LoadFirstPage()
    {
        currentPage = 1;
        filterStatus = -1;
        filterSubType = subtype;

        cmbDifficulty.Clear();

        cmbStatus.SelectIndex(0);
        

        GetPaged(currentPage);
    }

    public void Filter()
    {
        filterStatus = Convert.ToInt32(cmbStatus.GetSelectedId());
        filterDifficulty = Convert.ToInt32(cmbDifficulty.GetSelectedId());

        currentPage = 1;

        GetPaged(currentPage);
    }

    // Navigation
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

    public void FillPaged(PuzzleAllRsp rsp)
    {
        puzzleAllRsp = rsp;

        if (puzzleAllRsp == null || puzzleAllRsp.PuzzleInfos == null || puzzleAllRsp.PuzzleInfos.Count == 0)
        {
            ShowEmpty();
            return;
        }

        totalPages = puzzleAllRsp.TotalPages;
        currentPage = puzzleAllRsp.Page;

        UpdatePagination();

        lstPuzzle.ClearValues();
        
        txtEmpty.gameObject.SetActive(false);

        for (int i = 0; i < puzzleAllRsp.PuzzleInfos.Count; i++)
        {
            ListScrollerValue value = new ListScrollerValue(3, true);

            value.SetText(0, "");
            value.SetText(1, "");
            value.SetText(2, "");   //puzzleAllRsp.PuzzleInfos[i].Puzzle.PuzzleSubtypeId);

            lstPuzzle.AddValue(value);
        }

        lstPuzzle.ApplyValues();

        Display(0);

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

    public void Display(int idx)
    {
        ClearElements();
    }
}