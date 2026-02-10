using System;

public class IdDocRequest
{
    public long AppUserId { get; set; }
    public String IdDocNumber { get; set; }
    public long IdDocStateId { get; set; }
    public DateTime IdDocDate { get; set; }
    public String IdDocFront { get; set; }
    public String IdDocBack { get; set; }

    public IdDocRequest()
    {
    }

    public IdDocRequest(long appUserId, String idDocNumber, long idDocStateId, DateTime idDocDate,
                        String idDocFront, String idDocBack)
    {
        AppUserId = appUserId;
        IdDocNumber = idDocNumber;
        IdDocStateId = idDocStateId;
        IdDocDate = idDocDate;
        IdDocFront = idDocFront;
        IdDocBack = idDocBack;
    }
}
