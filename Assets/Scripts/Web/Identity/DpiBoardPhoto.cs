using System;

public class DpiBoardPhoto
{
    public String[] DpiFronts { get; set; }
    public String[] DpiBacks { get; set; }
    public String DpiPortrait { get; set; }

    public DpiBoardPhoto()
    {
    }

    public DpiBoardPhoto(String[] dpiFronts, String[] dpiBacks, String dpiPortrait)
    {
        DpiFronts = dpiFronts;
        DpiBacks = dpiBacks;
        DpiPortrait = dpiPortrait;
    }
}
