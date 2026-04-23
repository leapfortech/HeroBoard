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
    //[SerializeField]
    //Text txtBirthDate = null;

    [Title("Sprites")]
    [SerializeField]
    Sprite sprEmpty = null;
    [SerializeField]
    Sprite sprOnboarded = null;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmIdentityFull = null;

    public bool Selected { get; set; } = false;

    IdentityService identityService = null;
    AppUserService appUserService = null;
    List<IdentityFull> identityFulls = null;

    IdentityFull identityFull = null;

    private void Awake()
    {
        identityService = GetComponent<IdentityService>();
        appUserService = GetComponent<AppUserService>();
    }

    public void Clear()
    {
        //StateManager.Instance.IdentityFulls = new List<IdentityFull>();
        identityFulls = new List<IdentityFull>();
        dtmIdentityFull.ClearElements();
    }

    public void GetIdentitys()
    {
        ScreenDialog.Instance.Display();

        //StateManager.Instance.IdentityFulls = new List<IdentityFull>();
        identityFulls = new List<IdentityFull>();
        lstAppUsers.ApplyClearValues();
        txtAppUsersEmpty.gameObject.SetActive(false);

        identityFull = null;
        identityService.GetFullAll(1);
    }

    public void FillIdentitys(List<IdentityFull> identityFulls)
    {
        //StateManager.Instance.IdentityFulls = identityFulls;
        this.identityFulls = identityFulls;

        GetAppUsers();
    }

    public void GetAppUsers()
    {
        appUserService.GetFullsByStatus(1);
    }

    public void FillAppUsers(List<AppUserFull> appUserFulls)
    {
        //if (StateManager.Instance.IdentityFulls == null)
        if (identityFulls == null)
        {
            lstAppUsers.ApplyClearValues();
            txtAppUsersEmpty.gameObject.SetActive(false);
            identityFull = null;
            //StateManager.Instance.IdentityFulls = new List<IdentityFull>(appUserFulls.Count);
            identityFulls = new List<IdentityFull>(appUserFulls.Count);
        }

        AppUserFull a;
        for (int i = 0; i < appUserFulls.Count; i++)
        {
            a = appUserFulls[i];

            //StateManager.Instance.IdentityFulls.Add(new IdentityFull(-1, "-", null, "-", null, "-", DateTime.Now, "-", "-",
            //                                                         a.PhonePrefix, a.Phone, a.Email, a.CreateDateTime,
            //                                                         a.UpdateDateTime, a.AppUserStatusId, 0));

            identityFulls.Add(new IdentityFull(-1, "-", null, "-", null, "-", DateTime.Now, "-", "-", "-",
                                               a.PhonePrefix, a.Phone, a.Email, a.CreateDateTime,
                                               a.UpdateDateTime, a.AppUserStatusId, 0));
        }

        //if (StateManager.Instance.IdentityFulls.Count == 0)
        if (identityFulls.Count == 0)
        {
            lstAppUsers.ApplyClearValues();
            txtAppUsersEmpty.gameObject.SetActive(true);
            StateManager.Instance.BoardLoadHide();
            return;
        }

        //StateManager.Instance.IdentityFulls.Sort((idf1, idf2) => { return idf1.Id.CompareTo(idf2.Id); });
        identityFulls.Sort((idf1, idf2) => { return idf1.Id.CompareTo(idf2.Id); });

        lstAppUsers.ClearValues();

        ListScrollerValue lstAppUserValue;
        //for (int i = 0; i < StateManager.Instance.IdentityFulls.Count; i++)
        for (int i = 0; i < identityFulls.Count; i++)
        {
            lstAppUserValue = new ListScrollerValue(4, true);
            //IdentityFull identityFull = StateManager.Instance.IdentityFulls[i];
            IdentityFull identityFull = identityFulls[i];

            if (identityFull.Status == 0)
            {
                lstAppUserValue.SetText(0, identityFull.PhonePrefix + " " + identityFull.Phone);
                lstAppUserValue.SetText(1, $"{identityFull.Email}");
                lstAppUserValue.SetSprite(2, sprEmpty);
                lstAppUserValue.SetSprite(3, sprEmpty);
            }
            else
            {
                lstAppUserValue.SetText(0, identityFull.BirthCountry);
                lstAppUserValue.SetText(1, $"{identityFull.FirstNames} {identityFull.LastNames}");
                lstAppUserValue.SetSprite(2, sprOnboarded);
                lstAppUserValue.SetSprite(3, sprOnboarded); 
            }

            lstAppUsers.AddValue(lstAppUserValue);
        }

        lstAppUsers.ApplyValues();

        Display(0);

        StateManager.Instance.BoardLoadHide();
    }

    public void Display(int idx)
    {
        identityFull = identityFulls[idx];
        dtmIdentityFull.PopulateClass(identityFull);

        //txtBirthDate.TextValue = identityFull.BirthDate;
    }
}