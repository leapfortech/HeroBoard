using System;
using UnityEngine;

using Leap.Graphics.Tools;
using Leap.UI.Elements;
using Leap.UI.Dialog;

using Sirenix.OdinInspector;

public class RenapAction : MonoBehaviour
{
    [Title("CUI")]
    [SerializeField]
    InputField ifdCui = null;

    [Title("Identity")]
    [SerializeField]
    Text txtFirstNames = null;
    [SerializeField]
    Text txtLastNames = null;
    [Space]
    [SerializeField]
    Text txtGender = null;
    [SerializeField]
    Text txtMaritalStatus = null;
    [SerializeField]
    Text txtBirthDate = null;
    [SerializeField]
    Text txtBirthPlace = null;
    [SerializeField]
    Text txtNationality = null;
    [SerializeField]
    Text txtOccupation = null;
    [Space]
    [SerializeField]
    Image imgRenapPortrait = null;

    [Title("DPI")]
    [SerializeField]
    Text txtDueDate = null;
    [SerializeField]
    Text txtVersion = null;

    private Text[] texts;
    private RenapService renapService = null;

    private void Awake()
    {
        renapService = GetComponent<RenapService>();

        texts = new Text[10];
        texts[0] = txtFirstNames;
        texts[1] = txtLastNames;

        texts[2] = txtGender;
        texts[3] = txtMaritalStatus;
        texts[4] = txtBirthDate;
        texts[5] = txtBirthPlace;
        texts[6] = txtNationality;
        texts[7] = txtOccupation;

        texts[8] = txtDueDate;
        texts[9] = txtVersion;
    }

    public void Clear()
    {
        for (int i = 0; i < texts.Length; i++)
            texts[i].TextValue = "-";
        imgRenapPortrait.Sprite?.Destroy();
    }

    public void GetIdentityInfo()
    {
        if (String.IsNullOrEmpty(ifdCui.Text))
        {
            ChoiceDialog.Instance.Error("Renap", "El CUI es obligatorio");
            return;
        }

        if (!ElementHelper.Validate(ifdCui))
            return;

        ScreenDialog.Instance.Display();

        Clear();
        renapService.GetIdentityInfoByCui(ifdCui.Text);
    }

    public void Display(RenapIdentityInfo renapIdentityInfo)
    {
        RenapIdentity renapIdentity = renapIdentityInfo.RenapIdentity;

        txtFirstNames.TextValue = renapIdentity.GetFirstNames();
        txtLastNames.TextValue = renapIdentity.GetLastNames();

        txtGender.TextValue = renapIdentity.Gender == "M" ? "MASCULINO" : "FEMENINO";
        txtMaritalStatus.TextValue = (renapIdentity.MaritalStatus == "S" ? "SOLTER" : "CASAD") + (renapIdentity.Gender == "M" ? "O" : "A");
        txtBirthDate.TextValue = $"{renapIdentity.BirthDate:dd/MM/yyyy}";
        txtBirthPlace.TextValue = renapIdentity.GetBirthPlace();
        txtNationality.TextValue = renapIdentity.Nationality;
        txtOccupation.TextValue = renapIdentity.Occupation;

        txtDueDate.TextValue = renapIdentity.DpiDueDate.HasValue ? $"{renapIdentity.DpiDueDate:dd/MM/yyyy}" : "-";
        txtVersion.TextValue = String.IsNullOrEmpty(renapIdentity.DpiVersion) ? "-" : renapIdentity.DpiVersion;

        imgRenapPortrait.Sprite = renapIdentityInfo.Face?.CreateSprite("RenapIdentityPortrait");

        ScreenDialog.Instance.Hide();
    }
}
