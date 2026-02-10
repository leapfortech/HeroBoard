using System;

using Sirenix.OdinInspector;

public class AppUserNamed
{
    [ShowInInspector]
    public long Id { get; set; }
    [ShowInInspector]
    public String Code { get; set; }
    public long WebSysUserId { get; set; }
    [ShowInInspector]
    public String FirstName1 { get; set; }
    [ShowInInspector]
    public String FirstName2 { get; set; }
    [ShowInInspector]
    public String LastName1 { get; set; }
    [ShowInInspector]
    public String LastName2 { get; set; }
    [ShowInInspector]
    public String LastNameMarried { get; set; }
    [ShowInInspector]
    public String Email { get; set; }
    public long PhoneCountryId { get; set; } = -1;
    [ShowInInspector]
    public String Phone { get; set; }
    public String CSToken { get; set; }
    public int Options { get; set; }
    public long ReferrerAppUserId { get; set; }
    public int AppUserStatusId { get; set; }


    public AppUserNamed()
    {
    }

    public AppUserNamed(long id, String code, long webSysUserId, String firstName1, String firstName2, String lastName1, String lastName2, String lastNameMarried,
                        String email, long phoneCountryId, String phone, String cSToken, int options, long referrerAppUserId, int appUserStatusId)
    {
        Id = id;
        Code = code;
        WebSysUserId = webSysUserId;
        FirstName1 = firstName1;
        FirstName2 = firstName2;
        LastName1 = lastName1;
        LastName2 = lastName2;
        LastNameMarried = lastNameMarried;
        Email = email;
        PhoneCountryId = phoneCountryId;
        Phone = phone;
        CSToken = cSToken;
        Options = options;
        ReferrerAppUserId = referrerAppUserId;
        AppUserStatusId = appUserStatusId;
    }
}
