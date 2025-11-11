using System;
using UnityEngine;
using UnityEngine.Events;

using Leap.UI.Elements;
using Leap.UI.Dialog;

using Sirenix.OdinInspector;

public class ObdAppUserAction : MonoBehaviour
{
    [Title("List")]
    [SerializeField]
    ListScroller lstAppUsers = null;

    [SerializeField]
    Text txtAppUsersEmpty = null;

    [Title("Event")]
    [SerializeField]
    UnityEvent onDisplay;

    public bool Selected { get; set; } = false;
    AppUserService appUserService = null;

    private void Awake()
    {
        appUserService = GetComponent<AppUserService>();
    }

    private void OnEnable()
    {
        if (!Selected)
            return;

        onDisplay.Invoke();
    }

    // AppUsers

    public void GetAppUsers()
    {
        ScreenDialog.Instance.Display();

        lstAppUsers.ApplyClearValues();
        txtAppUsersEmpty.gameObject.SetActive(false);

        StateManager.Instance.AppUserIdx = -1;
        StateManager.Instance.ClearOnboardings();

        appUserService.GetAppUsersByStatus(3);
    }

    public void FillAppUsers(AppUserNamed[] appUsers)
    {
        if (appUsers.Length == 0)
        {
            lstAppUsers.ApplyClearValues();
            txtAppUsersEmpty.gameObject.SetActive(true);
            ScreenDialog.Instance.Hide();
            return;
        }

        lstAppUsers.ClearValues();

        ListScrollerValue lstAppUserValue;
        for (int i = 0; i < appUsers.Length; i++)
        {
            lstAppUserValue = new ListScrollerValue(3, false);
            if (!String.IsNullOrEmpty(appUsers[i].FirstName1))
                lstAppUserValue.SetText(0, appUsers[i].FirstName1 + (!String.IsNullOrEmpty(appUsers[i].FirstName2) ? " " + appUsers[i].FirstName2 : "") + " " +
                                          appUsers[i].LastName1 + (!String.IsNullOrEmpty(appUsers[i].LastName2) ? " " + appUsers[i].LastName2 : ""));
            else
                lstAppUserValue.SetText(0, "---");

            lstAppUserValue.SetText(1, appUsers[i].Phone);
            lstAppUserValue.SetText(2, appUsers[i].Email);

            lstAppUsers.AddValue(lstAppUserValue);
        }

        lstAppUsers.ApplyValues();

        //txtAppUsersEmpty.gameObject.SetActive(false);
        ScreenDialog.Instance.Hide();
    }
}
