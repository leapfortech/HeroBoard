using System;

using Sirenix.OdinInspector;

public class AppUser
{
    [ShowInInspector]
    public long Id { get; set; }
    [ShowInInspector]
    public String Code { get; set; }
    public long WebSysUserId { get; set; }
    public String CSToken { get; set; }
    public int Options { get; set; } = 11;
    public long ReferrerAppUserId { get; set; }
    public int AppUserStatusId { get; set; }


    public AppUser()
    {
    }

    public AppUser(long id, String code, long webSysUserId, String phone, String csToken, int options,
                   long referrerAppUserId, int appUserStatusId)
    {
        Id = id;
        Code = code;
        WebSysUserId = webSysUserId;
        CSToken = csToken;
        Options = options;
        ReferrerAppUserId = referrerAppUserId;
        AppUserStatusId = appUserStatusId;
    }
}
