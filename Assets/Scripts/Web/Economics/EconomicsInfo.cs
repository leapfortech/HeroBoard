using System;
using UnityEngine;

using Leap.Graphics.Tools;

using Sirenix.OdinInspector;

public class EconomicsInfo
{
    [ShowInInspector]
    public Economics Economics { get; set; }
    
    [ShowInInspector]
    public Income[] Incomes { get; set; }
    public String[] DocIncomes
    {
        get => null;
        set
        {
            DocIncomeSprites = new Sprite[value.Length];
            for (int i = 0; i < value.Length; i++)
                DocIncomeSprites[i] = value[i]?.CreateSprite($"DocIncome{Economics.Id:D03}|{i + 1:D02}");
        }
    }
    public Sprite[] DocIncomeSprites { get; set; }

    public EconomicsInfo()
    {
    }

    public EconomicsInfo(Economics economics, Income[] incomes, String[] docIncomes)
    {
        Economics = economics;
        Incomes = incomes;
        DocIncomes = docIncomes;
    }
}
