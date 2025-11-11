using System;

public class InvestmentFractionated
{
    public int Id { get; set; }
    public int InvestmentId { get; set; }
    public int ProductFractionatedId { get; set; }
    public double Amount { get; set; }
    public int Total { get; set; }
    public int Count { get; set; }
    public DateTime StartDate { get; set; }
    public int Status { get; set; }


    public InvestmentFractionated()
    {
    
}

    public InvestmentFractionated(int id, int investmentId, int productFractionatedId, double amount, int total,
                                 int count, DateTime startDate, int status)
    {
        Id = id;
        InvestmentId = investmentId;
        ProductFractionatedId = productFractionatedId;
        Amount = amount;
        Total = total;
        Count = count;
        StartDate = startDate;
        Status = status;
    }
}
