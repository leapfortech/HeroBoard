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

    [Title("AppUsers")]
    [SerializeField]
    ListScroller lstAppUsers = null;
    [SerializeField]
    Text txtAppUsersEmpty = null;

    [Title("Sprites")]
    [SerializeField]
    Sprite sprEmpty = null;
    [SerializeField]
    Sprite sprOnboarded = null;

    public bool Selected { get; set; } = false;

    AppUserService appUserService = null;
    List<UserInfo> userInfos = new List<UserInfo>();

    private void Awake()
    {
        appUserService = GetComponent<AppUserService>();
    }

    public void Clear()
    {
        userInfos = new List<UserInfo>();
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

    public void GetUserInfos()
    {
        ScreenDialog.Instance.Display();

        txtAppUsersEmpty.gameObject.SetActive(false);

        appUserService.GetUserInfosByStatus(1);
    }

    public void FillUserInfos(List<UserInfo> userInfos)
    {
        this.userInfos = userInfos;

        if (userInfos == null || userInfos.Count == 0)
        {
            lstAppUsers.ApplyClearValues();
            txtAppUsersEmpty.gameObject.SetActive(true);
            userInfos = new List<UserInfo>(userInfos.Count);
        }

        lstAppUsers.ClearValues();

        ListScrollerValue lstAppUserValue;
        for (int i = 0; i < userInfos.Count; i++)
        {
            lstAppUserValue = new ListScrollerValue(4, true);
            AppUserFull appUserFull = userInfos[i].AppUserFull;
            IdentityFull identityFull = userInfos[i].IdentityFull;

            lstAppUserValue.SetText(0, $"{userInfos[i].AppUserFull.Alias}");
            lstAppUserValue.SetText(1, IsRegisteredPhone(userInfos[i].AppUserFull.Email)
                                                         ? userInfos[i].AppUserFull.PhonePrefix + " " + userInfos[i].AppUserFull.Phone
                                                         : userInfos[i].AppUserFull.Email);
            lstAppUserValue.SetSprite(2, identityFull == null ? sprEmpty : sprOnboarded);
            lstAppUserValue.SetSprite(3, identityFull == null ? sprEmpty : sprOnboarded);

            lstAppUsers.AddValue(lstAppUserValue);
        }

        lstAppUsers.ApplyValues();

        Display(0);

        StateManager.Instance.BoardLoadHide();
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

    private static bool IsRegisteredPhone(String email)
    {
        return !String.IsNullOrEmpty(email) &&
               email.StartsWith("hm.", StringComparison.OrdinalIgnoreCase) &&
               email.EndsWith("@heroesmigrantes.com", StringComparison.OrdinalIgnoreCase);
    }
}