using Leap.UI.Elements;
using System;
using UnityEngine;

public class CompanyLogo
{
    public String Name { get; set; }
    public Sprite Logo { get; set; }

    public CompanyLogo()
    {
    }

    public CompanyLogo(string name, Sprite logo)
    {
        Name = name;
        Logo = logo;
    }
}
