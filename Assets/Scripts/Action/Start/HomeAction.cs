using System;
using System.Globalization;
using UnityEngine;

using Leap.UI.Elements;

using Sirenix.OdinInspector;

public class HomeAction : MonoBehaviour
{
    //[Title("Hello")]
    //[SerializeField]
    //Text txtAppUserName = null;

    //[Title("Loan")]
    //[SerializeField]
    //Text txtLoan = null;
    //[SerializeField]
    //Text txtFootnote = null;
    //[SerializeField]
    //Image imgLoan = null;

    //[SerializeField, Space]
    //Style[] loanStyles = null;
    //[SerializeField, Space]
    //String[] loanFootnotes = null;
    //[SerializeField, Space]
    //Sprite[] loanSprites = null;

    //[Title("Actions")]
    //[SerializeField]
    //Button btnTakeLoan = null;
    //[SerializeField]
    //Button btnDetails = null;

    public void Refresh()
    {
        RefreshAppUser();
        RefreshLoan();
    }

    public void RefreshAppUser()
    {
        //txtAppUserName.TextValue = StateManager.Instance.AppUser.FirstName1;
    }

    public void RefreshLoan()
    {
        //Loan loan = StateManager.Instance.LoanInfo == null ? null : StateManager.Instance.LoanInfo.Loan;

        //int days = loan == null ? 0 : (loan.DueDate - DateTime.Today).Days;
        //int state = loan == null ? 0 : days > 0 ? 1 : days == 0 ? 2 : 3;

        //txtLoan.SetStyle(loanStyles[state]);
        ////if (state == 0)
        //    //txtLoan.TextValue = "$" + StateManager.Instance.AppUser.LoanMaxAmount.ToString("N2", CultureInfo.InvariantCulture);
        //if (state == 1)
        //    txtLoan.TextValue = days + (days < 2 ? " day" : " days");
        //else if (state == 2)
        //    txtLoan.TextValue = "Today";
        //else if (state == 3)
        //    txtLoan.TextValue = -days + (-days < 2 ? " day" : " days");

        //txtFootnote.TextValue = loanFootnotes[state];
        //imgLoan.Sprite = loanSprites[state];

        //btnTakeLoan.gameObject.SetActive(state == 0);
        //btnDetails.gameObject.SetActive(state != 0);
    }
}