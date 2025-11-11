using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class ProjectService : MonoBehaviour
{
    [Serializable]
    public class FullsEvent : UnityEvent<List<ProjectProductFull>> { }
    [Serializable]
    public class ProjectInfoEvent : UnityEvent<ProjectInfo> { }
    [Serializable]
    public class ProjectImagesEvent : UnityEvent<ProjectImages[]> { }
    [Serializable]
    public class ProjectLikeIdsEvent : UnityEvent<List<int>> { }

    [SerializeField]
    private FullsEvent onFullsRetreived = null;

    [SerializeField]
    private ProjectInfoEvent onProjectInfoRetreived = null;

    [SerializeField]
    private ProjectLikeIdsEvent onProjectLikeIdsRetreived = null;

    [SerializeField]
    private ProjectImagesEvent onProjectImagesRetreived = null;

    [SerializeField]
    private UnityStringsEvent onImagesRetreived = null;

    [SerializeField]
    private UnityIntEvent onRegistered = null;

    [SerializeField]
    private UnityIntEvent onProjectLikeRegistered = null;

    [SerializeField]
    private UnityEvent onUpdated = null;


    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;


    // GET
    public void GetFulls()
    {
        ProjectFullsGetOperation fullsGetOp = new ProjectFullsGetOperation();
        try
        {
            fullsGetOp["on-complete"] = (Action<ProjectFullsGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFullsRetreived.Invoke(op.projectProductFulls);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            fullsGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetFullsByAppUser(int appUserId)
    {
        ProjectFullsByAppUserGetOperation fullsGetOp = new ProjectFullsByAppUserGetOperation();
        try
        {
            fullsGetOp.appUserId = appUserId;
            fullsGetOp["on-complete"] = (Action<ProjectFullsByAppUserGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFullsRetreived.Invoke(op.projectProductFulls);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            fullsGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetInfo(int id, bool images = true)
    {
        ProjectInfoGetOperation requestGetOp = new ProjectInfoGetOperation();
        try
        {
            requestGetOp.id = id;
            requestGetOp.images = images;
            requestGetOp["on-complete"] = (Action<ProjectInfoGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onProjectInfoRetreived.Invoke(op.projectInfo);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            requestGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetLikeIdsByAppUserId(int appUserId)
    {
        ProjectLikeIdsByAppUserIdGetOperation likeIdsGetOp = new ProjectLikeIdsByAppUserIdGetOperation();
        try
        {
            likeIdsGetOp.appUserId = appUserId;
            likeIdsGetOp["on-complete"] = (Action<ProjectLikeIdsByAppUserIdGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onProjectLikeIdsRetreived.Invoke(op.projectLikeIds);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            likeIdsGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetImages(bool first)
    {
        ProjectImagesGetOperation imagesGetOp = new ProjectImagesGetOperation();
        try
        {
            imagesGetOp.first = first;
            imagesGetOp["on-complete"] = (Action<ProjectImagesGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onProjectImagesRetreived.Invoke(op.projectImages);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            imagesGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetImagesById(int id)
    {
        ProjectImagesByIdGetOperation imagesGetOp = new ProjectImagesByIdGetOperation();
        try
        {
            imagesGetOp.id = id;
            imagesGetOp["on-complete"] = (Action<ProjectImagesByIdGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onImagesRetreived.Invoke(op.images);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            imagesGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // REGISTER
    public void Register(ProjectInfo projectInfo)
    {
        ProjectRegisterOperation registerOp = new ProjectRegisterOperation();
        try
        {
            registerOp.projectInfo = projectInfo;
            registerOp["on-complete"] = (Action<ProjectRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt32(op.id));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            registerOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }


    public void RegisterProjectLike(ProjectLike projectLike)
    {
        ProjectLikeRegisterOperation likeRegisterOp = new ProjectLikeRegisterOperation();
        try
        {
            likeRegisterOp.projectLike = projectLike;
            likeRegisterOp["on-complete"] = (Action<ProjectLikeRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onProjectLikeRegistered.Invoke(Convert.ToInt32(op.projectLikeId));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            likeRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // UPDATE
    public void UpdateProject(ProjectInfo projectInfo)
    {
        ProjectUpdateOperation updateOp = new ProjectUpdateOperation();
        try
        {
            updateOp.projectInfo = projectInfo;
            updateOp["on-complete"] = (Action<ProjectUpdateOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdated.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            updateOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
