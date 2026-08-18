using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class AppUserService : MonoBehaviour
{
    [Serializable]
    public class AppUserEvent : UnityEvent<AppUser> { }
    [Serializable]
    public class UserInfoAllEvent : UnityEvent<UserInfoAllRsp> { }

    [Serializable]
    public class AppUsersEvent : UnityEvent<AppUserNamed[]> { }

    [Serializable]
    public class AppUserFullsEvent : UnityEvent<List<AppUserFull>> { }


    [SerializeField]
    private AppUserEvent onAppUserRetreived = null;
    
    [SerializeField]
    private UserInfoAllEvent onUserInfoAllRetreived = null;

    [SerializeField]
    private AppUsersEvent onAppUsersRetreived = null;

    [SerializeField]
    private AppUserFullsEvent onAppUserFullsRetreived = null;

    [SerializeField]
    private UnityIntEvent onAppUsersCount = null;

    [SerializeField]
    private UnityEvent onUpdated = null;

    [SerializeField]
    private UnityEvent onPhoneUpdated = null;

    [SerializeField]
    private UnityEvent onStatusUpdated = null;

    [Title("Errors")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;

    [SerializeField]
    private UnityStringEvent onTimeoutError = null;

    // GET
    public void GetFullsByStatus(int status)
    {
        AppUserFullsGetOperation appUserFullsGetOp = new AppUserFullsGetOperation();
        try
        {
            appUserFullsGetOp.status = status;
            appUserFullsGetOp["on-complete"] = (Action<AppUserFullsGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onAppUserFullsRetreived.Invoke(op.appUserFulls);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            appUserFullsGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetUserInfoAllByAlias(UserInfoAllByAlias req)
    {
        UserInfoAllByAliasPostOperation userInfoAllPostOp = new UserInfoAllByAliasPostOperation();
        try
        {
            userInfoAllPostOp.req = req;
            userInfoAllPostOp["on-complete"] = (Action<UserInfoAllByAliasPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUserInfoAllRetreived.Invoke(op.rsp);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            userInfoAllPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetAppUsersCount()
    {
        AppUsersCountGetOperation appUsersCountGetOp = new AppUsersCountGetOperation();
        try
        {
            appUsersCountGetOp["on-complete"] = (Action<AppUsersCountGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onAppUsersCount.Invoke(Convert.ToInt32(op.count));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            appUsersCountGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetAppUsersByStatus(int status, int count = 0, int page = 0)
    {
        AppUsersByStatusGetOperation appUsersGetOp = new AppUsersByStatusGetOperation();
        try
        {
            appUsersGetOp.status = status;
            appUsersGetOp.count = count;
            appUsersGetOp.page = page;
            appUsersGetOp["on-complete"] = (Action<AppUsersByStatusGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onAppUsersRetreived.Invoke(op.appUsersNamed);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            appUsersGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetAppUsersCountByStatus(int status)
    {
        AppUsersCountGetOperation appUsersCountGetOp = new AppUsersCountGetOperation();
        try
        {
            appUsersCountGetOp["on-complete"] = (Action<AppUsersCountGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onAppUsersCount.Invoke(Convert.ToInt32(op.count));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            appUsersCountGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetAppUser(long appUserId)
    {
        AppUserByIdGetOperation appUserGetOp = new AppUserByIdGetOperation();
        try
        {
            appUserGetOp.id = appUserId;
            appUserGetOp["on-complete"] = (Action<AppUserByIdGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onAppUserRetreived.Invoke(op.appUser);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            appUserGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // UPDATE
    public void UpdatePerson(AppUser appUser)
    {
        AppUserPutOperation appUserPutOp = new AppUserPutOperation();
        try
        {
            appUserPutOp.appUser = appUser;
            appUserPutOp["on-complete"] = (Action<AppUserPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdated.Invoke();
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            appUserPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void UpdatePhone(PhoneRequest phoneRequest)
    {
        AppUserPhonePutOperation phonePutOp = new AppUserPhonePutOperation();
        try
        {
            phonePutOp.phoneRequest = phoneRequest;
            phonePutOp["on-complete"] = (Action<AppUserPhonePutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onPhoneUpdated.Invoke();
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            phonePutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void UpdateStatus(long appUserId, int status)
    {
        AppUserStatusPutOperation statusPutOp = new AppUserStatusPutOperation();
        try
        {
            statusPutOp.appUserId = appUserId;
            statusPutOp.appUserStatusId = status;
            statusPutOp["on-complete"] = (Action<AppUserStatusPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onStatusUpdated.Invoke();
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            statusPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
