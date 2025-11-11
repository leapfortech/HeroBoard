using System;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit;
using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class AccessService : MonoBehaviour
{
    [Serializable]
    public class UnityLoginEvent : UnityEvent<LoginBoardResponse> { }

    [Space]
    [SerializeField]
    private UnityLoginEvent onLogged = null;

    [SerializeField]
    private UnityIntEvent onRegistered = null;

    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;

    public void LoginBoard(String email, String version)
    {
        AccessLoginBoardPostOperation loginPostOp = new AccessLoginBoardPostOperation();
        try
        {
            loginPostOp.loginRequest = new LoginBoardRequest(email, version);
            loginPostOp["on-complete"] = (Action<AccessLoginBoardPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onLogged.Invoke(op.loginResponse);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            loginPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // Register
    public void RegisterBoard(RegisterBoardRequest registerRequest)
    {
        AccessRegisterBoardPostOperation registerPostOp = new AccessRegisterBoardPostOperation();
        try
        {
            registerPostOp.registerRequest = registerRequest;
            registerPostOp["on-complete"] = (Action<AccessRegisterBoardPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(int.Parse(op.customerId));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            registerPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
