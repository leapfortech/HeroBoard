using System;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class PhoneService : MonoBehaviour
{
    [Space]
    [SerializeField]
    private UnityStringEvent onRegistered = null;

    [SerializeField]
    private UnityStringEvent onValidated = null;

    [Title("Errors")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;

    [SerializeField]
    private UnityStringEvent onTimeoutError = null;

    // REGISTER
    public void RegisterPhone(long phoneCountryId, String phoneNumber)
    {
        RegisterPhonePostOperation registerPhonePostOp = new RegisterPhonePostOperation();
        try
        {
            registerPhonePostOp.phoneCountryId = phoneCountryId;
            registerPhonePostOp.phoneNumber = phoneNumber;

            registerPhonePostOp["on-complete"] = (Action<RegisterPhonePostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(registerPhonePostOp.result.Replace("\"", ""));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            registerPhonePostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // VALIDATION
    public void ValidateCode(PhoneCodeRequest phoneCodeRequest)
    {
        ValidateCodePostOperation validateCodePostOp = new ValidateCodePostOperation();
        try
        {
            validateCodePostOp.phoneCodeRequest = phoneCodeRequest;

            validateCodePostOp["on-complete"] = (Action<ValidateCodePostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onValidated.Invoke(validateCodePostOp.result.Replace("\"", ""));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            validateCodePostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
