using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class BankAccountService : MonoBehaviour
{
    [Serializable]
    public class BankAccountEvent : UnityEvent<BankAccount> { }


    [SerializeField]
    private BankAccountEvent onRetreived = null;

    [SerializeField]
    private UnityIntEvent onRegistered = null;

    [SerializeField]
    private UnityIntEvent onAdded = null;

    [SerializeField]
    private UnityIntEvent onUpdated = null;

    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;


    // GET
    public void GetBankAccount(int appUserId)
    {
        BankAccountGetOperation bankAccountGetOp = new BankAccountGetOperation();
        try
        {
            bankAccountGetOp.appUserId = appUserId;
            bankAccountGetOp["on-complete"] = (Action<BankAccountGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRetreived.Invoke(op.bankAccount);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            bankAccountGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // REGISTER
    public void Register(BankAccount bankAccount)
    {
        BankAccountRegisterOperation bankAccountRegisterPostOp = new BankAccountRegisterOperation();
        try
        {
            bankAccountRegisterPostOp.bankAccount = bankAccount;
            bankAccountRegisterPostOp["on-complete"] = (Action<BankAccountRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt32(op.id));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            bankAccountRegisterPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // ADD
    public void Add(BankAccount bankAccount)
    {
        BankAccountPostOperation bankAccountPostOp = new BankAccountPostOperation();
        try
        {
            bankAccountPostOp.bankAccount = bankAccount;
            bankAccountPostOp["on-complete"] = (Action<BankAccountPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onAdded.Invoke(Convert.ToInt32(op.id));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            bankAccountPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    //UPDATE
    public void UpdateBankAccount(BankAccount bankAccount)
    {
        BankAccountUpdateOperation bankAccountUpdateOp = new BankAccountUpdateOperation();
        try
        {
            bankAccountUpdateOp.bankAccount = bankAccount;
            bankAccountUpdateOp["on-complete"] = (Action<BankAccountUpdateOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdated.Invoke(Convert.ToInt32(op.id));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            bankAccountUpdateOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
