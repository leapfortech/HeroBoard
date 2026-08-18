using System;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using System.Collections.Generic;
using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class PuzzleService : MonoBehaviour
{
    [Serializable]
    public class AllByDifficultyEvent : UnityEvent<PuzzleAllRsp> { }

    [SerializeField]
    private AllByDifficultyEvent onAllRetreived = null;
    [Serializable]
    public class PuzzleFullEvent : UnityEvent<PuzzleFull> { }

    [Serializable]
    public class PuzzleFullsEvent : UnityEvent<List<PuzzleFull>> { }

    [SerializeField]
    private PuzzleFullEvent onFullRetreived = null;

    [SerializeField]
    private PuzzleFullsEvent onFullsRetreived = null;

    [SerializeField]
    private UnityLongEvent onRegistered = null;

    [SerializeField]
    private UnityBoolEvent onUpdated = null;

    [SerializeField]
    private UnityBoolEvent onStatusUpdated = null;


    [Title("Errors")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;

    [SerializeField]
    private UnityStringEvent onTimeoutError = null;


    // GET
    public void GetFull(long id, long likeAppUserId)
    {
        PuzzleGetFullOperation puzzleFullGetOp = new PuzzleGetFullOperation();
        try
        {
            puzzleFullGetOp.id = id;
            puzzleFullGetOp.likeAppUserId = likeAppUserId;
            puzzleFullGetOp["on-complete"] = (Action<PuzzleGetFullOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFullRetreived.Invoke(op.puzzleFull);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            puzzleFullGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetFullByPostId(long postId, long likeAppUserId)
    {
        PuzzleFullByPostIdGetFullOperation puzzleFullByPostIdGetOp = new PuzzleFullByPostIdGetFullOperation();
        try
        {
            puzzleFullByPostIdGetOp.postId = postId;
            puzzleFullByPostIdGetOp.likeAppUserId = likeAppUserId;
            puzzleFullByPostIdGetOp["on-complete"] = (Action<PuzzleFullByPostIdGetFullOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFullRetreived.Invoke(op.puzzleFull);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            puzzleFullByPostIdGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetFulls(int status)
    {
        PuzzleGetFullsOperation puzzleFullsGetOp = new PuzzleGetFullsOperation();
        try
        {
            puzzleFullsGetOp.status = status;
            puzzleFullsGetOp["on-complete"] = (Action<PuzzleGetFullsOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFullsRetreived.Invoke(op.puzzleFulls);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            puzzleFullsGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetAllByDifficulty(PuzzleAllByDifficultyReq req)
    {
        AllByDifficultyPostOperation AllByDifficultyPostOp = new AllByDifficultyPostOperation();
        try
        {
            AllByDifficultyPostOp.req = req;
            AllByDifficultyPostOp["on-complete"] = (Action<AllByDifficultyPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onAllRetreived.Invoke(op.rsp);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            AllByDifficultyPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // REGISTER
    public void Register(RegisterPuzzleRequest registerPuzzleRequest)
    {
        PuzzleRegisterOperation referredRegisterOp = new PuzzleRegisterOperation();
        try
        {
            referredRegisterOp.registerPuzzleRequest = registerPuzzleRequest;
            referredRegisterOp["on-complete"] = (Action<PuzzleRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt64(op.id));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            referredRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // UPDATE
    public void UpdatePuzzle(RegisterPuzzleRequest registerPuzzleRequest)
    {
        PuzzlePutOperation referredPutOp = new PuzzlePutOperation();
        try
        {
            referredPutOp.registerPuzzleRequest = registerPuzzleRequest;
            referredPutOp["on-complete"] = (Action<PuzzlePutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdated.Invoke(Convert.ToBoolean(op.response));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            referredPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void Accept(PostModerationRequest postModerationRequest)
    {
        PuzzleAcceptPutOperation acceptPutOp = new PuzzleAcceptPutOperation();
        try
        {
            acceptPutOp.postModerationRequest = postModerationRequest;
            acceptPutOp["on-complete"] = (Action<PuzzleAcceptPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdated.Invoke(op.response);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            acceptPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void Reject(PostModerationRequest postModerationRequest)
    {
        PuzzleRejectPutOperation rejectPutOp = new PuzzleRejectPutOperation();
        try
        {
            rejectPutOp.postModerationRequest = postModerationRequest;
            rejectPutOp["on-complete"] = (Action<PuzzleRejectPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdated.Invoke(op.response);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            rejectPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void UpdateStatus(long postId, long puzzleId, int status)
    {
        PuzzleUpdateStatusPutOperation updateStatusPutOp = new PuzzleUpdateStatusPutOperation();
        try
        {
            updateStatusPutOp.postId = postId;
            updateStatusPutOp.puzzleId = puzzleId;
            updateStatusPutOp.status = status;
            updateStatusPutOp["on-complete"] = (Action<PuzzleUpdateStatusPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onStatusUpdated.Invoke(op.response);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            updateStatusPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}