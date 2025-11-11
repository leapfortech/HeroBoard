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

    [Serializable]
    public class IdentityInfoEvent : UnityEvent<IdentityInfo> { }

    [Serializable]
    public class IdentityBoardInfoEvent : UnityEvent<IdentityBoardInfo> { }

    [SerializeField]
    private IdentityEvent onIdentityRetreived = null;

    [SerializeField]
    private IdentitysEvent onIdentitysRetreived = null;

    [SerializeField]
    private IdentityFullsEvent onIdentityFullsRetreived = null;

    [SerializeField]
    private IdentityInfoEvent onIdentityInfoRetreived = null;

    [SerializeField]
    private IdentityBoardInfoEvent onIdentityBoardInfoRetreived = null;

    [SerializeField]
    private UnityStringEvent onPortraitRetreived = null;

    [SerializeField]
    private UnityIntEvent onRegistered = null;

    [SerializeField]
    private UnityIntEvent onIdentityUpdated = null;

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

    public void GetById(int id)
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

    public void GetByAppUserId(int appUserId, int status = 1)
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

    public void GetInfoByAppUserId(int appUserId, int status = 1)
    {
        IdentityInfoAppUserGetOperation identityInfoAppUserGetOp = new IdentityInfoAppUserGetOperation();
        try
        {
            identityInfoAppUserGetOp.appUserId = appUserId;
            identityInfoAppUserGetOp.status = status;
            identityInfoAppUserGetOp["on-complete"] = (Action<IdentityInfoAppUserGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdentityInfoRetreived.Invoke(op.identityInfo);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            identityInfoAppUserGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetBoardInfoByAppUserId(int appUserId, int status = 1)
    {
        IdentityBoardInfoAppUserGetOperation identityBoardInfoAppUserGetOp = new IdentityBoardInfoAppUserGetOperation();
        try
        {
            identityBoardInfoAppUserGetOp.appUserId = appUserId;
            identityBoardInfoAppUserGetOp.status = status;
            identityBoardInfoAppUserGetOp["on-complete"] = (Action<IdentityBoardInfoAppUserGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdentityBoardInfoRetreived.Invoke(op.identityBoardInfo);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            identityBoardInfoAppUserGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetPortraitByAppUserId(int appUserId)
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
    public void Register(IdentityRegister identityRegister)
    {
        IdentityRegisterPostOperation identityRegisterPostOp = new IdentityRegisterPostOperation();
        try
        {
            identityRegisterPostOp.identityRegister = identityRegister;
            identityRegisterPostOp["on-complete"] = (Action<IdentityRegisterPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt32(op.id));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            identityRegisterPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // UPDATE
    public void UpdateIdentity(Identity identity)
    {
        IdentityPutOperation identityPutOperation = new IdentityPutOperation();
        try
        {
            identityPutOperation.identity = identity;
            identityPutOperation["on-complete"] = (Action<IdentityPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdentityUpdated.Invoke(Convert.ToInt32(op.id));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            identityPutOperation.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void UpdatePortrait(int appUserId, String portrait)
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

    public void UpdateInfo(IdentityInfo identityInfo)
    {
        IdentityInfoPutOperation identityFullPutOp = new IdentityInfoPutOperation();
        try
        {
            identityFullPutOp.identityInfo = identityInfo;
            identityFullPutOp["on-complete"] = (Action<IdentityInfoPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onIdentityUpdated.Invoke(Convert.ToInt32(op.id));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            identityFullPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
