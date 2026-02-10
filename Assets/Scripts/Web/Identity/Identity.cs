using System;

using Sirenix.OdinInspector;


public class Identity
{
    public long Id { get; set; }
    public long AppUserId { get; set; }
    public String FirstName1 { get; set; }
    public String FirstName2 { get; set; }
    public String FirstName3 { get; set; }
    public String LastName1 { get; set; }
    public String LastName2 { get; set; }
    public String LastNameMarried { get; set; }
    public long GenderId { get; set; }
    public DateTime BirthDate { get; set; }
    public long BirthCountryId { get; set; }
    public long BirthStateId { get; set; }
    public long BirthCityId { get; set; }
    public String NationalityIds { get; set; }
    public long MaritalStatusId { get; set; }
    public String Occupation { get; set; }
    public String Nit { get; set; }
    public String DpiCui { get; set; }
    public DateTime DpiIssueDate { get; set; }
    public DateTime DpiDueDate { get; set; }
    public String DpiVersion { get; set; }
    public String DpiSerie { get; set; }
    public String DpiMrz { get; set; }
    public long PhoneCountryId { get; set; }
    public String Phone { get; set; }
    public String Email { get; set; }
    public int IsPep { get; set; }
    public int HasPepIdentity { get; set; }
    public int IsCpe { get; set; }
    public int Status { get; set; }


    public Identity()
    {
    }

    public Identity(long id, long appUserId, String firstName1, String firstName2, String firstName3, String lastName1, String lastName2, String lastNameMarried,
                    long genderId, DateTime birthDate, long birthCountryId, long birthStateId, long birthCityId, String nationalityIds, long maritalStatusId,
                    String occupation, String nit, String dpiCui, DateTime dpiIssueDate, DateTime dpiDueDate, String dpiVersion, String dpiSerie, String dpiMrz,
                    long phoneCountryId, String phone, String email, int isPep, int hasPepIdentity, int isCpe, int status)
    {
        Id = id;
        AppUserId = appUserId;
        FirstName1 = firstName1;
        FirstName2 = firstName2;
        FirstName3 = firstName3;
        LastName1 = lastName1;
        LastName2 = lastName2;
        LastNameMarried = lastNameMarried;
        GenderId = genderId;
        BirthDate = birthDate;
        BirthCountryId = birthCountryId;
        BirthStateId = birthStateId;
        BirthCityId = birthCityId;
        NationalityIds = nationalityIds;
        MaritalStatusId = maritalStatusId;
        Occupation = occupation;
        Nit = nit;
        DpiCui = dpiCui;
        DpiIssueDate = dpiIssueDate;
        DpiDueDate = dpiDueDate;
        DpiVersion = dpiVersion;
        DpiSerie = dpiSerie;
        DpiMrz = dpiMrz;
        PhoneCountryId = phoneCountryId;
        Phone = phone;
        Email = email;
        IsPep = isPep;
        HasPepIdentity = hasPepIdentity;
        IsCpe = isCpe;
        Status = status;
    }

    public Identity(Identity identity)
    {
        Id = identity.Id;
        AppUserId = identity.AppUserId;
        FirstName1 = identity.FirstName1;
        FirstName2 = identity.FirstName2;
        FirstName3 = identity.FirstName3;
        LastName1 = identity.LastName1;
        LastName2 = identity.LastName2;
        LastNameMarried = identity.LastNameMarried;
        GenderId = identity.GenderId;
        BirthDate = identity.BirthDate;
        BirthCountryId = identity.BirthCountryId;
        BirthStateId = identity.BirthStateId;
        BirthCityId = identity.BirthCityId;
        NationalityIds = identity.NationalityIds;
        MaritalStatusId = identity.MaritalStatusId;
        Occupation = identity.Occupation;
        Nit = identity.Nit;
        DpiCui = identity.DpiCui;
        DpiIssueDate = identity.DpiIssueDate;
        DpiDueDate = identity.DpiDueDate;
        DpiVersion = identity.DpiVersion;
        DpiSerie = identity.DpiSerie;
        DpiMrz = identity.DpiMrz;
        PhoneCountryId = identity.PhoneCountryId;
        Phone = identity.Phone;
        Email = identity.Email;
        IsPep = identity.IsPep;
        HasPepIdentity = identity.HasPepIdentity;
        IsCpe = identity.IsCpe;
        Status = identity.Status;
    }

    public String GetFullName()
    {
        return FirstName1 + (String .IsNullOrEmpty(FirstName2) ? "" : (" " + FirstName2)) + " " + LastName1 + (String.IsNullOrEmpty(LastName2) ? "" : (" " + LastName2));
    }
}
