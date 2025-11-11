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
    public class ReferredFullsEvent : UnityEvent<List<ReferredFull>> { }


    [SerializeField]
    private ReferredEvent onRetreived = null;

    [SerializeField]
    private ReferredFullsEvent onFullsRetreived = null;

    [SerializeField]
    private UnityIntEvent onIdRetreived = null;

    [SerializeField]
    private UnityStringEvent onRegistered = null;

    [SerializeField]
    private UnityIntEvent onUpdated = null;


    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;


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
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            referredsGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetFullAll()
    {
        ReferredFullsGetOperation referredFullsGetOp = new ReferredFullsGetOperation();
        try
        {
            referredFullsGetOp["on-complete"] = (Action<ReferredFullsGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFullsRetreived.Invoke(op.referredFulls);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            referredFullsGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetByAppUser(int appUserId)
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
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            referredGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetHistory(DateTime startDate, DateTime endDate)
    {
        HistoryGetOperation historyGetOp = new HistoryGetOperation();
        try
        {
            historyGetOp.referredHistoryRequest = new ReferredHistoryRequest(StateManager.Instance.AppUser.Id, startDate, endDate);
            historyGetOp["on-complete"] = (Action<HistoryGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRetreived.Invoke(op.referreds);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            historyGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetIdByCode(String code)
    {
        IdByCodeGetOperation idByCodeGetOp = new IdByCodeGetOperation();
        try
        {
            idByCodeGetOp.code = code;
            idByCodeGetOp["on-complete"] = (Action<IdByCodeGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdRetreived.Invoke(Convert.ToInt32(op.response));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
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
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
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
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            referredPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
