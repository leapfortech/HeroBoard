using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

using Leap.Graphics.Tools;
using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.UI.Extensions;
using Leap.Data.Mapper;
using Leap.Data.Collections;

using Sirenix.OdinInspector;

public class AppUserAction : MonoBehaviour
{
    //[Title("Elements")]
    //[SerializeField]
    //ElementValue[] elementValues = null;

    [Title("AppUsers")]
    [SerializeField]
    ListScroller lstAppUsers = null;
    [SerializeField]
    Text txtAppUsersEmpty = null;
    [SerializeField]
    Text txtBirthDate = null;

    [Title("Sprites")]
    [SerializeField]
    Sprite sprEmpty = null;
    [SerializeField]
    Sprite sprOnboarded = null;
    [SerializeField]
    Sprite sprInvesting = null;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmIdentityFull = null;

    public bool Selected { get; set; } = false;

    IdentityService identityService = null;
    AppUserService appUserService = null;

    IdentityFull identityFull = null;

    private void Awake()
    {
        identityService = GetComponent<IdentityService>();
        appUserService = GetComponent<AppUserService>();
    }

    public void Clear()
    {
        StateManager.Instance.IdentityFulls = new List<IdentityFull>();
        dtmIdentityFull.ClearElements();
    }

    public void GetIdentitys()
    {
        ScreenDialog.Instance.Display();

        StateManager.Instance.IdentityFulls = new List<IdentityFull>();
        lstAppUsers.ApplyClearValues();
        txtAppUsersEmpty.gameObject.SetActive(false);

        identityFull = null;
        identityService.GetFullAll(1);
    }

    public void FillIdentitys(List<IdentityFull> identityFulls)
    {
        StateManager.Instance.IdentityFulls = identityFulls;

        GetAppUsers();
    }

    public void GetAppUsers()
    {
        appUserService.GetFullsByStatus(0);
    }

    public void FillAppUsers(List<AppUserFull> appUserFulls)
    {
        if (StateManager.Instance.IdentityFulls == null)
        {
            lstAppUsers.ApplyClearValues();
            txtAppUsersEmpty.gameObject.SetActive(false);
            identityFull = null;
            StateManager.Instance.IdentityFulls = new List<IdentityFull>(appUserFulls.Count);
        }

        AppUserFull a;
        for (int i = 0; i < appUserFulls.Count; i++)
        {
            a = appUserFulls[i];
            StateManager.Instance.IdentityFulls.Add(new IdentityFull(-1, a.Id, "-", null, null, "-", null, null, "-", DateTime.Now, "-", null, a.AuthUserId, null, "-", "-", "-", "-", DateTime.Now, DateTime.Now, "-", "-", "-",
                                                                         a.PhonePrefix, a.Phone, a.Email, 0, 0, 0, 0, a.CreateDateTime, a.UpdateDateTime, a.AppUserStatusId, 0));
        }

        if (StateManager.Instance.IdentityFulls.Count == 0)
        {
            lstAppUsers.ApplyClearValues();
            txtAppUsersEmpty.gameObject.SetActive(true);
            StateManager.Instance.BoardLoadHide();
            return;
        }

        StateManager.Instance.IdentityFulls.Sort((idf1, idf2) => { return idf1.AppUserId.CompareTo(idf2.AppUserId); });

        lstAppUsers.ClearValues();

        ListScrollerValue lstAppUserValue;
        for (int i = 0; i < StateManager.Instance.IdentityFulls.Count; i++)
        {
            lstAppUserValue = new ListScrollerValue(4, true);
            IdentityFull identityFull = StateManager.Instance.IdentityFulls[i];

            if (identityFull.AppUserStatusId == 0)
            {
                lstAppUserValue.SetText(0, identityFull.BirthCity);
                lstAppUserValue.SetText(1, $"{identityFull.Email}");
                lstAppUserValue.SetSprite(2, sprEmpty);
                lstAppUserValue.SetSprite(3, sprEmpty);
            }
            else
            {
                lstAppUserValue.SetText(0, identityFull.DpiCui);
                lstAppUserValue.SetText(1, $"{identityFull.FirstNames} {identityFull.LastNames}");
                lstAppUserValue.SetSprite(2, (identityFull.AppUserStatusId == 1 || identityFull.AppUserStatusId == 5) ? identityFull.Investments == 0 ? sprOnboarded : sprInvesting : sprEmpty);
                lstAppUserValue.SetSprite(3, (identityFull.AppUserStatusId == 1 || identityFull.AppUserStatusId == 5) ? sprEmpty : sprOnboarded);
            }

            lstAppUsers.AddValue(lstAppUserValue);
        }

        lstAppUsers.ApplyValues();

        Display(0);

        StateManager.Instance.BoardLoadHide();
    }

    public void Display(int idx)
    {
        identityFull = StateManager.Instance.IdentityFulls[idx];
        dtmIdentityFull.PopulateClass(identityFull);

        if (identityFull.Status == 0)
            txtBirthDate.TextValue = "-";
    }
}