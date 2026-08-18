using System;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using System.Collections.Generic;
using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class ReferredService : MonoBehaviour
{
    [Serializable]
    public class ReferredEvent : UnityEvent<List<Referred>> { }
    [Serializable]
    public class ReferredFullAllEvent : UnityEvent<ReferredFullAllRsp> { }


    [SerializeField]
    private ReferredEvent onRetreived = null;

    [SerializeField]
    private ReferredFullAllEvent onFullAllRetreived = null;

    [SerializeField]
    private UnityLongEvent onIdRetreived = null;

    [SerializeField]
    private UnityStringEvent onRegistered = null;

    [SerializeField]
    private UnityLongEvent onUpdated = null;


    [Title("Errors")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;

    [SerializeField]
    private UnityStringEvent onTimeoutError = null;


    // GET
    public void GetAll()
    {
        ReferredsGetOperation referredsGetOp = new ReferredsGetOperation();
        try
        {
            referredsGetOp["on-complete"] = (Action<ReferredsGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRetreived.Invoke(op.referreds);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            referredsGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetFullAllByCode(ReferredAllByCodeReq req)
    {
        ReferredFullAllByCodePostOperation referredFullAllPostOp = new ReferredFullAllByCodePostOperation();
        try
        {
            referredFullAllPostOp.req = req;
            referredFullAllPostOp["on-complete"] = (Action<ReferredFullAllByCodePostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFullAllRetreived.Invoke(op.rsp);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            referredFullAllPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetByAppUser(long appUserId)
    {
        ReferredGetOperation referredGetOp = new ReferredGetOperation();
        try
        {
            referredGetOp.appUserId = appUserId;
            referredGetOp["on-complete"] = (Action<ReferredGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRetreived.Invoke(op.referreds);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            referredGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    //public void GetHistory(DateTime startDate, DateTime endDate)
    //{
    //    HistoryGetOperation historyGetOp = new HistoryGetOperation();
    //    try
    //    {
    //        historyGetOp.referredHistoryRequest = new ReferredHistoryRequest(StateManager.Instance.AppUser.Id, startDate, endDate);
    //        historyGetOp["on-complete"] = (Action<HistoryGetOperation, HttpResponse>)((op, response) =>
    //        {
    //            if (response != null && !response.HasError)
    //                onRetreived.Invoke(op.referreds);
    //            else
    //                WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
    //        });
    //        historyGetOp.Send();
    //    }
    //    catch (Exception ex)
    //    {
    //        WebManager.Instance.OnSendError(ex.Message);
    //    }
    //}

    public void GetIdByCode(String code)
    {
        IdByCodeGetOperation idByCodeGetOp = new IdByCodeGetOperation();
        try
        {
            idByCodeGetOp.code = code;
            idByCodeGetOp["on-complete"] = (Action<IdByCodeGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdRetreived.Invoke(Convert.ToInt64(op.response));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            idByCodeGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // REGISTER
    public void Register(Referred referred)
    {
        ReferredRegisterOperation referredRegisterOp = new ReferredRegisterOperation();
        try
        {
            referredRegisterOp.referred = referred;
            referredRegisterOp["on-complete"] = (Action<ReferredRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(op.referredIds);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            referredRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // UPDATE
    public void UpdateReference(Referred referred)
    {
        ReferredPutOperation referredPutOp = new ReferredPutOperation();
        try
        {
            referredPutOp.referred = referred;
            referredPutOp["on-complete"] = (Action<ReferredPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdated.Invoke(op.referredlId);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            referredPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
