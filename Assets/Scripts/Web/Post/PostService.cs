using System;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class PostService : MonoBehaviour
{
    [Serializable]
    public class PostFeedResponseEvent : UnityEvent<PostFeedResponse> { }
    [Serializable]
    public class PostFullsPagedResponseEvent : UnityEvent<PostFullsPagedResponse> { }

    [SerializeField]
    private PostFeedResponseEvent onFeedRetreived = null;

    [SerializeField]
    private PostFullsPagedResponseEvent onFullsPagedRetreived = null;

    [SerializeField]
    private UnityStringsEvent onImagesRetreived = null;

    [SerializeField]
    private UnityLongEvent onRegistered = null;

    [Title("Errors")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;

    [SerializeField]
    private UnityStringEvent onTimeoutError = null;


    // GET
    public void GetImagesById(long id, String first = "true")
    {
        ImagesByIdGetOperation imagesGetOp = new ImagesByIdGetOperation();
        try
        {
            imagesGetOp.id = id;
            imagesGetOp.first = first;
            imagesGetOp["on-complete"] = (Action<ImagesByIdGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onImagesRetreived.Invoke(op.projectImages);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            imagesGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // POST
    public void GetPostFeed(PostFeedRequest postFeedRequest)
    {
        PostFeedOperation postFeedOp = new PostFeedOperation();
        try
        {
            postFeedOp.postFeedRequest = postFeedRequest;
            postFeedOp["on-complete"] = (Action<PostFeedOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFeedRetreived.Invoke(op.postFeedResponse);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            postFeedOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetFullsPagedByType(PostTypePagedRequest postTypePagedRequest)
    {
        PostFullsPagedByTypeOperation postFullsPagedByTypeOp = new PostFullsPagedByTypeOperation();
        try
        {
            postFullsPagedByTypeOp.postTypePagedRequest = postTypePagedRequest;
            postFullsPagedByTypeOp["on-complete"] = (Action<PostFullsPagedByTypeOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFullsPagedRetreived.Invoke(op.postFullsPagedResponse);
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            postFullsPagedByTypeOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // REGISTER
    public void RegisterShare(Share share)
    {
        ShareRegisterOperation shareRegisterOp = new ShareRegisterOperation();
        try
        {
            shareRegisterOp.share = share;
            shareRegisterOp["on-complete"] = (Action<ShareRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt64(op.shareId));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            shareRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void RegisterFavorite(Favorite favorite)
    {
        FavoriteRegisterOperation favoriteRegisterOp = new FavoriteRegisterOperation();
        try
        {
            favoriteRegisterOp.favorite = favorite;
            favoriteRegisterOp["on-complete"] = (Action<FavoriteRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt64(op.favoriteId));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            favoriteRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void RegisterComment(Comment comment)
    {
        CommentRegisterOperation commentRegisterOp = new CommentRegisterOperation();
        try
        {
            commentRegisterOp.comment = comment;
            commentRegisterOp["on-complete"] = (Action<CommentRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt64(op.commentId));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            commentRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void RegisterCommenPlaint(CommentPlaint commentPlaint)
    {
        CommentPlaintRegisterOperation commentPlaintRegisterOp = new CommentPlaintRegisterOperation();
        try
        {
            commentPlaintRegisterOp.commentPlaint = commentPlaint;
            commentPlaintRegisterOp["on-complete"] = (Action<CommentPlaintRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt64(op.commentPlaintId));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            commentPlaintRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void RegisterPostPlaint(PostPlaint postPlaint)
    {
        PostPlaintRegisterOperation postPlaintRegisterOp = new PostPlaintRegisterOperation();
        try
        {
            postPlaintRegisterOp.postPlaint = postPlaint;
            postPlaintRegisterOp["on-complete"] = (Action<PostPlaintRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt64(op.postPlaintId));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            postPlaintRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void RegisterPostRead(PostRead postRead)
    {
        PostReadRegisterOperation postReadRegisterOp = new PostReadRegisterOperation();
        try
        {
            postReadRegisterOp.postRead = postRead;
            postReadRegisterOp["on-complete"] = (Action<PostReadRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt64(op.postReadId));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            postReadRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void RegisterReaction(Reaction reaction)
    {
        ReactionRegisterOperation reactionRegisterOp = new ReactionRegisterOperation();
        try
        {
            reactionRegisterOp.reaction = reaction;
            reactionRegisterOp["on-complete"] = (Action<ReactionRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt64(op.reactionId));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            reactionRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void RegisterLike(Like like)
    {
        LikeRegisterOperation likeRegisterOp = new LikeRegisterOperation();
        try
        {
            likeRegisterOp.like = like;
            likeRegisterOp["on-complete"] = (Action<LikeRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt64(op.likeId));
                else
                    WebManager.Instance.OnResponseError(response, onResponseError, onTimeoutError);
            });
            likeRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}