using System;
using System.Globalization;
using UnityEngine;

using Leap.Data.Collections;
using Leap.UI.Elements;
using Leap.UI.Dialog;

using Sirenix.OdinInspector;

public class InvestmentValidateAction : MonoBehaviour
{
    [Title("Investments")]
    [SerializeField]
    ListScroller lstInvDocInfos = null;
    [SerializeField]
    Text txtInvDocInfosEmpty = null;

    [Title("Display")]
    [SerializeField]
    Style stlItemBkg = null;
    [SerializeField]
    Style stlItemWait = null;
    [SerializeField]
    Image imgOverlay = null;

    [Title("Identity")]
    [SerializeField]
    Text txtFirstName1 = null;
    [SerializeField]
    Text txtFirstName2 = null;
    [SerializeField]
    Text txtLastName1 = null;
    [SerializeField]
    Text txtLastName2 = null;
    [SerializeField]
    ListScroller lstDocRtus = null;

    [Title("Economics")]
    [SerializeField]
    Text txtIncomes = null;
    [SerializeField]
    Text txtExpenses = null;
    [SerializeField]
    Text txtActivity = null;
    [SerializeField]
    ListScroller lstIncomes = null;
    [SerializeField]
    ListScroller lstDocIncomes = null;

    [Title("Investment")]
    [SerializeField]
    Text txtProject = null;
    [SerializeField]
    Text txtProduct = null;
    [SerializeField]
    Text txtAmount = null;

    [Title("Doc Bank")]
    [SerializeField]
    ListScroller lstDocBanks = null;

    [Title("Data")]
    [SerializeField]
    ValueList vllProductType = null;
    [SerializeField]
    ValueList vllIncomeType = null;
    [SerializeField]
    ValueList vllCurrency = null;

    [Title("Update")]
    [SerializeField]
    GameObject imgWrnComment = null;
    [SerializeField]
    InputField ifdWrnComment = null;
    [Title("Reject")]
    [SerializeField]
    GameObject imgDngComment = null;
    [SerializeField]
    ToggleGroup tggDngComment = null;
    [SerializeField]
    InputField ifdDngComment = null;

    private readonly String[] motives = { "", "RTU", "perfil económico", "constancias", "estados de cuenta" };
    private readonly CultureInfo cultureInfo = new CultureInfo("en-US");

    public bool Selected { get; set; } = false;

    InvestmentService investmentService = null;
    InvestmentDocInfo[] investmentDocInfos = null;

    InvestmentDocInfo investmentDocInfo = null;
    int investmentDocInfoIdx = -1;

    private void Awake()
    {
        investmentService = GetComponent<InvestmentService>();
    }

    public void Clear()
    {
        txtFirstName1.Clear();
        txtFirstName2.Clear();
        txtLastName1.Clear();
        txtLastName2.Clear();
        lstDocRtus.ApplyClearValues();

        txtIncomes.Clear();
        txtExpenses.Clear();
        txtActivity.Clear();
        lstIncomes.ClearValues();
        lstDocIncomes.ClearValues();

        txtProject.Clear();
        txtProduct.Clear();
        txtAmount.Clear();

        lstDocBanks.ApplyClearValues();
    }

    public void GetInvestmentDocInfos()
    {
        ScreenDialog.Instance.Display();

        lstInvDocInfos.ApplyClearValues();
        txtInvDocInfosEmpty.gameObject.SetActive(false);

        investmentDocInfos = null;
        investmentService.GetDocInfosByStatus(9);
    }

    public void FillInvestmentDocInfos(InvestmentDocInfo[] investmentDocInfos)
    {
        this.investmentDocInfos = investmentDocInfos;

        if (investmentDocInfos.Length == 0)
        {
            lstInvDocInfos.ApplyClearValues();
            txtInvDocInfosEmpty.gameObject.SetActive(true);
            StateManager.Instance.BoardLoadHide();
            return;
        }

        FillDocInfos();
    }

    private void FillDocInfos()
    {
        if (StateManager.Instance.ProjectProductFulls.Count == 0 || StateManager.Instance.IdentityFulls.Count == 0)
        {
            Invoke(nameof(FillDocInfos), 0.2f);
            return;
        }

        lstInvDocInfos.ClearValues();

        ListScrollerValue lstInvDocInfoValue;
        for (int i = 0; i < investmentDocInfos.Length; i++)
        {
            ProjectProductFull projectProductFull = StateManager.Instance.GetProjectProductFull(investmentDocInfos[i].Investment.ProjectId);
            IdentityFull identityFull = StateManager.Instance.GetIdentityFull(investmentDocInfos[i].Investment.AppUserId);

            lstInvDocInfoValue = new ListScrollerValue(5, false);
            lstInvDocInfoValue.SetStyle(0, investmentDocInfos[i].Investment.InvestmentMotiveId == 0 ? stlItemBkg : stlItemWait);
            lstInvDocInfoValue.SetText(1, $"{identityFull.FirstName1}{(identityFull.FirstName2 == null ? "" : " " + identityFull.FirstName2)} {identityFull.LastName1}{(identityFull.LastName2 == null ? "" : " " + identityFull.LastName2)}");
            lstInvDocInfoValue.SetText(2, projectProductFull.ProjectFull.Name);
            lstInvDocInfoValue.SetText(3, vllProductType.FindRecordCellString(investmentDocInfos[i].Investment.ProductTypeId, "Name"));
            lstInvDocInfoValue.SetText(4, GetStringAmount(projectProductFull.ProjectFull.CurrencySymbol, investmentDocInfos[i].Investment.TotalAmount, 2));

            lstInvDocInfos.AddValue(lstInvDocInfoValue);
        }

        lstInvDocInfos.ApplyValues();

        StateManager.Instance.BoardLoadHide();
    }

    public void Display(int idx)
    {
        investmentDocInfo = investmentDocInfos[idx];
        investmentDocInfoIdx = idx;

        IdentityFull identityFull = StateManager.Instance.GetIdentityFull(investmentDocInfo.Investment.AppUserId);
        ProjectProductFull projectProductFull = StateManager.Instance.GetProjectProductFull(investmentDocInfo.Investment.ProjectId);

        DisplayIdentity(identityFull);
        DisplayInvestment(projectProductFull);
        DisplayEconomics(projectProductFull);
        DisplayDocBanks();

        imgOverlay.gameObject.SetActive(investmentDocInfo.Investment.InvestmentMotiveId != 0);

        ScreenDialog.Instance.Hide();
    }

    private String GetStringAmount(String currencySymbol, double value, int decimals = 0)
    {
        return $"{currencySymbol} {value.ToString($"N{decimals}", cultureInfo)}";
    }

    //private String GetStringPourcent(double value, int decimals = 0)
    //{
    //    return $"{(value * 100d).ToString($"N{decimals}", cultureInfo)} %";
    //}

    private void DisplayIdentity(IdentityFull identityFull)
    {
        txtFirstName1.TextValue = identityFull.FirstName1;
        txtFirstName2.TextValue = identityFull.FirstName2;
        txtLastName1.TextValue = identityFull.LastName1;
        txtLastName2.TextValue = identityFull.LastName2;

        if (investmentDocInfo.DocRtuSprites.Length == 0)
        {
            lstDocRtus.ApplyClearValues();
            return;
        }

        lstDocRtus.ClearValues();

        ListScrollerValue lstDocRtuValue;
        for (int i = 0; i < investmentDocInfo.DocRtuSprites.Length; i++)
        {
            lstDocRtuValue = new ListScrollerValue(1, false);
            lstDocRtuValue.SetSprite(0, investmentDocInfo.DocRtuSprites[i]);

            lstDocRtus.AddValue(lstDocRtuValue);
        }

        lstDocRtus.ApplyValues();
    }

    public void ZoomDocRtu(int idx)
    {
        ZoomDialog.Instance.Display(lstDocRtus[idx].GetSprite(0));
    }

    private void DisplayInvestment(ProjectProductFull projectProductFull)
    {
        txtProject.TextValue = projectProductFull.ProjectFull.Name;
        txtProduct.TextValue = vllProductType.FindRecordCellString(investmentDocInfo.Investment.ProductTypeId, "Name");
        txtAmount.TextValue = GetStringAmount(projectProductFull.ProjectFull.CurrencySymbol, investmentDocInfo.Investment.TotalAmount, 2);
    }

    private void DisplayEconomics(ProjectProductFull projectProductFull)
    {
        txtIncomes.TextValue = GetStringAmount(vllCurrency.FindRecordCellString(investmentDocInfo.EconomicsInfo.Economics.IncomeCurrencyId, 2), investmentDocInfo.EconomicsInfo.Economics.IncomeAmount, 2);
        txtExpenses.TextValue = GetStringAmount(vllCurrency.FindRecordCellString(investmentDocInfo.EconomicsInfo.Economics.ExpensesCurrencyId, 2), investmentDocInfo.EconomicsInfo.Economics.ExpensesAmount, 2);
        txtActivity.TextValue = investmentDocInfo.EconomicsInfo.Economics.Activity;

        if (investmentDocInfo.EconomicsInfo.DocIncomeSprites.Length == 0)
        {
            lstDocIncomes.ApplyClearValues();
        }
        else
        {
            lstDocIncomes.ClearValues();

            ListScrollerValue lstDocIncomeValue;
            for (int i = 0; i < investmentDocInfo.EconomicsInfo.DocIncomeSprites.Length; i++)
            {
                lstDocIncomeValue = new ListScrollerValue(1, false);
                lstDocIncomeValue.SetSprite(0, investmentDocInfo.EconomicsInfo.DocIncomeSprites[i]);

                lstDocIncomes.AddValue(lstDocIncomeValue);
            }

            lstDocIncomes.ApplyValues();
        }

        if (investmentDocInfo.EconomicsInfo.Incomes.Length == 0)
        {
            lstIncomes.ApplyClearValues();
        }
        else
        {
            lstIncomes.ClearValues();

            ListScrollerValue lstIncomeValue;
            for (int i = 0; i < investmentDocInfo.EconomicsInfo.Incomes.Length; i++)
            {
                lstIncomeValue = new ListScrollerValue(2, false);
                lstIncomeValue.SetText(0, vllIncomeType.FindRecordCellString(investmentDocInfo.EconomicsInfo.Incomes[i].IncomeTypeId, "Name"));
                lstIncomeValue.SetText(1, investmentDocInfo.EconomicsInfo.Incomes[i].Detail);

                lstIncomes.AddValue(lstIncomeValue);
            }

            lstIncomes.ApplyValues();
        }
    }

    public void ZoomDocIncome(int idx)
    {
        ZoomDialog.Instance.Display(lstDocIncomes[idx].GetSprite(0));
    }

    private void DisplayDocBanks()
    {
        if (investmentDocInfo.DocBankSprites.Length == 0)
        {
            lstDocBanks.ApplyClearValues();
            return;
        }

        lstDocBanks.ClearValues();

        ListScrollerValue lstDocBankValue;
        for (int i = 0; i < investmentDocInfo.DocBankSprites.Length; i++)
        {
            lstDocBankValue = new ListScrollerValue(1, false);
            lstDocBankValue.SetSprite(0, investmentDocInfo.DocBankSprites[i]);

            lstDocBanks.AddValue(lstDocBankValue);
        }

        lstDocBanks.ApplyValues();
    }

    public void ZoomDocBank(int idx)
    {
        ZoomDialog.Instance.Display(lstDocBanks[idx].GetSprite(0));
    }

    // Validation

    int motiveId = -1;

    public void DisplayRequestUpdate(int motiveId)
    {
        this.motiveId = motiveId;
        ifdWrnComment.Clear();
        imgWrnComment.SetActive(true);
    }

    public void RequestUpdate()
    {
        if (!ifdWrnComment.IsValid())
        {
            ChoiceDialog.Instance.Error("Inversiones", "El comentario es obligatorio.");
            return;
        }

        ChoiceDialog.Instance.Info("Inversiones", $"Estás seguro de pedir una actualización de {motives[motiveId]}?", SendRequestUpdate, null, "Sí", "No");
    }

    private void SendRequestUpdate()
    {
        imgWrnComment.SetActive(false);
        investmentService.RequestUpdate(new InvestmentBoardResponse(investmentDocInfo.Investment.Id, investmentDocInfo.Investment.AppUserId, StateManager.Instance.BoardUser.Id, motiveId, ifdWrnComment.Text));
    }

    public void Authorize()
    {
        ChoiceDialog.Instance.Info("Inversiones", $"Estás seguro de autorizar esta inversión?", SendAuthorize, null, "Sí", "No");
    }

    private void SendAuthorize()
    {
        investmentService.Authorize(new InvestmentBoardResponse(investmentDocInfo.Investment.Id, investmentDocInfo.Investment.AppUserId, StateManager.Instance.BoardUser.Id, 0, null));
    }

    public void DisplayReject()
    {
        tggDngComment.Value = "0";
        ifdDngComment.Clear();
        imgDngComment.SetActive(true);
    }

    public void Reject()
    {
        if (!ifdDngComment.IsValid())
        {
            ChoiceDialog.Instance.Error("Inversiones", "El comentario es obligatorio.");
            return;
        }

        ChoiceDialog.Instance.Info("Inversiones", $"Estás seguro de rechazar esta inversión?", SendReject, null, "Sí", "No");
    }

    private void SendReject()
    {
        imgDngComment.SetActive(false);
        motiveId = int.Parse(tggDngComment.Value);
        investmentService.Reject(new InvestmentBoardResponse(investmentDocInfo.Investment.Id, investmentDocInfo.Investment.AppUserId, StateManager.Instance.BoardUser.Id, motiveId, ifdDngComment.Text));
    }
}
