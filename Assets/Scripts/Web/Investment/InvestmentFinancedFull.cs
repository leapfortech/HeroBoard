using System;
using System.Collections.Generic;

public class InvestmentFinancedFull : InvestmentFull
{
    public int Id { get; set; }
    public int ProductFinancedId { get; set; }
    public double AdvAmount { get; set; }
    public int AdvInstallmentTotal { get; set; }
    public double LoanInterestRate { get; set; }
    public int LoanTerm { get; set; }
    public int Status { get; set; }

    public InvestmentFinancedFull()
    {
    }

    public InvestmentFinancedFull(int id, int investmentId, int productFinancedId,
                                  int projectId, int productTypeId, int appUserId, DateTime effectiveDate, int developmentTerm, int cpiCount, double totalAmount, double reserveAmount, double dueAmount,
                                  double discountRate, double discountAmount, double balance, DateTime? completionDate, String docuSignReference, int boardUserId, int investmentMotiveId, String boardComment, int investmentStatusId,
                                  double advAmount, int advInstallmentTotal, double loanInterestRate, int loanTerm, int status, List<InvestmentBankPayment> investmentBankPayments, List<InvestmentInstallmentInfo> investmentInstallmentInfos)
                                  : base(investmentId, projectId, productTypeId, appUserId, effectiveDate, developmentTerm, cpiCount, totalAmount, reserveAmount, dueAmount, discountRate, discountAmount,
                                         balance, completionDate, docuSignReference, boardUserId, investmentMotiveId, boardComment, investmentStatusId, investmentBankPayments, investmentInstallmentInfos)
    {
        Id = id;
        ProductFinancedId = productFinancedId;
        AdvAmount = advAmount;
        AdvInstallmentTotal = advInstallmentTotal;
        LoanInterestRate = loanInterestRate;
        LoanTerm = loanTerm;
        Status = status;
    }
}