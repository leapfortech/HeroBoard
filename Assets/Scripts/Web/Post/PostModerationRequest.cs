using UnityEngine;

public class PostModerationRequest
{
    public long PostId { get; set; }
    public long TypeId { get; set; }

    public PostModerationRequest()
    {
    }

    public PostModerationRequest(long postId, long typeId)
    {
        PostId = postId;
        TypeId = typeId;
    }
}
