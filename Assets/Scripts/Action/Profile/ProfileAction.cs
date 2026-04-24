using UnityEngine;

using Leap.UI.Elements;
using Leap.Data.Web;
using Leap.Data.Collections;

using Sirenix.OdinInspector;

public class ProfileAction : MonoBehaviour
{
    [Title("Fields")]
    [SerializeField]
    Text txtUserName = null;
    [SerializeField]
    Text txtFirstNames = null;
    [SerializeField]
    Text txtLastNames = null;
    [SerializeField]
    Text txtAutUserId = null;
    
    [SerializeField]
    Text txtEmail = null;
    [SerializeField]
    Text txtPhone = null;


    [Title("Data")]
    [SerializeField]
    ValueList vllCountry = null;

    private void Awake()
    {
    }

    public void Clear()
    {
        txtUserName.TextValue = "-";
    }

    public void Display()
    {
        txtUserName.TextValue = StateManager.Instance.Identity.FirstName1 + " " + StateManager.Instance.Identity.LastName1;

        txtFirstNames.TextValue = StateManager.Instance.Identity.FirstNames;
        txtLastNames.TextValue = StateManager.Instance.Identity.LastNames;
        txtAutUserId.TextValue = WebManager.Instance.WebSysUser.AuthUserId;
        txtEmail.TextValue = WebManager.Instance.WebSysUser.Email;
        txtPhone.TextValue = $"{vllCountry.FindRecordCellString(WebManager.Instance.WebSysUser.PhoneCountryId, 2)} {WebManager.Instance.WebSysUser.Phone}";
    }
}
