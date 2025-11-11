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

public class ReferredAction : MonoBehaviour
{
    //[Title("Elements")]
    //[SerializeField]
    //ElementValue[] elementValues = null;

    [Title("Referred")]
    [SerializeField]
    ListScroller lstReferreds = null;
    [SerializeField]
    Text txtReferredsEmpty = null;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmReferred = null;
    [SerializeField]
    DataMapper dtmReferrer = null;

    //private readonly CultureInfo cultureInfo = new CultureInfo("en-US");

    public bool Selected { get; set; } = false;

    ReferredService referredService = null;
    List<ReferredFull> referreds = null;

    ReferredFull referred = null;
    //int referredIdx = -1;

    private void Awake()
    {
        referredService = GetComponent<ReferredService>();
    }

    public void Clear()
    {
        dtmReferred.ClearElements();
    }

    public void GetReferreds()
    {
        ScreenDialog.Instance.Display();

        lstReferreds.ApplyClearValues();
        txtReferredsEmpty.gameObject.SetActive(false);

        referred = null;
        referredService.GetFullAll();
    }

    public void FillReferreds(List<ReferredFull> referreds)
    {
        this.referreds = referreds;

        if (referreds.Count == 0)
        {
            lstReferreds.ApplyClearValues();
            txtReferredsEmpty.gameObject.SetActive(true);
            StateManager.Instance.BoardLoadHide();
            return;
        }

        lstReferreds.ClearValues();

        ListScrollerValue lstReferredValue;
        for (int i = 0; i < referreds.Count; i++)
        {
            lstReferredValue = new ListScrollerValue(2, true);
            lstReferredValue.SetText(0, referreds[i].Code);
            lstReferredValue.SetText(1, $"{referreds[i].FirstNames} {referreds[i].LastNames}");

            lstReferreds.AddValue(lstReferredValue);
        }

        lstReferreds.ApplyValues();

        Display(0);

        StateManager.Instance.BoardLoadHide();
    }

    public void Display(int idx)
    {
        referred = referreds[idx];
        //referredIdx = idx;

        dtmReferred.PopulateClass(referred);
        dtmReferrer.PopulateClass(referred.Referrer);
    }
}
