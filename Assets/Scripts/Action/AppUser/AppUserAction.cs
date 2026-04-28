using System;
using System.Collections.Generic;
using UnityEngine;

using Leap.UI.Elements;
using Leap.UI.Dialog;

using Sirenix.OdinInspector;

public class AppUserAction : MonoBehaviour
{
    [Title("Elements")]
    [SerializeField]
    Text txtFirstNames = null;
    [SerializeField]
    Text txtLastNames = null;
    [SerializeField]
    Text txtBirthDate = null;
    [SerializeField]
    Text txtGender = null;
    [SerializeField]
    Text txtBirthPlace = null;
    [SerializeField]
    Text txtAddress = null;
    [SerializeField]
    Text txtPhone = null;
    [SerializeField]
    Text txtEmail = null;

    [Title("Filters")]
    [SerializeField]
    InputField ifdAlias = null;
    [SerializeField]
    Button btnFilter = null;

    [SerializeField]
    ToggleGroup tggSort = null;

    [Title("AppUsers")]
    [SerializeField]
    ListScroller lstAppUsers = null;
    [SerializeField]
    Text txtAppUsersEmpty = null;

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

    [Title("Sprites")]
    [SerializeField]
    Sprite sprEmpty = null;
    [SerializeField]
    Sprite sprOnboarded = null;

    AppUserService appUserService = null;

    List<UserInfo> userInfos = new List<UserInfo>();
    UserInfoAllRsp userInfoAllRsp = null;

    int currentPage = 1;
    int totalPages = 1;

    string filterAlias = null;
    int filterStatus = -1;

    private void Awake()
    {
        appUserService = GetComponent<AppUserService>();
    }

    private void Start()
    {
        btnNext?.AddAction(NextPage);
        btnBack?.AddAction(BackPage);
        btnFilter?.AddAction(Filter);
    }

    public void ClearElements()
    {
        txtFirstNames.TextValue = "-";
        txtLastNames.TextValue = "-";
        txtBirthDate.TextValue = "-";
        txtGender.TextValue = "-";
        txtBirthPlace.TextValue = "-";
        txtAddress.TextValue = "-";
        txtPhone.TextValue = "-";
        txtEmail.TextValue = "-";
    }

    public void LoadFirstPage()
    {
        currentPage = 1;
        filterAlias = null;

        ifdAlias?.ClearValue();

        GetPaged(currentPage);
    }

    public void Filter()
    {
        filterAlias = ifdAlias.Text;

        if (String.IsNullOrWhiteSpace(filterAlias))
            filterAlias = null;

        currentPage = 1;

        GetPaged(currentPage);
    }

    public void GetPaged(int page)
    {
        ScreenDialog.Instance.Display();

        currentPage = page;

        btnNext.Interactable = false;
        btnBack.Interactable = false;

        UserInfoAllByAlias req = new UserInfoAllByAlias(page, pageSize, filterAlias, filterStatus);

        appUserService.GetUserInfoAllByAlias(req);
    }

    public void FillPaged(UserInfoAllRsp rsp)
    {
        userInfoAllRsp = rsp;

        if (rsp == null || rsp.UserInfos == null || rsp.UserInfos.Count == 0)
        {
            ShowEmpty();
            return;
        }

        userInfos = rsp.UserInfos;

        totalPages = rsp.TotalPages;
        currentPage = rsp.Page;

        UpdatePagination();

        SortItems(userInfos);

        lstAppUsers.ClearValues();
        txtAppUsersEmpty.gameObject.SetActive(false);

        for (int i = 0; i < userInfos.Count; i++)
        {
            ListScrollerValue value = new ListScrollerValue(4, true);

            AppUserFull app = userInfos[i].AppUserFull;
            IdentityFull idt = userInfos[i].IdentityFull;

            value.SetText(0, app.Alias);

            value.SetText(1,
                IsRegisteredPhone(app.Email)
                ? app.PhonePrefix + " " + app.Phone
                : app.Email
            );

            value.SetSprite(2, idt == null ? sprEmpty : sprOnboarded);
            value.SetSprite(3, idt == null ? sprEmpty : sprOnboarded);

            lstAppUsers.AddValue(value);
        }

        lstAppUsers.ApplyValues();
        lstAppUsers.CheckToggle(0, true);

        Display(0);

        StateManager.Instance.BoardLoadHide();
    }

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

    public void UpdatePagination()
    {
        txtPage.TextValue = $"Página {currentPage} / {Mathf.Max(totalPages, 1)}";

        btnBack.Interactable = currentPage > 1;
        btnNext.Interactable = currentPage < totalPages;
    }

    public void ShowEmpty()
    {
        ClearElements();

        txtAppUsersEmpty.gameObject.SetActive(true);
        lstAppUsers.ApplyClearValues();

        StateManager.Instance.BoardLoadHide();
    }

    public void SortChanged()
    {
        if (userInfoAllRsp != null && userInfoAllRsp.UserInfos != null)
            FillPaged(userInfoAllRsp);
    }

    private void SortItems(List<UserInfo> items)
    {
        int sortOption = Convert.ToInt32(tggSort.Value);

        for (int i = 0; i < items.Count - 1; i++)
        {
            for (int j = i + 1; j < items.Count; j++)
            {
                UserInfo a = items[i];
                UserInfo b = items[j];

                int compare = 0;

                // 1-2 Alias
                if (sortOption == 1 || sortOption == 2)
                {
                    compare = String.Compare(
                        a.AppUserFull.Alias,
                        b.AppUserFull.Alias,
                        StringComparison.OrdinalIgnoreCase);
                }
                // 3-4 Email / Phone
                else if (sortOption == 3 || sortOption == 4)
                {
                    string valA = IsRegisteredPhone(a.AppUserFull.Email)
                        ? a.AppUserFull.Phone
                        : a.AppUserFull.Email;

                    string valB = IsRegisteredPhone(b.AppUserFull.Email)
                        ? b.AppUserFull.Phone
                        : b.AppUserFull.Email;

                    compare = String.Compare(valA, valB, StringComparison.OrdinalIgnoreCase);
                }

                // DESC
                if (sortOption % 2 == 0)
                    compare = -compare;

                if (compare > 0)
                {
                    UserInfo temp = items[i];
                    items[i] = items[j];
                    items[j] = temp;
                }
            }
        }
    }

    private static bool IsRegisteredPhone(String email)
    {
        return !String.IsNullOrEmpty(email) &&
               email.StartsWith("hm.", StringComparison.OrdinalIgnoreCase) &&
               email.EndsWith("@heroesmigrantes.com", StringComparison.OrdinalIgnoreCase);
    }

    public void Display(int idx)
    {
        if (userInfos[idx].IdentityFull == null)
        {
            ClearElements();
            txtPhone.TextValue = IsRegisteredPhone(userInfos[idx].AppUserFull.Email) ? userInfos[idx].AppUserFull.PhonePrefix + " " + userInfos[idx].AppUserFull.Phone : "-";
            txtEmail.TextValue = !IsRegisteredPhone(userInfos[idx].AppUserFull.Email) ? userInfos[idx].AppUserFull.Email : "-";
        }
        else
        {
            AppUserFull app = userInfos[idx].AppUserFull;
            IdentityFull idt = userInfos[idx].IdentityFull;
            AddressFull add = userInfos[idx].AddressFull;

            String firstNames = "";

            if (!String.IsNullOrEmpty(idt.FirstName1))
                firstNames = idt.FirstName1;

            if (!String.IsNullOrEmpty(idt.FirstName2))
                firstNames += (firstNames != "" ? " " : "") + idt.FirstName2;

            txtFirstNames.TextValue = firstNames != "" ? firstNames : "-";

            String lastNames = "";

            if (!String.IsNullOrEmpty(idt.LastName1))
                lastNames = idt.LastName1;

            if (!String.IsNullOrEmpty(idt.LastName2))
                lastNames += (lastNames != "" ? " " : "") + idt.LastName2;

            txtLastNames.TextValue = lastNames != "" ? lastNames : "-";

            DateTime sqlMinDate = new DateTime(1753, 1, 1);

            txtBirthDate.TextValue = idt.BirthDate > sqlMinDate ? idt.BirthDate.ToString("dd/MM/yyyy") : "-";

            txtGender.TextValue = !String.IsNullOrEmpty(idt.Gender) ? idt.Gender : "-";

            String birthPlace = "";

            if (!String.IsNullOrEmpty(idt.BirthCountry))
                birthPlace = idt.BirthCountry;

            if (!String.IsNullOrEmpty(idt.BirthState))
                birthPlace += (birthPlace != "" ? ", " : "") + idt.BirthState;

            if (!String.IsNullOrEmpty(idt.BirthCity))
                birthPlace += (birthPlace != "" ? ", " : "") + idt.BirthCity;

            txtBirthPlace.TextValue = birthPlace != "" ? birthPlace : "-";

            String addressText = "";

            if (add != null && !String.IsNullOrEmpty(add.Country))
                addressText = add.Country;

            if (add != null && !String.IsNullOrEmpty(add.State))
                addressText += (addressText != "" ? ", " : "") + add.State;

            if (add != null && !String.IsNullOrEmpty(add.City))
                addressText += (addressText != "" ? ", " : "") + add.City;

            txtAddress.TextValue = addressText != "" ? addressText : "-";

            bool isPhone = !String.IsNullOrEmpty(app.Email) && IsRegisteredPhone(app.Email);

            String phone = "";

            if (isPhone)
            {
                if (!String.IsNullOrEmpty(app.PhonePrefix))
                    phone = app.PhonePrefix;

                if (!String.IsNullOrEmpty(app.Phone))
                    phone += (phone != "" ? " " : "") + app.Phone;
            }

            txtPhone.TextValue = isPhone ? phone : "-";
            txtEmail.TextValue = !isPhone ? (app.Email ?? "-") : "-";
        }
    }
}