using System;
using System.Globalization;
using UnityEngine;

using Leap.Data.Mapper;
using Leap.Graphics.Tools;
using Leap.UI.Elements;
using Leap.UI.Dialog;

using Sirenix.OdinInspector;

public class PaymentAction : MonoBehaviour
{
    [Title("List")]
    [SerializeField]
    ListScroller lstPayments = null;
    [SerializeField]
    Text txtPaymentsEmpty = null;

    [Title("Payment")]
    [SerializeField]
    Text lblPayment = null;

    [SerializeField]
    Text txtAmount = null;

    [SerializeField]
    Text txtAmountUSD = null;

    [SerializeField]
    Image imgReceipt = null;

    [SerializeField]
    DataMapper dtmBankPayment = null;

    [Title("Action")]
    [SerializeField]
    Button btnAuthorize = null;

    [SerializeField]
    Button btnReject = null;

    [SerializeField]
    Button btnRejectReceipt = null;

    public bool Selected { get; set; } = false;
    InvestmentService investmentService = null;
    double exchangeRate = 7.90d;

    private void Awake()
    {
        investmentService = GetComponent<InvestmentService>();

        btnAuthorize.AddAction(Authorize);
        btnReject.AddAction(Reject);
        btnRejectReceipt.AddAction(RejectReceipt);
    }

    private void Start()
    {
        exchangeRate = Convert.ToDouble(AppManager.Instance.GetParamValue("ExchangeRate"), CultureInfo.InvariantCulture);
    }

    // Clear
    public void Clear()
    {
        StateManager.Instance.PaymentBankIdx = -1;
        lblPayment.TextValue = "Cobro";
        dtmBankPayment.ClearElements();
        txtAmount.Clear();
        txtAmountUSD.Clear();
        imgReceipt.Sprite?.Destroy();
        imgReceipt.Clear();
        imgReceipt.gameObject.SetActive(false);
        btnAuthorize.gameObject.SetActive(false);
        btnReject.gameObject.SetActive(false);
    }

    // Payments

    public void GetPayments()
    {
        ScreenDialog.Instance.Display();
        Clear();
        txtPaymentsEmpty.gameObject.SetActive(false);

        investmentService.GetBankPaymentFullsByStatus(2);
    }

    public void FillPayments(InvestmentPaymentBankFull[] bankPaymentFulls)
    {
        StateManager.Instance.PaymentBanks = bankPaymentFulls;

        if (bankPaymentFulls.Length == 0)
        {
            lstPayments.ApplyClearValues();
            txtPaymentsEmpty.gameObject.SetActive(true);
            StateManager.Instance.BoardLoadHide();
            return;
        }

        lstPayments.ClearValues();

        ListScrollerValue lstPaymentValue;
        for (int i = 0; i < bankPaymentFulls.Length; i++)
        {
            lstPaymentValue = new ListScrollerValue(5, true);

            lstPaymentValue.SetText(0, $"{bankPaymentFulls[i].SendDateTime:dd/MM/yyyy}");
            lstPaymentValue.SetText(1, $"{bankPaymentFulls[i].SendDateTime:HH:mm}");
            lstPaymentValue.SetText(2, bankPaymentFulls[i].FirstName1 + (!String.IsNullOrEmpty(bankPaymentFulls[i].FirstName2) ? " " + bankPaymentFulls[i].FirstName2 : "") + " " +
                                       bankPaymentFulls[i].LastName1 + (!String.IsNullOrEmpty(bankPaymentFulls[i].LastName2) ? " " + bankPaymentFulls[i].LastName2 : ""));
            lstPaymentValue.SetText(3, $"{bankPaymentFulls[i].AccountHpb.Split(" - ")[0]}");
            lstPaymentValue.SetText(4, $"{bankPaymentFulls[i].CurrencySymbol} " + bankPaymentFulls[i].Amount.ToString("F2", CultureInfo.InvariantCulture));

            lstPayments.AddValue(lstPaymentValue);
        }

        lstPayments.ApplyValues();
        lstPayments.CheckToggle(0, true);

        StateManager.Instance.BoardLoadHide();
    }

    // Payment

    public void Display(int bankPaymentIdx)
    {
        if (bankPaymentIdx == StateManager.Instance.PaymentBankIdx)
            return;

        StateManager.Instance.PaymentBankIdx = bankPaymentIdx;
        InvestmentPaymentBankFull bankPayment = StateManager.Instance.PaymentBank;

        dtmBankPayment.PopulateClass<InvestmentPaymentBankFull>(bankPayment);
        txtAmount.TextValue = $"{bankPayment.CurrencySymbol} " + bankPayment.Amount.ToString("F2", CultureInfo.InvariantCulture);
        if (bankPayment.CurrencySymbol == "$")
            txtAmountUSD.Clear();
        else
            txtAmountUSD.TextValue = "($ " + Math.Round(bankPayment.Amount / exchangeRate, 2).ToString("F2", CultureInfo.InvariantCulture) + ")";

        btnAuthorize.gameObject.SetActive(true);
        btnReject.gameObject.SetActive(true);

        // Transferencia
        if (bankPayment.TransactionTypeId == 1)
        {
            lblPayment.TextValue = "Transferencia";
            btnAuthorize.Title = "Autorizar la transferencia";
            btnAuthorize.SetStyle();
            btnReject.Title = "Rechazar la transferencia";
            btnReject.SetStyle();
            imgReceipt.Sprite?.Destroy();
            imgReceipt.gameObject.SetActive(false);
            ScreenDialog.Instance.Hide();
            return;
        }

        // Depósito
        lblPayment.TextValue = "Depósito";
        btnAuthorize.Title = "Autorizar el depósito";
        btnAuthorize.SetStyle();
        btnReject.Title = "Rechazar el depósito";
        btnReject.SetStyle();
        imgReceipt.gameObject.SetActive(true);
        imgReceipt.Sprite?.Destroy();
        imgReceipt.Sprite = bankPayment.Receipt.CreateSprite("Receipt");
        ScreenDialog.Instance.Hide();
    }

    public void ZoomReceipt()
    {
        ZoomDialog.Instance.Display(imgReceipt.Sprite);
    }

    // Validation
    private void Authorize()
    {
        String transaction = StateManager.Instance.PaymentBank.TransactionTypeId == 1 ? "esta transferencia" : "este depósito";
        ChoiceDialog.Instance.Info("Inversiones", $"Estás seguro de autorizar {transaction}?", SendAuthorize, null, "Sí", "No");
    }

    private void SendAuthorize()
    {
        ScreenDialog.Instance.Display();
        investmentService.PaymentAuthorize(StateManager.Instance.BoardUser.Id, StateManager.Instance.PaymentBank.Id);
    }

    private void Reject()
    {
        String transaction = StateManager.Instance.PaymentBank.TransactionTypeId == 1 ? "esta transferencia" : "este depósito";
        ChoiceDialog.Instance.Info("Inversiones", $"Estás seguro de rechazar {transaction}?", SendReject, null, "Sí", "No");
    }

    private void SendReject()
    {
        ScreenDialog.Instance.Display();
        investmentService.PaymentReject(StateManager.Instance.BoardUser.Id, StateManager.Instance.PaymentBank.Id, false);
    }

    private void RejectReceipt()
    {
        ChoiceDialog.Instance.Info("Inversiones", $"Estás seguro de rechazar el recibo de este depósito?", SendRejectReceipt, null, "Sí", "No");
    }

    private void SendRejectReceipt()
    {
        ScreenDialog.Instance.Display();
        investmentService.PaymentReject(StateManager.Instance.BoardUser.Id, StateManager.Instance.PaymentBank.Id, true);
    }
}
