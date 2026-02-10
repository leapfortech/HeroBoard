using System;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class BoardUserService : MonoBehaviour
{
    [Serializable]
    public class BoardUserEvent : UnityEvent<BoardUser> { }

    [Serializable]
    public class BoardUserFullsEvent : UnityEvent<BoardUserFull[]> { }

    [SerializeField]
    private BoardUserEvent onRetreived = null;

    [SerializeField]
    private BoardUserFullsEvent onFullsRetreived = null;

    [SerializeField]
    private UnityIntEvent onCount = null;

    [SerializeField]
    private UnityEvent onUpdated = null;

    [SerializeField]
    private UnityEvent onStatusUpdated = null;

    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;

    // GET
    public void GetFulls()
    {
        BoardUserFullsGetOperation boardUsersFullGetOp = new BoardUserFullsGetOperation();
        try
        {
            boardUsersFullGetOp["on-complete"] = (Action<BoardUserFullsGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFullsRetreived.Invoke(op.boardUserFulls);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            boardUsersFullGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetFullsByStatus(int status)
    {
        BoardUserFullsByStatusGetOperation boardUsersFullGetOp = new BoardUserFullsByStatusGetOperation();
        try
        {
            boardUsersFullGetOp.status = status;
            boardUsersFullGetOp["on-complete"] = (Action<BoardUserFullsByStatusGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFullsRetreived.Invoke(op.boardUserFulls);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            boardUsersFullGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetById(long boardUserId)
    {
        BoardUserByIdGetOperation boardUserGetOp = new BoardUserByIdGetOperation();
        try
        {
            boardUserGetOp.id = boardUserId;
            boardUserGetOp["on-complete"] = (Action<BoardUserByIdGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRetreived.Invoke(op.boardUser);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            boardUserGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetCount()
    {
        BoardUsersCountGetOperation boardUsersCountGetOp = new BoardUsersCountGetOperation();
        try
        {
            boardUsersCountGetOp["on-complete"] = (Action<BoardUsersCountGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onCount.Invoke(Convert.ToInt32(op.count));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            boardUsersCountGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetCountByStatus(int status)
    {
        BoardUsersCountGetOperation boardUsersCountGetOp = new BoardUsersCountGetOperation();
        try
        {
            boardUsersCountGetOp["on-complete"] = (Action<BoardUsersCountGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onCount.Invoke(Convert.ToInt32(op.count));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            boardUsersCountGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // UPDATE
    public void UpdateFull(BoardUserFull boardUserFull)
    {
        BoardUserFullPutOperation boardUserFullPutOp = new BoardUserFullPutOperation();
        try
        {
            boardUserFullPutOp.boardUserFull = boardUserFull;
            boardUserFullPutOp["on-complete"] = (Action<BoardUserFullPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdated.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            boardUserFullPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void UpdateBoardUser(BoardUser boardUser)
    {
        BoardUserPutOperation boardUserPutOp = new BoardUserPutOperation();
        try
        {
            boardUserPutOp.boardUser = boardUser;
            boardUserPutOp["on-complete"] = (Action<BoardUserPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdated.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            boardUserPutOp.Send();
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
