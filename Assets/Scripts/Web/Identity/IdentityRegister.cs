using System;

public class IdentityRegister
{
    public IdentityInfo IdentityFull { get; set; }
    public String Portrait { get; set; }

    public IdentityRegister()
    {
    }

    public IdentityRegister(IdentityInfo identityFull, String portrait)
    {
        IdentityFull = identityFull;
        Portrait = portrait;
    }
}
