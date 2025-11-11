using Sirenix.OdinInspector;
using System.Collections.Generic;

public class IncomeInfo
{
    [ShowInInspector]
    public Economics Economics { get; set; }
    [ShowInInspector]
    public Income[] Incomes { get; set; }

    public IncomeInfo()
    {
    }

    public IncomeInfo(Economics economics, Income[] incomes)
    {
        Economics = economics;
        Incomes = incomes;
    }
}
