using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Leap.Core.Tools;
using Leap.Core.Security;
using Leap.Graphics.Tools;
using Leap.UI.Elements;
using Leap.UI.Page;
using Leap.UI.Dialog;
using Leap.Data.Collections;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class LoginAction : MonoBehaviour
{
    [Title("Version")]
    [SerializeField]
    Text txtStartVersion = null;

    [SerializeField]
    Text txtLoginVersion = null;

    [Title("Fields")]
    [SerializeField]
    InputField ifdEmail = null;

    [SerializeField]
    InputField ifdPassword = null;

    [SerializeField]
    Toggle chkRemember = null;

    [Title("Authenticate")]
    [SerializeField]
    Button btnRegister;

    [Title("Action")]
    [SerializeField]
    Button btnLogin = null;

    [Title("Pages")]
    [SerializeField]
    Page loginPage = null;

    [SerializeField]
    Page homePage = null;

    //[SerializeField]
    //Page registerRejectedPage = null;

    [Title("Messages")]
    [SerializeField, TextArea(2, 4)]
    String accessDeniedError = "Acceso Denegado.";

    [Space]
    [SerializeField, TextArea(2, 6)]
    String resendMailLinkMsg = "Se mandó de nuevo el enlace de confimación a tu correo:\r\n{0}\r\n" +
                               "Si no lo encuentras, busca en tu carpeta de Spams o envíalo de nuevo.\r\nConfirma tu correo y prueba de nuevo.";

    readonly String[] envVersion = { " Dev", " QA", "" };

    AccessService accessService = null;
    WebSysUserService webSysUserService = null;
    WebSysTokenService webSysTokenService = null;
    NotificationService notificationService = null;

    ElementValue[] elementValues = null;
    LoginBoardResponse loginResponse = null;

    private void Awake()
    {
        accessService = GetComponent<AccessService>();
        webSysUserService = GetComponent<WebSysUserService>();
        webSysTokenService = GetComponent<WebSysTokenService>();
        notificationService = GetComponent<NotificationService>();

        elementValues = new ElementValue[2];
        elementValues[0] = ifdEmail;
        elementValues[1] = ifdPassword;
    }

    public void DisplayVersion()
    {
        txtStartVersion.TextValue = txtLoginVersion.TextValue = "v " + Application.version + envVersion[WebManager.Instance.EnvironmentId];
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Email"))
            PlayerPrefs.SetString("Email", "");

        btnLogin?.AddAction(DoLogin);
    }

    public void Clear()
    {
        for (int i = 0; i < elementValues.Length; i++)
            elementValues[i].Clear();
    }

    public void Display()
    {
        ifdEmail.Text = PlayerPrefs.GetString("Email");
        ifdEmail.Revalidate(ifdEmail.Text.Length > 0);
        chkRemember.Checked = ifdEmail.Text.Length > 0;
    }

    // Login
    private void DoLogin()  // Manual Login
    {
        if (!ElementHelper.Validate(elementValues))
            return;

        Login(ifdEmail.Text, ifdPassword.Text);
    }

    public void Login(String[] credentials)  // Biometric Login
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        Login(credentials[0], credentials[1]);
#elif UNITY_IOS
        LoginApple();
#endif
    }

    private void Login(String eMail, String password)
    {
        ScreenDialog.Instance.Display();
        PageManager.Instance.ResetTimer();

        FirebaseManager.Instance.Login(eMail, password, OnLoginDone, null);
    }

#if !UNITY_EDITOR && UNITY_IOS
    private void LoginApple()
    {
        ScreenDialog.Instance.Display();
        PageManager.Instance.ResetTimer();

        FirebaseManager.Instance.Login(NativeAuthManager.Instance.AppleCredential, NativeAuthManager.Instance.AppleRawNonce, OnLoginDone, null);
    }
#endif

    private void OnLoginDone(String eMail)
    {
        PlayerPrefs.SetString("Email", chkRemember.Checked ? eMail : "");

        accessService.LoginBoard(eMail, Application.version);
    }

    public void ResendMailLink()
    {
        if (!ElementHelper.Validate(elementValues))
            return;

        ScreenDialog.Instance.Display();

        FirebaseManager.Instance.Login(ifdEmail.Text, ifdPassword.Text, OnLoginMailLinkDone, null, false);
    }

    private void OnLoginMailLinkDone(String eMail)
    {
        webSysUserService.SendMailLink(eMail);
    }

    public void ApplyMailLink()
    {
        ChoiceDialog.Instance.Info("Login", String.Format(resendMailLinkMsg, ifdEmail.Text));
    }

    // SetCustomer
    public void SetAppUser(LoginBoardResponse response)
    {
        loginResponse = response;
        StateManager.Instance.BoardUser = response.BoardUser;
        WebManager.Instance.WebSysUser = response.WebSysUser;
    }

    // SystemToken
    public void AddSystemToken()
    {
        webSysTokenService.Add(new WebSysToken(-1, WebManager.Instance.WebSysUser.Id, NotificationManager.Instance.Token ?? "STANDALONE_TOKEN", 1));
    }

    // Change Page
    public void ChangePage()
    {
        if (loginResponse.Message != null)
        {
            String[] message = loginResponse.Message.Split('|');
            if (message[0] == "1")
                ChoiceDialog.Instance.Info(message[1], message[2], loginResponse.Granted == 1 ? (UnityAction)ChangePageLink : GoToLink);
            else
                ChoiceDialog.Instance.Error(message[1], message[2], loginResponse.Granted == 1 ? (UnityAction)ChangePageLink : GoToLink);
        }
        else if (loginResponse.Granted == 1)
            ChangePageGranted();
        else
            ChoiceDialog.Instance.Error("Login", accessDeniedError, loginResponse.Link != null ? GoToLink : (UnityAction)null);
    }

    private void GoToLink()
    {
        if (loginResponse.Link == null)
            return;

        String[] links = loginResponse.Link.Split('|');

#if UNITY_IOS
        if (links.Length > 1)
            Application.OpenURL(links[1]);
        else
            Application.OpenURL(links[0]);
#else
        Application.OpenURL(links[0]);
#endif
    }

    private void ChangePageLink()
    {
        GoToLink();
        ChangePageGranted();
    }

    public void ChangePageVerified()
    {
        StateManager.Instance.BoardUser.BoardUserStatusId = 1;
        ChangePageGranted();
    }

    private void ChangePageGranted()
    {
        //ScreenDialog.Instance.Display();

        Clear();

        GetNotifications();
    }

    public void ChangeToHomePage()
    {
        Clear();

        PageManager.Instance.ChangePage(homePage);
    }

    // Notification
    public void GetNotifications()
    {
        notificationService.GetNotifications(WebManager.Instance.WebSysUser.Id);
        //Invoke(nameof(ChangeToLoginPage), 0.1f);
    }

    // Remote Login
    public void RemoteLogin(String[] message)
    {
        ChoiceDialog.Instance.Info(message[0], message[1], ChangeToLoginPage);
    }

    public void ChangeToLoginPage()
    {
        PageManager.Instance.ChangePage(loginPage);
    }
}
