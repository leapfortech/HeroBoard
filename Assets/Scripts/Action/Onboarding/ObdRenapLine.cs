using System;
using UnityEngine;

using Leap.UI.Elements;

public class ObdRenapLine : MonoBehaviour
{
    [SerializeField]
    Text txtDpi;

    [SerializeField]
    Text txtRenap;

    [SerializeField]
    ToggleGroup tggVerification;

    public String Dpi
    {
        get => txtDpi.TextValue;
        set => txtDpi.TextValue = value;
    }

    public String Renap
    {
        get => txtRenap.TextValue;
        set => txtRenap.TextValue = value;
    }

    public int Result => Convert.ToInt32(tggVerification.Value);

    public void Verify()
    {
        tggVerification.Value = txtRenap.TextValue.Length > 0 ? txtDpi.TextValue.ToUpperInvariant() == txtRenap.TextValue.ToUpperInvariant() ? "1" : "0" : txtDpi.TextValue.Length == 0 ? "1" : "0";
    }

    public String Value
    {
        get => tggVerification.Value;
        set => tggVerification.Value = value;
    }

    public void Clear()
    {
        txtDpi.TextValue = "";
        txtRenap.TextValue = "";
        tggVerification.Value = "0";
    }
}