using System;
using UnityEngine;

using Leap.Graphics.Tools;

using hg.LitJson;

public class AppUserInfo
{
    public long Id { get; set; } = -1L;
    public String Alias { get; set; } = null;
    public String Thumbnail
    {
        get => null;
        set => ThumbnailSprite = value?.CreateSprite("Thumbnail");
    }
    [JsonIgnore]
    public Sprite ThumbnailSprite { get; set; } = null;
    public LocalityFull InterestLocality { get; set; } = null;
    public LocalityFull CurrentLocality { get; set; } = null;

    public AppUserInfo()
    {

    }

    public AppUserInfo(long id, String alias, LocalityFull interestLocality, LocalityFull currentLocality)
    {
        Id = id;
        Alias = alias;
        InterestLocality = interestLocality;
        CurrentLocality = currentLocality;
    }
}
