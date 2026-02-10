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
    public class AppUsersEvent : UnityEvent<AppUserNamed[]> { }

    [Serializable]
    public class AppUserFullsEvent : UnityEvent<List<AppUserFull>> { }

    [Serializable]
    public class AppUserResponseEvent : UnityEvent<AppUserResponse> { }

    [SerializeField]
    private AppUserEvent onAppUserRetreived = null;

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

    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;

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
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            appUserFullsGetOp.Send();
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
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
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
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
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
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
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
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
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
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
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
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
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
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            statusPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
