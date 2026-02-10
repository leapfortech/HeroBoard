using System;

public class Referred
{
    public long Id { get; set; }
    public String Code { get; set; }
    public long AppUserId { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public long PhoneCountryId { get; set; }
    public String Phone { get; set; }
    public String Email { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime UpdateDateTime { get; set; }
    public int Status { get; set; }


    public Referred()
    {
    }

    public Referred(long id, String code, long appUserId, String firstName, String lastName, long phoneCountryId,
                    String phone, String email, DateTime createDateTime, DateTime updateDateTime, int status)
    {
        Id = id;
        Code = code;
        AppUserId = appUserId;
        FirstName = firstName;
        LastName = lastName;
        PhoneCountryId = phoneCountryId;
        Phone = phone;
        Email = email;
        CreateDateTime = createDateTime;
        UpdateDateTime = updateDateTime;
        Status = status;
    }
}