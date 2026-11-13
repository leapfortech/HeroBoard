using System;

public class ContactFull
{
    public long Id { get; set; } = -1L;
    public long PostId { get; set; } = -1L;
    public String Name { get; set; } = null;
    public int Status { get; set; } = -1;

    public ContactFull()
    {
    }

    public ContactFull(long id, long postId, String name, int status)
    {
        Id = id;
        PostId = postId;
        Name = name;
        Status = status;
    }
}
