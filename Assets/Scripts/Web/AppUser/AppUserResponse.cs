using System;

public class AppUserResponse
{
    public AppUser AppUser { get; set; }
    public String IdDocFront { get; set; }
    public String IdDocBack { get; set; }

    public AppUserResponse()
    {
    }

    public AppUserResponse(AppUser appUser, String idDocFront, String idDocBack)
    {
        AppUser = appUser;
        IdDocFront = idDocFront;
        IdDocBack = idDocBack;
    }
}
