using System;
using UnityEngine;

using Leap.Graphics.Tools;

public class InvestmentDocInfo
{
    public Investment Investment { get; set; }
    public String[] DocRtus
    {
        get => null;
        set
        {
            DocRtuSprites = new Sprite[value.Length];
            for (int i = 0; i < value.Length; i++)
                DocRtuSprites[i] = value[i]?.CreateSprite($"DocRtu{Investment.Id:D03}|{i + 1:D02}");
        }
    }
    public Sprite[] DocRtuSprites { get; set; }
    public EconomicsInfo EconomicsInfo { get; set; }
    public String[] DocBanks
    {
        get => null;
        set
        {
            DocBankSprites = new Sprite[value.Length];
            for (int i = 0; i < value.Length; i++)
                DocBankSprites[i] = value[i]?.CreateSprite($"DocBank{Investment.Id:D03}|{i + 1:D02}");
        }
    }
    public Sprite[] DocBankSprites { get; set; }

    public InvestmentDocInfo()
    {
    }

    public InvestmentDocInfo(Investment investment, String[] docRtus, EconomicsInfo economicsInfo, String[] docBanks)
    {
        Investment = investment;
        DocRtus = docRtus;
        EconomicsInfo = economicsInfo;
        DocBanks = docBanks;
    }
}