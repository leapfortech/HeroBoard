using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class IdentityService : MonoBehaviour
{
    [Serializable]
    public class IdentityEvent : UnityEvent<Identity> { }

    [Serializable]
    public class IdentitysEvent : UnityEvent<List<Identity>> { }

    [Serializable]
    public class IdentityFullsEvent : UnityEvent<List<IdentityFull>> { }


    [SerializeField]
    private IdentityEvent onIdentityRetreived = null;

    [SerializeField]
    private IdentitysEvent onIdentitysRetreived = null;

    [SerializeField]
    private IdentityFullsEvent onIdentityFullsRetreived = null;

    [SerializeField]
    private UnityStringEvent onPortraitRetreived = null;

    [SerializeField]
    private UnityEvent onPortraitUpdated = null;

    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;

    // GET
    public void GetAll(int status)
    {
        IdentitysGetOperation identitysGetOp = new IdentitysGetOperation();
        try
        {
            identitysGetOp.status = status;
            identitysGetOp["on-complete"] = (Action<IdentitysGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdentitysRetreived.Invoke(op.identitys);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            identitysGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetFullAll(int status)
    {
        IdentityFullsGetOperation identityFullsGetOp = new IdentityFullsGetOperation();
        try
        {
            identityFullsGetOp.status = status;
            identityFullsGetOp["on-complete"] = (Action<IdentityFullsGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdentityFullsRetreived.Invoke(op.identityFulls);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            identityFullsGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetById(long id)
    {
        IdentityGetOperation identityGetOp = new IdentityGetOperation();
        try
        {
            identityGetOp.id = id;
            identityGetOp["on-complete"] = (Action<IdentityGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdentityRetreived.Invoke(op.identity);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            identityGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetByAppUserId(long appUserId, int status = 1)
    {
        IdentityAppUserGetOperation identityAppUserGetOp = new IdentityAppUserGetOperation();
        try
        {
            identityAppUserGetOp.appUserId = appUserId;
            identityAppUserGetOp.status = status;
            identityAppUserGetOp["on-complete"] = (Action<IdentityAppUserGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdentityRetreived.Invoke(op.identity);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            identityAppUserGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetPortraitByAppUserId(long appUserId)
    {
        PortraitAppUserGetOperation portraitAppUserGetOp = new PortraitAppUserGetOperation();
        try
        {
            portraitAppUserGetOp.appUserId = appUserId;
            portraitAppUserGetOp["on-complete"] = (Action<PortraitAppUserGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onPortraitRetreived.Invoke(op.portrait[1..(op.portrait.Length - 1)]);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            portraitAppUserGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // REGISTER

    // UPDATE
    public void UpdatePortrait(long appUserId, String portrait)
    {
        IdentityPortraitPutOperation identityPortraitPutOp = new IdentityPortraitPutOperation();
        try
        {
            identityPortraitPutOp.appUserId = appUserId;
            identityPortraitPutOp.portrait = "\"" + portrait + "\"";
            identityPortraitPutOp["on-complete"] = (Action<IdentityPortraitPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onPortraitUpdated.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            identityPortraitPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
