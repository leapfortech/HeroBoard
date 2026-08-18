using System;
using System.Collections.Generic;
using UnityEngine;

using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.Data.Mapper;

using Sirenix.OdinInspector;

public class ReferredAction : MonoBehaviour
{
    [Title("Elements")]
    [SerializeField]
    Text[] texts = null;

    [Title("Filters")]
    [SerializeField]
    InputField ifdCode = null;
    [SerializeField]
    Button btnFilter = null;

    [SerializeField]
    ToggleGroup tggSort = null;

    [Title("Referred")]
    [SerializeField]
    ListScroller lstReferreds = null;
    [SerializeField]
    Text txtReferredsEmpty = null;

    [Title("Navigation")]
    [SerializeField]
    Button btnNext = null;
    [SerializeField]
    Button btnBack = null;
    [SerializeField]
    Text txtPage = null;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmReferred = null;
    [SerializeField]
    DataMapper dtmReferrer = null;

    [Title("Config")]
    [SerializeField]
    int pageSize = 10;

    // Navigation
    int currentPage = 1;
    int totalPages = 1;

    // Filters
    string filterCode = null;
    int filterStatus = -1;

    ReferredService referredService = null;
    ReferredFullAllRsp referredFullAllRsp = null;

    List<ReferredFull> referreds = null;
    ReferredFull referred = null;

    private void Awake()
    {
        referredService = GetComponent<ReferredService>();
    }

    private void Start()
    {
        btnNext?.AddAction(NextPage);
        btnBack?.AddAction(BackPage);
        btnFilter?.AddAction(Filter);
    }

    public void ClearElements()
    {
        for (int i = 0; i < texts.Length; i++)
            texts[i].TextValue = "-";
    }

    public void LoadFirstPage()
    {
        currentPage = 1;
        filterCode = null;
        filterStatus = -1;

        ifdCode?.ClearValue();

        GetPaged(currentPage);
    }

    public void Filter()
    {
        filterCode = ifdCode.Text;

        if (string.IsNullOrWhiteSpace(filterCode))
            filterCode = null;

        currentPage = 1;

        GetPaged(currentPage);
    }

    // Navigation
    public void NextPage()
    {
        if (currentPage >= totalPages) return;
        GetPaged(currentPage + 1);
    }

    public void BackPage()
    {
        if (currentPage <= 1) return;
        GetPaged(currentPage - 1);
    }

    void GetPaged(int page)
    {
        ScreenDialog.Instance.Display();

        currentPage = page;

        btnNext.Interactable = false;
        btnBack.Interactable = false;

        var req = new ReferredAllByCodeReq(page, pageSize, filterCode, filterStatus);

        referredService.GetFullAllByCode(req);
    }

    public void FillPaged(ReferredFullAllRsp rsp)
    {
        referredFullAllRsp = rsp;

        if (referredFullAllRsp == null || referredFullAllRsp.ReferredFulls == null || referredFullAllRsp.ReferredFulls.Count == 0)
        {
            ShowEmpty();
            return;
        }

        referreds = referredFullAllRsp.ReferredFulls;

        totalPages = referredFullAllRsp.TotalPages;
        currentPage = referredFullAllRsp.Page;

        UpdatePagination();

        lstReferreds.ClearValues();

        SortItems(rsp.ReferredFulls);

        txtReferredsEmpty.gameObject.SetActive(false);

        for (int i = 0; i < referreds.Count; i++)
        {
            ReferredFull item = referreds[i];

            ListScrollerValue value = new ListScrollerValue(lstReferreds.ListItem, true);
            value.SetText(0, item.Code);
            value.SetText(1, $"{item.FirstName1} {item.LastName1}");

            lstReferreds.AddValue(value);
        }

        lstReferreds.ApplyValues();
        lstReferreds.CheckToggle(0, true);

        Display(0);

        StateManager.Instance.BoardLoadHide();
    }

    public void Display(int idx)
    {
        referred = referreds[idx];

        dtmReferred.PopulateClass(referred);
        dtmReferrer.PopulateClass(referred.Referrer);
    }

    public void UpdatePagination()
    {
        txtPage.TextValue = $"Página {currentPage} / {Mathf.Max(totalPages, 1)}";

        btnBack.Interactable = currentPage > 1;
        btnNext.Interactable = currentPage < totalPages;
    }

    public void ShowEmpty()
    {
        ClearElements();

        txtReferredsEmpty.gameObject.SetActive(true);
        lstReferreds.ApplyClearValues();

        StateManager.Instance.BoardLoadHide();
    }

    public void SortChanged()
    {
        if (referredFullAllRsp != null && referredFullAllRsp.ReferredFulls != null)
            FillPaged(referredFullAllRsp);
    }

    private void SortItems(List<ReferredFull> items)
    {
        int sortOption = Convert.ToInt32(tggSort.Value);

        for (int i = 0; i < items.Count - 1; i++)
        {
            for (int j = i + 1; j < items.Count; j++)
            {
                ReferredFull a = items[i];
                ReferredFull b = items[j];

                int compare = 0;

                if (sortOption == 1 || sortOption == 2) // Code
                {
                    compare = String.Compare(a.Code, b.Code, StringComparison.OrdinalIgnoreCase);
                }
                else if (sortOption == 3 || sortOption == 4) // Name
                {
                    String nameA = $"{a.FirstName1} {a.FirstName2} {a.LastName1} {a.LastName2}";
                    String nameB = $"{b.FirstName1} {b.FirstName2} {b.LastName1} {b.LastName2}";

                    compare = String.Compare(nameA.Trim(), nameB.Trim(), StringComparison.OrdinalIgnoreCase);
                }

                // Desc
                if (sortOption % 2 == 0)
                    compare = -compare;

                if (compare > 0)
                {
                    ReferredFull temp = items[i];
                    items[i] = items[j];
                    items[j] = temp;
                }
            }
        }
    }
}
