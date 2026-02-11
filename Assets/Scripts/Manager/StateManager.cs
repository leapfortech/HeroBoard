using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Leap.Core.Tools;
using Leap.UI.Dialog;

using Sirenix.OdinInspector;

public class StateManager : SingletonBehaviour<StateManager>
{
    private readonly String[] monthNames = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
    public String[] MonthNames => monthNames;

    // LoadBoard
    [Title("LoadBoard")]
    [SerializeField]
    int loadBoardTotal = 7;

    [SerializeField, Space]
    UnityEvent onBoardLoaded;

    private int loadCount = 0;
    public bool BoardLoading { get; set; } = false;

    public void BoardLoadZero() { loadCount = 0; BoardLoading = true; }

    public void BoardLoadHide()
    {
        if (!BoardLoading)
            ScreenDialog.Instance.Hide();
        else
            BoardLoadInc();
    }

    public bool BoardLoadInc()
    {
        if (!BoardLoading)
            return false;

        loadCount++;
        if (loadCount != loadBoardTotal)
            return true;

        onBoardLoaded.Invoke();

        loadCount = 0;
        BoardLoading = false;

        return true;
    }

    // BoardUser
    [Title("BoardUser")]
    [ShowInInspector, HideReferenceObjectPicker, ReadOnly]
    public BoardUser BoardUser { get; set; } = null;

    public void ClearAll()
    {
        BoardUser = null;
    }

   

    // IdentityFull
    [Title("IdentityFull")]
    [ShowInInspector, HideReferenceObjectPicker, ReadOnly]
    public List<IdentityFull> IdentityFulls
    {
        get => identityFulls;
        set
        {
            identityFulls = value;
            identityFullDict = new Dictionary<long, IdentityFull>(identityFulls.Count);
            for (int i = 0; i < identityFulls.Count; i++)
                identityFullDict.Add(identityFulls[i].Id, identityFulls[i]); //RM BEFORE AppUserId
        }
    }
    Dictionary<long, IdentityFull> identityFullDict = new Dictionary<long, IdentityFull>();
    List<IdentityFull> identityFulls = new List<IdentityFull>();
    public IdentityFull GetIdentityFull(int appUserId) => identityFullDict.TryGetValue(appUserId, out IdentityFull identityFull) ? identityFull : null;

    // Onboarding
    public AppUserNamed[] AppUsers { get; set; } = null;
    public int AppUserIdx { get; set; } = -1;
    public AppUserNamed AppUser => AppUserIdx == -1 ? null : AppUsers[AppUserIdx];


    public Identity AppUserIdentity { get; set; } = null;
    public Address AppUserAddress { get; set; } = null;

    public void ClearOnboardings()
    {
        AppUserIdentity = null;
        AppUserAddress = null;
    }
}