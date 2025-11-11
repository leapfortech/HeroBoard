using System;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class RenapService : MonoBehaviour
{
    [Serializable]
    public class RenapIdentityInfoEvent : UnityEvent<RenapIdentityInfo> { }

    [SerializeField]
    private RenapIdentityInfoEvent onIdentityInfoRetreived = null;

    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;

    // Renap
    public void GetIdentityInfo(int appUserId)
    {
        RenapIdentityInfoGetOperation renapIdentityGetOp = new RenapIdentityInfoGetOperation();
        try
        {
            renapIdentityGetOp.appUserId = appUserId;
            renapIdentityGetOp["on-complete"] = (Action<RenapIdentityInfoGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdentityInfoRetreived.Invoke(op.renapIdentityInfo);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            renapIdentityGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetIdentityInfoByCui(String cui)
    {
        RenapIdentityInfoCuiGetOperation renapIdentityCuiGetOp = new RenapIdentityInfoCuiGetOperation();
        try
        {
            renapIdentityCuiGetOp.cui = cui;
            renapIdentityCuiGetOp["on-complete"] = (Action<RenapIdentityInfoCuiGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdentityInfoRetreived.Invoke(op.renapIdentityInfo);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            renapIdentityCuiGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
