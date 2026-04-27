using System;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using System.Collections.Generic;
using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class ServiceWishService : MonoBehaviour
{
    [Serializable]
    public class AllByTypeEvent : UnityEvent<ServiceWishAllRsp> { }

    [SerializeField]
    private AllByTypeEvent onAllRetreived = null;

    [SerializeField]
    private UnityLongEvent onRegistered = null;

    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;

    public void GetAllByType(ServiceWishAllByTypeReq req)
    {
        AllByTypePostOperation AllByTypePostOp = new AllByTypePostOperation();
        try
        {
            AllByTypePostOp.req = req;
            AllByTypePostOp["on-complete"] = (Action<AllByTypePostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onAllRetreived.Invoke(op.rsp);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            AllByTypePostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // REGISTER
    public void Register(ServiceWish serviceWish)
    {
        ServiceWishRegisterOperation serviceWishRegisterOp = new ServiceWishRegisterOperation();
        try
        {
            serviceWishRegisterOp.serviceWish = serviceWish;
            serviceWishRegisterOp["on-complete"] = (Action<ServiceWishRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt64(op.id));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            serviceWishRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}