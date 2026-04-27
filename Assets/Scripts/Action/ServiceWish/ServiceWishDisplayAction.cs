using System;
using UnityEngine;

using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.UI.Extensions;
using Leap.Data.Collections;

using Sirenix.OdinInspector;
using System.Collections.Generic;


public class ServiceWishDisplayAction : MonoBehaviour
{
    [Title("Value")]
    [SerializeField]
    ValueList vllServiceWishType = null;

    [Title("Filters")]
    //[SerializeField]
    //ComboAdapter cmbStatus = null;
    [SerializeField]
    ComboAdapter cmbType = null;
    //[SerializeField]
    //Button btnFilter = null;

    [SerializeField]
    ToggleGroup tggSort = null;

    [Title("List")]
    [SerializeField]
    ListScroller lstServiceWish = null;
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
    int filterStatus = -1;
    long filterType = -1;

    ServiceWishService serviceWishService = null;
    ServiceWishAllRsp serviceWishAllRsp = null;

    private void Awake()
    {
        serviceWishService = GetComponent<ServiceWishService>();
    }

    private void Start()
    {
        btnNext?.AddAction(NextPage);
        btnBack?.AddAction(BackPage);
        //btnFilter?.AddAction(Filter);
    }

    public void LoadFirstPage()
    {
        currentPage = 1;
        filterStatus = -1;
        filterType = -1;

        cmbType.Clear();

        //cmbStatus.SelectIndex(0);
        //cmbType.SelectIndex(0);

        GetPaged(currentPage);
    }

    public void Filter()
    {
        //filterStatus = Convert.ToInt32(cmbStatus.GetSelectedId());
        filterType = Convert.ToInt64(cmbType.GetSelectedId());

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

    void GetPaged(int page)
    {
        ScreenDialog.Instance.Display();

        currentPage = page;

        btnNext.Interactable = false;
        btnBack.Interactable = false;

        ServiceWishAllByTypeReq req = new ServiceWishAllByTypeReq(page, pageSize, filterType, filterStatus);

        serviceWishService.GetAllByType(req);
    }

    public void FillPaged(ServiceWishAllRsp rsp)
    {
        serviceWishAllRsp = rsp;

        if (serviceWishAllRsp == null || serviceWishAllRsp.ServiceWishs == null || serviceWishAllRsp.ServiceWishs.Count == 0)
        {
            ShowEmpty();
            return;
        }

        totalPages = serviceWishAllRsp.TotalPages;
        currentPage = serviceWishAllRsp.Page;

        UpdatePagination();

        lstServiceWish.ClearValues();

        SortItems(rsp.ServiceWishs);
        
        txtEmpty.gameObject.SetActive(false);

        for (int i = 0; i < serviceWishAllRsp.ServiceWishs.Count; i++)
        {
            ListScrollerValue value = new ListScrollerValue(3, true);

            value.SetText(0, vllServiceWishType.FindRecordCellString(serviceWishAllRsp.ServiceWishs[i].ServiceTypeId, "Name"));
            value.SetText(1, serviceWishAllRsp.ServiceWishs[i].Comment);
            value.SetText(2, serviceWishAllRsp.ServiceWishs[i].CreateDateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm"));

            lstServiceWish.AddValue(value);
        }

        lstServiceWish.ApplyValues();
        //lstServiceWish.CheckToggle(0, true);

        StateManager.Instance.BoardLoadHide();
    }

    void UpdatePagination()
    {
        txtPage.TextValue = $"Página {currentPage} / {Mathf.Max(totalPages, 1)}";

        btnBack.Interactable = currentPage > 1;
        btnNext.Interactable = currentPage < totalPages;
    }

    void ShowEmpty()
    {
        txtEmpty.gameObject.SetActive(true);
        lstServiceWish.ApplyClearValues();

        StateManager.Instance.BoardLoadHide();
    }

    public void SortChanged()
    {
        if (serviceWishAllRsp != null && serviceWishAllRsp.ServiceWishs != null)
            FillPaged(serviceWishAllRsp);
    }

    private void SortItems(List<ServiceWish> items)
    {
        int sortOption = Convert.ToInt32(tggSort.Value);

        for (int i = 0; i < items.Count - 1; i++)
        {
            for (int j = i + 1; j < items.Count; j++)
            {
                ServiceWish a = items[i];
                ServiceWish b = items[j];

                int compare = 0;

                if (sortOption == 1 || sortOption == 2) // Type
                {
                    String nameA = vllServiceWishType.FindRecordCellString(a.ServiceTypeId, "Name");
                    String nameB = vllServiceWishType.FindRecordCellString(b.ServiceTypeId, "Name");
                    compare = String.Compare(nameA, nameB, StringComparison.OrdinalIgnoreCase);
                }
                else if (sortOption == 3 || sortOption == 4) // Comment
                    compare = String.Compare(a.Comment, b.Comment, StringComparison.OrdinalIgnoreCase);
                else if (sortOption == 5 || sortOption == 6) // Date
                    compare = DateTime.Compare(a.CreateDateTime, b.CreateDateTime);

                // Desc
                if (sortOption % 2 == 0)
                    compare = -compare;

                if (compare > 0)
                {
                    ServiceWish temp = items[i];
                    items[i] = items[j];
                    items[j] = temp;
                }
            }
        }
    }
}