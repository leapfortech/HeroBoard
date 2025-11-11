using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class InvestmentService : MonoBehaviour
{
    [Serializable]
    public class FullsEvent : UnityEvent<InvestmentResponse> { }

    [Serializable]
    public class FractionatedFullsEvent : UnityEvent<InvestmentFractionatedFull[]> { }
    [Serializable]
    public class FinancedFullsEvent : UnityEvent<InvestmentFinancedFull[]> { }
    [Serializable]
    public class PrepaidFullsEvent : UnityEvent<InvestmentPrepaidFull[]> { }
    [Serializable]
    public class FractionatedFullEvent : UnityEvent<InvestmentFractionatedFull> { }
    [Serializable]
    public class FinancedFullEvent : UnityEvent<InvestmentFinancedFull> { }
    [Serializable]
    public class PrepaidFullEvent : UnityEvent<InvestmentPrepaidFull> { }
    [Serializable]
    public class BankPaymentFullsEvent : UnityEvent<InvestmentPaymentBankFull[]> { }
    [Serializable]
    public class DocInfosEvent : UnityEvent<InvestmentDocInfo[]> { }
    [Serializable]
    public class PaymentEvent : UnityEvent<InvestmentPayment> { }


    [Title("Full")]
    [SerializeField]
    private FullsEvent onFullsRetreived = null;
    [SerializeField]
    private FractionatedFullsEvent onFractionatedFullsRetreived = null;
    [SerializeField]
    private FinancedFullsEvent onFinancedFullsRetreived = null;
    [SerializeField]
    private PrepaidFullsEvent onPrepaidFullsRetreived = null;

    [Space]
    [SerializeField]
    private BankPaymentFullsEvent onBankPaymentFullsRetreived = null;
    [SerializeField]
    private DocInfosEvent onDocInfosRetreived = null;

    [Title("Register")]
    [SerializeField]
    private UnityIntEvent onRegistered = null;

    [SerializeField]
    private UnityEvent onDocRegistered = null;

    [SerializeField]
    private UnityIntsEvent onReferencesRegistered = null;

    [SerializeField]
    private UnityIntsEvent onIdentitysRegistered = null;
    [SerializeField]
    private UnityEvent onIdentitysCreated = null;

    [Title("Validate")]
    [SerializeField]
    private UnityEvent onUpdateRequested = null;
    [SerializeField]
    private UnityEvent onAuthorized = null;
    [SerializeField]
    private UnityEvent onRejected = null;

    [Title("Payment")]
    [SerializeField]
    private PaymentEvent onPaid = null;
    [SerializeField]
    private UnityEvent onPaymentAccepted = null;
    [SerializeField]
    private UnityEvent onPaymentRejected = null;
    //[SerializeField]
    //private PaymentEvent onPaymentAck = null;


    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;


    // GET
    public void GetFullsByStatus(int status = -1)
    {
        FullsByStatusGetOperation fullsByStatusGetOp = new FullsByStatusGetOperation();
        try
        {
            fullsByStatusGetOp.status = status;
            fullsByStatusGetOp["on-complete"] = (Action<FullsByStatusGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFullsRetreived.Invoke(op.investmentResponse);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            fullsByStatusGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetFullsByAppUserId(int appUserId)
    {
        FullsByAppUserIdGetOperation fullsByAppUserIdGetOp = new FullsByAppUserIdGetOperation();
        try
        {
            fullsByAppUserIdGetOp.appUserId = appUserId;
            fullsByAppUserIdGetOp["on-complete"] = (Action<FullsByAppUserIdGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFullsRetreived.Invoke(op.investmentResponse);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            fullsByAppUserIdGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetFractionatedFullsByStatus(int status)
    {
        FractionatedFullsByStatusGetOperation fractionatedFullsByStatusGetOp = new FractionatedFullsByStatusGetOperation();
        try
        {
            fractionatedFullsByStatusGetOp.status = status;
            fractionatedFullsByStatusGetOp["on-complete"] = (Action<FractionatedFullsByStatusGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFractionatedFullsRetreived.Invoke(op.investmentFractionatedFulls);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            fractionatedFullsByStatusGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetFractionatedFullsByAppUserId(int appUserId, int status = -1)
    {
        FractionatedFullsByAppUserIdGetOperation fractionatedFullsByAppUserIdGetOp = new FractionatedFullsByAppUserIdGetOperation();
        try
        {
            fractionatedFullsByAppUserIdGetOp.appUserId = appUserId;
            fractionatedFullsByAppUserIdGetOp.status = status;
            fractionatedFullsByAppUserIdGetOp["on-complete"] = (Action<FractionatedFullsByAppUserIdGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFractionatedFullsRetreived.Invoke(op.investmentIntallmentFulls);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            fractionatedFullsByAppUserIdGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetFinancedFulls(int status)
    {
        FinancedFullsByStatusGetOperation financedFullsByStatusGetOp = new FinancedFullsByStatusGetOperation();
        try
        {
            financedFullsByStatusGetOp.status = status;
            financedFullsByStatusGetOp["on-complete"] = (Action<FinancedFullsByStatusGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFinancedFullsRetreived.Invoke(op.investmentFinancedFulls);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            financedFullsByStatusGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetFinancedFullsByAppUserId(int appUserId, int status = -1)
    {
        FinancedFullsByAppUserIdGetOperation financedFullsByAppUserIdGetOp = new FinancedFullsByAppUserIdGetOperation();
        try
        {
            financedFullsByAppUserIdGetOp.appUserId = appUserId;
            financedFullsByAppUserIdGetOp.status = status;
            financedFullsByAppUserIdGetOp["on-complete"] = (Action<FinancedFullsByAppUserIdGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFinancedFullsRetreived.Invoke(op.investmentFinancedFulls);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            financedFullsByAppUserIdGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetPrepaidFullsByStatus(int status)
    {
        PrepaidFullsByStatusGetOperation prepaidFullsByStatusGetOp = new PrepaidFullsByStatusGetOperation();
        try
        {
            prepaidFullsByStatusGetOp.status = status;
            prepaidFullsByStatusGetOp["on-complete"] = (Action<PrepaidFullsByStatusGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onPrepaidFullsRetreived.Invoke(op.investmentPrepaidFulls);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            prepaidFullsByStatusGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetPrepaidFullsByAppUserId(int appUserId, int status = -1)
    {
        PrepaidFullsByAppUserIdGetOperation prepaidFullsByAppUserIdGetOp = new PrepaidFullsByAppUserIdGetOperation();
        try
        {
            prepaidFullsByAppUserIdGetOp.appUserId = appUserId;
            prepaidFullsByAppUserIdGetOp.status = status;
            prepaidFullsByAppUserIdGetOp["on-complete"] = (Action<PrepaidFullsByAppUserIdGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onPrepaidFullsRetreived.Invoke(op.investmentPrepaidFulls);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            prepaidFullsByAppUserIdGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetBankPaymentFullsByStatus(int status)
    {
        BankPaymentFullsStatusGetOperation bankTransactionStatusGetOp = new BankPaymentFullsStatusGetOperation();
        try
        {
            bankTransactionStatusGetOp.status = status;
            bankTransactionStatusGetOp["on-complete"] = (Action<BankPaymentFullsStatusGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onBankPaymentFullsRetreived.Invoke(op.bankPaymentFulls);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            bankTransactionStatusGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // REGISTER
    public void Register(Investment investment)
    {
        RegisterOperation registerOp = new RegisterOperation();
        try
        {
            registerOp.investment = investment;
            registerOp["on-complete"] = (Action<RegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt32(op.id));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            registerOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // DOCS
    public void GetDocInfosByStatus(int status)
    {
        DocInfosByStatusGetOperation docInfosStatusGetOp = new DocInfosByStatusGetOperation();
        try
        {
            docInfosStatusGetOp.status = status;
            docInfosStatusGetOp["on-complete"] = (Action<DocInfosByStatusGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onDocInfosRetreived.Invoke(op.investmentDocInfos);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            docInfosStatusGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void RegisterDocRtu(InvestmentDocRequest investmentDocRequest)
    {
        DocRtuRegisterOperation docRTURegisterOp = new DocRtuRegisterOperation();
        try
        {
            docRTURegisterOp.investmentDocRequest = investmentDocRequest;
            docRTURegisterOp["on-complete"] = (Action<DocRtuRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onDocRegistered.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            docRTURegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void RegisterDocIncome(InvestmentDocRequest investmentDocRequest)
    {
        DocIncomeRegisterOperation docIncomeRegisterOp = new DocIncomeRegisterOperation();
        try
        {
            docIncomeRegisterOp.investmentDocRequest = investmentDocRequest;
            docIncomeRegisterOp["on-complete"] = (Action<DocIncomeRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onDocRegistered.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            docIncomeRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void RegisterDocBank(InvestmentDocRequest investmentDocRequest)
    {
        DocBankRegisterOperation docBankRegisterOp = new DocBankRegisterOperation();
        try
        {
            docBankRegisterOp.investmentDocRequest = investmentDocRequest;
            docBankRegisterOp["on-complete"] = (Action<DocBankRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onDocRegistered.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            docBankRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // REFERENCES

    public void RegisterReferences(InvestmentReference[] investmentReferences)
    {
        ReferencesRegisterOperation referencesRegisterOp = new ReferencesRegisterOperation();
        try
        {
            referencesRegisterOp.investmentReferences = investmentReferences;
            referencesRegisterOp["on-complete"] = (Action<ReferencesRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onReferencesRegistered.Invoke(op.ids);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            referencesRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // SIGNATORY

    public void RegisterSignatories(InvestmentIdentityRequest[] investmentIdentityRequests)
    {
        SignatoryRegisterOperation signatoriesRegisterOp = new SignatoryRegisterOperation();
        try
        {
            signatoriesRegisterOp.investmentIdentityRequests = investmentIdentityRequests;
            signatoriesRegisterOp["on-complete"] = (Action<SignatoryRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdentitysRegistered.Invoke(op.ids);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            signatoriesRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void CreateSignatory(int investmentId)
    {
        SignatoryCreateOperation signatoryCreateOp = new SignatoryCreateOperation();
        try
        {
            signatoryCreateOp.investmentId = investmentId;
            signatoryCreateOp["on-complete"] = (Action<SignatoryCreateOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdentitysCreated.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            signatoryCreateOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // BENEFICIARY

    public void RegisterBeneficiaries(InvestmentIdentityRequest[] investmentIdentityRequests)
    {
        BeneficiaryRegisterOperation beneficiariesRegisterOp = new BeneficiaryRegisterOperation();
        try
        {
            beneficiariesRegisterOp.investmentIdentityRequests = investmentIdentityRequests;
            beneficiariesRegisterOp["on-complete"] = (Action<BeneficiaryRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdentitysRegistered.Invoke(op.ids);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            beneficiariesRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void CreateBeneficiary(int investmentId)
    {
        BeneficiaryCreateOperation beneficiaryCreateOp = new BeneficiaryCreateOperation();
        try
        {
            beneficiaryCreateOp.investmentId = investmentId;
            beneficiaryCreateOp["on-complete"] = (Action<BeneficiaryCreateOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdentitysCreated.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            beneficiaryCreateOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // VALIDATION

    public void RequestUpdate(InvestmentBoardResponse boardResponse)
    {
        RequestUpdateOperation requestUpdateOp = new RequestUpdateOperation();
        try
        {
            requestUpdateOp.boardResponse = boardResponse;
            requestUpdateOp["on-complete"] = (Action<RequestUpdateOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdateRequested.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            requestUpdateOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void Authorize(InvestmentBoardResponse boardResponse)
    {
        AuthorizeOperation authorizePutOp = new AuthorizeOperation();
        try
        {
            authorizePutOp.boardResponse = boardResponse;
            authorizePutOp["on-complete"] = (Action<AuthorizeOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onAuthorized.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            authorizePutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void Reject(InvestmentBoardResponse boardResponse)
    {
        RejectOperation rejectPutOp = new RejectOperation();
        try
        {
            rejectPutOp.boardResponse = boardResponse;
            rejectPutOp["on-complete"] = (Action<RejectOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRejected.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            rejectPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // PAYMENT

    public void BankPayment(InvestmentBankPayment request)
    {
        InvestmentPaymentBankPostOperation investmentBankPaymentPostOp = new InvestmentPaymentBankPostOperation();
        try
        {
            investmentBankPaymentPostOp.request = request;
            investmentBankPaymentPostOp["on-complete"] = (Action<InvestmentPaymentBankPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onPaid.Invoke(op.investmentPayment);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            investmentBankPaymentPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void CardPayment(InvestmentCardPayment request)
    {
        InvestmentPaymentCardPostOperation investmentCardPaymentPostOp = new InvestmentPaymentCardPostOperation();
        try
        {
            investmentCardPaymentPostOp.request = request;
            investmentCardPaymentPostOp["on-complete"] = (Action<InvestmentPaymentCardPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onPaid.Invoke(op.investmentPayment);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            investmentCardPaymentPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void PaymentAuthorize(int boardUserId, int paymentId)
    {
        PaymentAuthorizePostOperation paymentAuthorizePostOp = new PaymentAuthorizePostOperation();
        try
        {
            paymentAuthorizePostOp.boardUserId = boardUserId;
            paymentAuthorizePostOp.paymentId = paymentId;
            paymentAuthorizePostOp["on-complete"] = (Action<PaymentAuthorizePostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onPaymentAccepted.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            paymentAuthorizePostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void PaymentReject(int boardUserId, int paymentId, bool receipt)
    {
        PaymentRejectPostOperation paymentRejectPostOp = new PaymentRejectPostOperation();
        try
        {
            paymentRejectPostOp.boardUserId = boardUserId;
            paymentRejectPostOp.paymentId = paymentId;
            paymentRejectPostOp.receipt = receipt ? 1 : 0;
            paymentRejectPostOp["on-complete"] = (Action<PaymentRejectPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onPaymentRejected.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            paymentRejectPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    //public void PaymentAcknowledge(int investmentPaymentId)
    //{
    //    PaymentAcknowledgeOperation paymentAckOp = new PaymentAcknowledgeOperation();
    //    try
    //    {
    //        paymentAckOp.investmentPaymentId = investmentPaymentId;
    //        paymentAckOp["on-complete"] = (Action<PaymentAcknowledgeOperation, HttpResponse>)((op, response) =>
    //        {
    //            if (response != null && !response.HasError)
    //                onPaymentAck.Invoke(paymentAckOp.investmentPayment);
    //            else
    //                onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
    //        });
    //        paymentAckOp.Send();
    //    }
    //    catch (Exception ex)
    //    {
    //        WebManager.Instance.OnSendError(ex.Message);
    //    }
    //}
}
