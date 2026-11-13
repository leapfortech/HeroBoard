using System;

public class LinkFull
{
    public long Id { get; set; } = -1L;
    public long LinkTypeId { get; set; } = -1L;
    public long PostId { get; set; } = -1L;
    public String Url { get; set; } = null;
    public int Status { get; set; } = -1;

    public LinkFull()
    {
    }

    public LinkFull(long id, long linkTypeId, long postId, String url, int status)
    {
        Id = id;
        LinkTypeId = linkTypeId;
        PostId = postId;
        Url = url;
        Status = status;
    }
}
