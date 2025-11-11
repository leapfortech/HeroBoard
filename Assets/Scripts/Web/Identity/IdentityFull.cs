using System;

using Sirenix.OdinInspector;

public class IdentityFull
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public String FirstName1 { get; set; }
    public String FirstName2 { get; set; }
    public String FirstName3 { get; set; }
    public String FirstNames => FirstName1 + (String.IsNullOrEmpty(FirstName2) ? "" : (" " + FirstName2));
    public String LastName1 { get; set; }
    public String LastName2 { get; set; }
    public String LastNameMarried { get; set; }
    public String LastNames => LastName1 + (String.IsNullOrEmpty(LastName2) ? "" : (" " + LastName2)) + (String.IsNullOrEmpty(LastNameMarried) ? "" : (" de " + LastNameMarried));
    public String Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public String BirthCountry { get; set; }
    public String BirthState { get; set; }
    public String BirthCity { get; set; }
    public String BirthPlace => String.IsNullOrEmpty(BirthState) ? BirthCountry : (BirthCity + ", " + BirthState + ", " + BirthCountry);
    public String Nationalities { get; set; }
    public String MaritalStatus { get; set; }
    public String Occupation { get; set; }
    public String Nit { get; set; }
    public String DpiCui { get; set; }
    public DateTime DpiIssueDate { get; set; }
    public DateTime DpiDueDate { get; set; }
    public String DpiVersion { get; set; }
    public String DpiSerie { get; set; }
    public String DpiMrz { get; set; }
    public String PhonePrefix { get; set; }
    public String Phone { get; set; }
    public String Email { get; set; }
    public int IsPep { get; set; }
    public int HasPepIdentity { get; set; }
    public int IsCpe { get; set; }
    public int Investments { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime UpdateDateTime { get; set; }
    public int AppUserStatusId { get; set; }
    public int Status { get; set; }


    public IdentityFull()
    {
    }

    public IdentityFull(int id, int appUserId, String firstName1, String firstName2, String firstName3, String lastName1, String lastName2, String lastNameMarried,
                        String gender, DateTime birthDate, String birthCountry, String birthState, String birthCity, String nationalities, String maritalStatus,
                        String occupation, String nit, String dpiCui, DateTime dpiIssueDate, DateTime dpiDueDate, String dpiVersion, String dpiSerie, String dpiMrz,
                        String phonePrefix, String phone, String email, int isPep, int hasPepIdentity, int isCpe, int investments,
                        DateTime createDateTime, DateTime updateDateTime, int appUserStatusId, int status)
    {
        Id = id;
        AppUserId = appUserId;
        FirstName1 = firstName1;
        FirstName2 = firstName2;
        FirstName3 = firstName3;
        LastName1 = lastName1;
        LastName2 = lastName2;
        LastNameMarried = lastNameMarried;
        Gender = gender;
        BirthDate = birthDate;
        BirthCountry = birthCountry;
        BirthState = birthState;
        BirthCity = birthCity;
        Nationalities = nationalities;
        MaritalStatus = maritalStatus;
        Occupation = occupation;
        Nit = nit;
        DpiCui = dpiCui;
        DpiIssueDate = dpiIssueDate;
        DpiDueDate = dpiDueDate;
        DpiVersion = dpiVersion;
        DpiSerie = dpiSerie;
        DpiMrz = dpiMrz;
        PhonePrefix = phonePrefix;
        Phone = phone;
        Email = email;
        IsPep = isPep;
        HasPepIdentity = hasPepIdentity;
        IsCpe = isCpe;
        Investments = investments;
        CreateDateTime = createDateTime;
        UpdateDateTime = updateDateTime;
        AppUserStatusId = appUserStatusId;
        Status = status;
    }

    public IdentityFull(IdentityFull identityFull)
    {
        Id = identityFull.Id;
        AppUserId = identityFull.AppUserId;
        FirstName1 = identityFull.FirstName1;
        FirstName2 = identityFull.FirstName2;
        FirstName3 = identityFull.FirstName3;
        LastName1 = identityFull.LastName1;
        LastName2 = identityFull.LastName2;
        LastNameMarried = identityFull.LastNameMarried;
        Gender = identityFull.Gender;
        BirthDate = identityFull.BirthDate;
        BirthCountry = identityFull.BirthCountry;
        BirthState = identityFull.BirthState;
        BirthCity = identityFull.BirthCity;
        Nationalities = identityFull.Nationalities;
        MaritalStatus = identityFull.MaritalStatus;
        Occupation = identityFull.Occupation;
        Nit = identityFull.Nit;
        DpiCui = identityFull.DpiCui;
        DpiIssueDate = identityFull.DpiIssueDate;
        DpiDueDate = identityFull.DpiDueDate;
        DpiVersion = identityFull.DpiVersion;
        DpiSerie = identityFull.DpiSerie;
        DpiMrz = identityFull.DpiMrz;
        PhonePrefix = identityFull.PhonePrefix;
        Phone = identityFull.Phone;
        Email = identityFull.Email;
        IsPep = identityFull.IsPep;
        HasPepIdentity = identityFull.HasPepIdentity;
        IsCpe = identityFull.IsCpe;
        Investments = identityFull.Investments;
        CreateDateTime = identityFull.CreateDateTime;
        UpdateDateTime = identityFull.UpdateDateTime;
        AppUserStatusId = identityFull.AppUserStatusId;
        Status = identityFull.Status;
    }
}
