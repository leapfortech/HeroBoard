using System;

using Sirenix.OdinInspector;

public class IdentityFull
{
    public long Id { get; set; }
    public String FirstName1 { get; set; }
    public String FirstName2 { get; set; }
    public String FirstNames => FirstName1 + (String.IsNullOrEmpty(FirstName2) ? "" : (" " + FirstName2));
    public String LastName1 { get; set; }
    public String LastName2 { get; set; }
    public String LastNames => LastName1 + (String.IsNullOrEmpty(LastName2) ? "" : (" " + LastName2));
    public String Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public String OriginCountry { get; set; }
    public String OriginState { get; set; }
    public String PhonePrefix { get; set; }
    public String Phone { get; set; }
    public String Email { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime UpdateDateTime { get; set; }
    public int AppUserStatusId { get; set; }
    public int Status { get; set; }


    public IdentityFull()
    {
    }

    public IdentityFull(long id, String firstName1, String firstName2, String lastName1, String lastName2,
                            String gender, DateTime birthDate, String originCountry, String originState,
                            String phonePrefix, String phone, String email,
                            DateTime createDateTime, DateTime updateDateTime, int appUserStatusId, int status)
    {
        Id = id;
        FirstName1 = firstName1;
        FirstName2 = firstName2;
        LastName1 = lastName1;
        LastName2 = lastName2;
        Gender = gender;
        BirthDate = birthDate;
        OriginCountry = originCountry;
        OriginState = originState;
        PhonePrefix = phonePrefix;
        Phone = phone;
        Email = email;
        CreateDateTime = createDateTime;
        UpdateDateTime = updateDateTime;
        AppUserStatusId = appUserStatusId;
        Status = status;
    }

    public IdentityFull(IdentityFull identityFull)
    {
        Id = identityFull.Id;
        FirstName1 = identityFull.FirstName1;
        FirstName2 = identityFull.FirstName2;
        LastName1 = identityFull.LastName1;
        LastName2 = identityFull.LastName2;
        Gender = identityFull.Gender;
        BirthDate = identityFull.BirthDate;
        OriginCountry = identityFull.OriginCountry;
        OriginState = identityFull.OriginState;
        PhonePrefix = identityFull.PhonePrefix;
        Phone = identityFull.Phone;
        Email = identityFull.Email;
        CreateDateTime = identityFull.CreateDateTime;
        UpdateDateTime = identityFull.UpdateDateTime;
        AppUserStatusId = identityFull.AppUserStatusId;
        Status = identityFull.Status;
    }
}
