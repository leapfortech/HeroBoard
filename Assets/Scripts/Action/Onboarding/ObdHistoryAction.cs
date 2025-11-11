using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Leap.Core.Tools;
using Leap.UI.Elements;
using Leap.UI.Page;
using Leap.UI.Dialog;

using Sirenix.OdinInspector;

public class ObdHistoryAction : MonoBehaviour
{
    [Title("List")]
    [SerializeField]
    ListScroller lstOnboardings = null;

    [Title("Styles")]
    [SerializeField]
    Style[] styles = null;

    [Title("Display")]
    [SerializeField]
    Image imgOverlay = null;
    [SerializeField]
    ToggleGroup tggOnboarding = null;

    [Title("Authorize")]
    [SerializeField]
    Button btnProgress = null;

    [SerializeField]
    Button btnWaiting = null;

    [SerializeField]
    Button btnAuthorize = null;

    [SerializeField]
    Button btnReject = null;

    [Title("Events")]
    [SerializeField]
    UnityIntEvent onAppUserChanged;

    [SerializeField]
    UnityEvent onOnboardings;

    [SerializeField]
    UnityEvent onFinished;

    OnboardingService onboardingService = null;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (onboardingService == null)
            onboardingService = GetComponent<OnboardingService>();
    }

    // AppUser

    public void ChangeAppUser(int appUserIdx)
    {
        StateManager.Instance.AppUserIdx = appUserIdx;
        StateManager.Instance.ClearOnboardings();

        onAppUserChanged.Invoke(appUserIdx);
    }

    public void UpdateOnboarding(int appUserId)
    {
        int appUserIdx = -1;

        for (int idx = 0; idx < StateManager.Instance.AppUsers.Length; idx++)
        {
            if (StateManager.Instance.AppUsers[idx].Id == appUserId)
            {
                appUserIdx = idx;
                break;
            }
        }

        if (appUserIdx == -1)
        {
            ChoiceDialog.Instance.Error("Onboarding", "Inversionista #" + appUserId + " no encontrado.");
            return;
        }

        if (StateManager.Instance.AppUserIdx == appUserIdx)
        {
            ChoiceDialog.Instance.Info("Onboarding", StateManager.Instance.AppUsers[appUserIdx].FirstName1 + " " + StateManager.Instance.AppUsers[appUserIdx].LastName1 +
                                                     " ha actualizado sus datos.", () => { ChangeAppUser(appUserIdx); });
        }
        else
        {
            ChoiceDialog.Instance.Info("Onboarding", StateManager.Instance.AppUsers[appUserIdx].FirstName1 + " " + StateManager.Instance.AppUsers[appUserIdx].LastName1 +
                                                     " ha actualizado sus datos.\r\n¿Quieres desplegar su actualización?", () => { ChangeAppUser(appUserIdx); }, null, "Sí", "Ahora no");
        }
    }

    // Onboardings

    public void GetOnboardings(int _)
    {
        Initialize();

        onboardingService.GetOnboardings(StateManager.Instance.AppUser.Id);
    }

    public void ApplyOnboardings(List<Onboarding> onboardings)
    {
        StateManager.Instance.Onboardings = onboardings;

        //Panel pnlOnboarding = transform.parent.parent.parent.GetComponent<Panel>();
        //pnlOnboarding.PanelController.ChangePanel(pnlOnboarding);
        onOnboardings.Invoke();

        tggOnboarding.Value = "0";

        //ApplyOnboardings();
        ApplyMenu();
    }

    private void ApplyOnboardings()
    {
        lstOnboardings.ClearValues();

        for (int i = 0; i < StateManager.Instance.Onboardings.Count; i++)
        {
            ListScrollerValue lstObdValue = new ListScrollerValue(7, false);
            Onboarding onboarding = StateManager.Instance.Onboardings[i];
            lstObdValue.SetText(0, $"{onboarding.CreateDateTime.ToLocalTime():dd/MM/yyyy HH:mm:ss}");
            lstObdValue.SetStyle(1, styles[Onboarding.GetResultType(onboarding.GetDpiFrontResult())]);
            lstObdValue.SetStyle(2, styles[Onboarding.GetResultType(onboarding.GetDpiBackResult())]);
            lstObdValue.SetStyle(3, styles[Onboarding.GetResultType(onboarding.GetRenapResult())]);
            lstObdValue.SetStyle(4, styles[Onboarding.GetResultType(onboarding.GetPortraitResult())]);
            lstObdValue.SetStyle(5, styles[Onboarding.GetResultType(onboarding.GetAddressResult())]);

            lstOnboardings.AddValue(lstObdValue);
        }

        lstOnboardings.ApplyValues();
    }

    private void ApplyMenu()
    {
        imgOverlay.gameObject.SetActive(StateManager.Instance.Onboarding.Status == 3);

        btnWaiting.gameObject.SetActive(StateManager.Instance.Onboarding.Status == 3);
        btnAuthorize.gameObject.SetActive(StateManager.Instance.Onboarding.IsAuthorized());
        btnReject.gameObject.SetActive(StateManager.Instance.Onboarding.IsRejected());
        btnProgress.gameObject.SetActive(!btnWaiting.gameObject.activeSelf && !btnAuthorize.gameObject.activeSelf && !btnReject.gameObject.activeSelf);
    }

    // Add / Update

    public void AddOnboarding()
    {
        onboardingService.AddOnboarding(StateManager.Instance.Onboarding);
    }

    public void UpdateOnboarding()
    {
        onboardingService.UpdateOnboarding(StateManager.Instance.Onboarding);
    }

    public void ApplyOnboarding(int onboardingId)
    {
        StateManager.Instance.Onboarding.Id = onboardingId;

        //ApplyOnboardings();
        ApplyMenu();

        ScreenDialog.Instance.Hide();
    }

    public void AuthorizeOnboarding()
    {
        ChoiceDialog.Instance.Info("Onboarding", "¿Estás seguro de que quieres autorizar este onboarding?", () => { onboardingService.Authorize(StateManager.Instance.Onboarding); }, null, "Sí", "No");
    }

    public void RejectOnboarding()
    {
        ChoiceDialog.Instance.Info("Onboarding", "¿Estás seguro de que quieres rechazar este onboarding?", () => { onboardingService.Reject(StateManager.Instance.Onboarding); }, null, "Sí", "No");
    }

    public void ApplyOnboardingResult()
    {
        onFinished.Invoke();
    }

    // Refresh

    public void RefreshOnboardingStyles()
    {
        Invoke(nameof(DoRefreshOnboardingStyles), .02f);
    }

    private void DoRefreshOnboardingStyles()
    {
        ThemeManager.Instance.RefreshStylesInHierarchy(lstOnboardings.gameObject);
    }
}
