using System;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class AlertService : MonoBehaviour
{
    [Serializable]
    public class AlertGetEvent : UnityEvent<Alert> { }

    [Serializable]
    public class AlertGetByCustomerEvent : UnityEvent<Alert[]> { }

    [SerializeField]
    private AlertGetEvent onRetreived = null;

    [SerializeField]
    private AlertGetByCustomerEvent onAlertsRetreived = null;

    [SerializeField]
    private UnityEvent onAdded = null;

    [SerializeField]
    private UnityEvent onDelete = null;

    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;

    // GET
    public void GetAlert(int alertId)
    {
        AlertGetOperation alertGetOp = new AlertGetOperation();
        try
        {
            alertGetOp.alertId = alertId;
            alertGetOp["on-complete"] = (Action<AlertGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRetreived.Invoke(op.alert);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            alertGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetAlertByCustomer(int customerId)
    {
        AlertGetByCustomerOperation alertGetByCustomerOp = new AlertGetByCustomerOperation();
        try
        {
            alertGetByCustomerOp.customerId = customerId;
            alertGetByCustomerOp["on-complete"] = (Action<AlertGetByCustomerOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onAlertsRetreived.Invoke(op.alerts);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            alertGetByCustomerOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }


    // ADD
    public void Add(Alert alert)
    {
        AlertPostOperation alertPostOp = new AlertPostOperation();
        try
        {
            alertPostOp.alert = alert;
            alertPostOp["on-complete"] = (Action<AlertPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onAdded.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            alertPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // DELETE
    public void Delete(int alertId)
    {
        AlertPutOperation alertPutOp = new AlertPutOperation();
        try
        {
            alertPutOp.alertId = alertId;
            alertPutOp["on-complete"] = (Action<AlertPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onDelete.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            alertPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
