using System;
using UnityEngine;
using UnityEngine.Events;

using Leap.Data.Mapper;
using Leap.UI.Elements;
using Leap.UI.Dialog;

using Sirenix.OdinInspector;

public class ObdRenapAction : MonoBehaviour
{
    [Title("Validation")]
    [SerializeField]
    GameObject renapValues = null;
    [SerializeField]
    ToggleGroup tggRenapValidation = null;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmRenapDpi = null;

    [SerializeField]
    DataMapper dtmRenapData = null;

    [Title("Events")]
    [SerializeField]
    UnityEvent onAddOnboarding = null;

    [SerializeField]
    UnityEvent onUpdateOnboarding = null;

    RenapService renapService = null;

    ObdRenapLine[] renapLines = null;
    int renapIdentityId = -1;

    Onboarding onboarding = null;
    bool checkChange = false, resultChange = false;

    private void Awake()
    {
        renapService = GetComponent<RenapService>();
        renapLines = renapValues.GetComponentsInChildren<ObdRenapLine>();
    }

    // Renap

    public void GetRenapIdentity(int appUserIdx)
    {
        Clear();

        renapService.GetIdentityInfo(StateManager.Instance.AppUsers[appUserIdx].Id);
    }

    public void ApplyRenapIdentity(RenapIdentityInfo renapIdentityInfo)
    {
        if (!String.IsNullOrEmpty(renapIdentityInfo.RenapIdentity.FirstName1))
        {
            dtmRenapData.PopulateClass<RenapIdentity>(renapIdentityInfo.RenapIdentity);

            // Gender
            renapLines[10].Renap = renapIdentityInfo.RenapIdentity.Gender[0] == 'F' ? "Femenino" : renapIdentityInfo.RenapIdentity.Gender[0] == 'M' ? "Masculino" : "";

            // Marital Status
            renapLines[11].Renap = renapIdentityInfo.RenapIdentity.MaritalStatus[0] == 'S' ? renapIdentityInfo.RenapIdentity.Gender[0] == 'F' ? "Soltera" : renapIdentityInfo.RenapIdentity.Gender[0] == 'M' ? "Soltero" : "" :
                                   renapIdentityInfo.RenapIdentity.MaritalStatus[0] == 'C' ? renapIdentityInfo.RenapIdentity.Gender[0] == 'F' ? "Casada" : renapIdentityInfo.RenapIdentity.Gender[0] == 'M' ? "Casado" : "" : "";
        }

        renapIdentityId = renapIdentityInfo.RenapIdentity.Id;

        ApplyUserAppDpi();
    }

    private void ApplyUserAppDpi()
    {
        if (StateManager.Instance.AppUserIdentity == null)
        {
            Invoke(nameof(ApplyUserAppDpi), 0.1f);
            return;
        }

        DoApplyAppUserIdentity();

        ApplyOnboarding();
    }

    // Update Case
    public void ApplyAppUserIdentity()
    {
        DoApplyAppUserIdentity();

        //if (!String.IsNullOrEmpty(renapLines[3].Rtu))
        {
            for (int i = 0; i < renapLines.Length; i++)
                renapLines[i].Verify();

            tggRenapValidation.Value = "0";
        }
        //else
        //{
        //    for (int i = 0; i < renapLines.Length; i++)
        //        renapLines[i].Value = "2";

        //    tggRenapValidation.Value = "1";
        //}
    }

    private void DoApplyAppUserIdentity()
    {
        dtmRenapDpi.PopulateClass<Identity>(StateManager.Instance.AppUserIdentity);

        // Gender
        renapLines[10].Dpi = StateManager.Instance.AppUserIdentity.GenderId == 1 ? "Femenino" : StateManager.Instance.AppUserIdentity.GenderId == 2 ? "Masculino" : "";

        // Marital Status
        renapLines[11].Dpi = StateManager.Instance.AppUserIdentity.MaritalStatusId == 1 ? StateManager.Instance.AppUserIdentity.GenderId == 1 ? "Soltera" : StateManager.Instance.AppUserIdentity.GenderId == 2 ? "Soltero" : "" :
                           StateManager.Instance.AppUserIdentity.MaritalStatusId == 2 ? StateManager.Instance.AppUserIdentity.GenderId == 1 ? "Casada" : StateManager.Instance.AppUserIdentity.GenderId == 2 ? "Casado" : "" : "";
    }

    public void SetDpiVersion(String version)
    {
        renapLines[1].Dpi = version;
        renapLines[1].Verify();
    }

    // Onboarding

    private void ApplyOnboarding()
    {
        if (StateManager.Instance.Onboarding == null)
        {
            Invoke(nameof(ApplyOnboarding), 0.1f);
            return;
        }

        onboarding = StateManager.Instance.Onboarding;
        onboarding.RenapIdentityId = renapIdentityId;

        if (onboarding.Renap == 0)
        {
            //if (!String.IsNullOrEmpty(renapLines[3].Rtu))
            {
                for (int i = 0; i < renapLines.Length; i++)
                    renapLines[i].Verify();

                tggRenapValidation.Value = "0";
            }
            //else
            //{
            //    for (int i = 0; i < renapLines.Length; i++)
            //        renapLines[i].Value = "2";

            //    tggRenapValidation.Value = "1";
            //}
        }
        else
        {
            for (int idx = 0; idx < renapLines.Length; idx++)
                renapLines[idx].Value = onboarding.GetRenapCheck(idx).ToString();

            tggRenapValidation.Value = onboarding.GetRenapResult().ToString();
        }
    }

    public void SendRenap()
    {
        if (!DoSendRenap())
            onboarding = null;
    }

    private bool DoSendRenap()
    {
        onboarding = new Onboarding(StateManager.Instance.Onboarding);

        checkChange = false;

        for (int idx = 0; idx < renapLines.Length; idx++)
            checkChange |= onboarding.SetRenapCheck(idx, Convert.ToInt32(renapLines[idx].Value));

        // Result
        resultChange = onboarding.GetRenapResult() != Convert.ToInt32(tggRenapValidation.Value);

        if (resultChange)
        {
            onboarding.SetRenapResult(Convert.ToInt32(tggRenapValidation.Value));
            onboarding.BoardUserId = StateManager.Instance.BoardUser.Id;
            onboarding.Status = onboarding.GetStatus(onboarding.GetRenapResult());

            if (Onboarding.GetResultType(onboarding.GetRenapResult()) == 1)  // Accepted
            {
                for (int idx = 0; idx < renapLines.Length; idx++)
                {
                    if (renapLines[idx].Value[0] != '0')
                        continue;

                    ChoiceDialog.Instance.Error("Para validar una etapa, es necesario chequear todos los campos.");
                    return false;
                }
            }
        }
        else if (!checkChange)
        {
            ChoiceDialog.Instance.Warning("Renap", "No hay ningún cambio.");
            return false;
        }

        ChoiceDialog.Instance.Info("Renap", "¿Estás seguro de que quieres enviar los cambios?", SendAppUserRenap, null, "Sí", "No");
        return true;
    }

    private void SendAppUserRenap()
    {
        ScreenDialog.Instance.Display();

        if (resultChange)
        {
            onboarding.CreateDateTime = onboarding.UpdateDateTime = DateTime.Now;
            StateManager.Instance.Onboardings.Insert(0, onboarding);
            onAddOnboarding.Invoke();
        }
        else if (checkChange)
        {
            onboarding.UpdateDateTime = DateTime.Now;
            StateManager.Instance.Onboardings[0] = onboarding;
            onUpdateOnboarding.Invoke();
        }

        onboarding = null;
    }

    // Clear

    public void Clear()
    {
        for (int i = 0; i < renapLines.Length; i++)
            renapLines[i].Clear();
    }
}
