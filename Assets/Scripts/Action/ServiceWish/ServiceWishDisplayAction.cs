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
    [Title("Fields")]
    [SerializeField]
    Text txtContact = null;
    [SerializeField]
    Text txtNames = null;
    [SerializeField]
    Text txtBirthPlace = null;
    [SerializeField]
    Text txtAddress = null;

    [Title("Value")]
    [SerializeField]
    ValueList vllServiceWishType = null;
    [SerializeField]
    ValueList vllCountry = null;
    [SerializeField]
    ValueList vllState = null;
    [SerializeField]
    ValueList vllCity = null;

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
    ServiceWishUser serviceWishUser = null;

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

    public void ClearElements()
    {
        txtContact.TextValue = "-";
        txtNames.TextValue = "-";
        txtBirthPlace.TextValue = "-";
        txtAddress.TextValue = "-";
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

        if (serviceWishAllRsp == null || serviceWishAllRsp.ServiceWishInfos == null || serviceWishAllRsp.ServiceWishInfos.Count == 0)
        {
            ShowEmpty();
            return;
        }

        totalPages = serviceWishAllRsp.TotalPages;
        currentPage = serviceWishAllRsp.Page;

        UpdatePagination();

        lstServiceWish.ClearValues();

        SortItems(rsp.ServiceWishInfos);
        
        txtEmpty.gameObject.SetActive(false);

        for (int i = 0; i < serviceWishAllRsp.ServiceWishInfos.Count; i++)
        {
            ListScrollerValue value = new ListScrollerValue(3, true);

            value.SetText(0, vllServiceWishType.FindRecordCellString(serviceWishAllRsp.ServiceWishInfos[i].ServiceWish.ServiceTypeId, "Name"));
            value.SetText(1, serviceWishAllRsp.ServiceWishInfos[i].ServiceWish.Comment);
            value.SetText(2, serviceWishAllRsp.ServiceWishInfos[i].ServiceWish.CreateDateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm"));

            lstServiceWish.AddValue(value);
        }

        lstServiceWish.ApplyValues();
        //lstServiceWish.CheckToggle(0, true);

        Display(0);

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

    public void Display(int idx)
    {
        ClearElements();
        
        serviceWishUser = serviceWishAllRsp.ServiceWishInfos[idx].ServiceWishUser;

        bool isPhone = serviceWishAllRsp.ServiceWishInfos[idx].ServiceWishUser.Email.StartsWith("hm.") &&
                       serviceWishAllRsp.ServiceWishInfos[idx].ServiceWishUser.Email.EndsWith("@heroesmigrantes.com");

        txtContact.TextValue = isPhone ? vllCountry.FindRecordCellString(serviceWishUser.PhoneCountryId, 2) + " " + serviceWishUser.Phone : serviceWishUser.Email;
        
        txtNames.TextValue = $"{(String.IsNullOrWhiteSpace(serviceWishUser.FirstName1) ? "" : serviceWishUser.FirstName1 + " ")}" +
                             $"{(String.IsNullOrWhiteSpace(serviceWishUser.FirstName2) ? "" : serviceWishUser.FirstName2 + " ")}" +
                             $"{(String.IsNullOrWhiteSpace(serviceWishUser.LastName1) ? "" : serviceWishUser.LastName1 + " ")}" +
                             $"{(String.IsNullOrWhiteSpace(serviceWishUser.LastName2) ? "" : serviceWishUser.LastName2)}";

        if (String.IsNullOrWhiteSpace(txtNames.TextValue))
            txtNames.TextValue = "-";

        String birthCountry = vllCountry.FindRecordCellString(serviceWishUser.BirthCountryId, "Name");
        String birthState = vllState.FindRecordCellString(serviceWishUser.BirthStateId, "Name");
        String birthCity = vllCity.FindRecordCellString(serviceWishUser.BirthCityId, "Name");

        txtBirthPlace.TextValue = $"{(String.IsNullOrWhiteSpace(birthCountry) ? "" : birthCountry)}" +
                                  $"{(String.IsNullOrWhiteSpace(birthState) ? "" : ", " + birthState)}" +
                                  $"{(String.IsNullOrWhiteSpace(birthCity) ? "" : ", " + birthCity)}";
                                  

        txtBirthPlace.TextValue = txtBirthPlace.TextValue.Trim().Trim(',');

        if (String.IsNullOrWhiteSpace(txtBirthPlace.TextValue))
            txtBirthPlace.TextValue = "-";

        String country = vllCountry.FindRecordCellString(serviceWishUser.CountryId, "Name");
        String state = vllState.FindRecordCellString(serviceWishUser.StateId, "Name");
        String city = vllCity.FindRecordCellString(serviceWishUser.CityId, "Name");

        txtAddress.TextValue = $"{(String.IsNullOrWhiteSpace(country) ? "" : country)}" +
                               $"{(String.IsNullOrWhiteSpace(state) ? "" : ", " + state)}" +
                               $"{(String.IsNullOrWhiteSpace(city) ? "" : ", " + city)}";

        txtAddress.TextValue = txtAddress.TextValue.Trim().Trim(',');

        if (String.IsNullOrWhiteSpace(txtAddress.TextValue))
            txtAddress.TextValue = "-";

    }

    public void SortChanged()
    {
        if (serviceWishAllRsp != null && serviceWishAllRsp.ServiceWishInfos != null)
            FillPaged(serviceWishAllRsp);
    }

    private void SortItems(List<ServiceWishInfo> items)
    {
        int sortOption = Convert.ToInt32(tggSort.Value);

        for (int i = 0; i < items.Count - 1; i++)
        {
            for (int j = i + 1; j < items.Count; j++)
            {
                ServiceWishInfo aInfo = items[i];
                ServiceWishInfo bInfo = items[j];

                ServiceWish a = aInfo.ServiceWish;
                ServiceWish b = bInfo.ServiceWish;

                int compare = 0;

                if (sortOption == 1 || sortOption == 2) // Type
                {
                    String nameA = vllServiceWishType.FindRecordCellString(a.ServiceTypeId, "Name");
                    String nameB = vllServiceWishType.FindRecordCellString(b.ServiceTypeId, "Name");

                    compare = String.Compare(nameA, nameB, StringComparison.OrdinalIgnoreCase);
                }
                else if (sortOption == 3 || sortOption == 4) // Comment
                {
                    compare = String.Compare(a.Comment, b.Comment, StringComparison.OrdinalIgnoreCase);
                }
                else if (sortOption == 5 || sortOption == 6) // Date
                {
                    compare = DateTime.Compare(a.CreateDateTime, b.CreateDateTime);
                }
                else if (sortOption == 7 || sortOption == 8) // User Name
                {
                    String fullNameA =
                        $"{aInfo.ServiceWishUser?.FirstName1} {aInfo.ServiceWishUser?.LastName1}".Trim();

                    String fullNameB =
                        $"{bInfo.ServiceWishUser?.FirstName1} {bInfo.ServiceWishUser?.LastName1}".Trim();

                    compare = String.Compare(fullNameA, fullNameB, StringComparison.OrdinalIgnoreCase);
                }

                // Desc
                if (sortOption % 2 == 0)
                    compare = -compare;

                if (compare > 0)
                {
                    ServiceWishInfo temp = items[i];
                    items[i] = items[j];
                    items[j] = temp;
                }
            }
        }
    }
}