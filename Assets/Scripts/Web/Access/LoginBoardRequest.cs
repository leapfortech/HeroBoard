using System;

public class LoginBoardRequest
{
    public String Email { get; set; }
    public String Version { get; set; }

    public LoginBoardRequest()
    {
    }

    public LoginBoardRequest(String email, String version)
    {
        Email = email;
        Version = version;
    }
}
