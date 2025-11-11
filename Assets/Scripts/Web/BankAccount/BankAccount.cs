using System;

using Sirenix.OdinInspector;

public class BankAccount
{
    public int Id { get; set; } = -1;
    public int AppUserId { get; set; }
    [ShowInInspector]
    public int BankId { get; set; }
    [ShowInInspector]
    public int BankAccountTypeId { get; set; }
    [ShowInInspector]
    public int CurrencyId { get; set; }
    [ShowInInspector]
    public String Number { get; set; }
    [ShowInInspector]
    public String Name { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime UpdateDateTime { get; set; }
    public int Status { get; set; }


    public BankAccount()
    {
    }

    public BankAccount(int id, int appUserId, int bankId, int bankAccountTypeId, int currencyId, String number, String name,
                       DateTime createDateTime, DateTime updateDateTime, int status)
    {
        Id = id;
        AppUserId = appUserId;
        BankId = bankId;
        BankAccountTypeId = bankAccountTypeId;
        CurrencyId = currencyId;
        Number = number;
        Name = name;
        CreateDateTime = createDateTime;
        UpdateDateTime = updateDateTime;
        Status = status;
    }
}
