using System;
using System.Collections.Generic;


public class InvestmentFractionatedFull : InvestmentFull
{
    public int Id { get; set; }
    public int ProductFractionatedId { get; set; }
    public double Amount { get; set; }
    public int InstallmentCount { get; set; }
    public int Status { get; set; }

    public InvestmentFractionatedFull()
    {
    }

    public InvestmentFractionatedFull(int id, int investmentId, int productFractionatedId,
                                      int projectId, int productTypeId, int appUserId, DateTime effectiveDate, int developmentTerm, int cpiCount, double totalAmount, double reserveAmount, double dueAmount,
                                      double discountRate, double discountAmount, double balance, DateTime? completionDate, String docuSignReference, int boardUserId, int investmentMotiveId, String boardComment, int investmentStatusId,
                                      double amount, int installmentCount, int status, List<InvestmentBankPayment> investmentBankPayments, List<InvestmentInstallmentInfo> investmentInstallmentInfos)
                                      : base(investmentId, projectId, productTypeId, appUserId, effectiveDate, developmentTerm, cpiCount, totalAmount, reserveAmount, dueAmount, discountRate, discountAmount,
                                             balance, completionDate, docuSignReference, boardUserId, investmentMotiveId, boardComment, investmentStatusId, investmentBankPayments, investmentInstallmentInfos)
    {
        Id = id;
        ProductFractionatedId = productFractionatedId;
        Amount = amount;
        InstallmentCount = installmentCount;
        Status = status;
    }
}