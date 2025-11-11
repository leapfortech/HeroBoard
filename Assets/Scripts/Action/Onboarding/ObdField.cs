using System;
using UnityEngine;

using Leap.UI.Elements;

using Sirenix.OdinInspector;

[Serializable]
public class ObdField
{
    [HideInInspector]
    public int Idx;
    [LabelWidth(80)]
    public String Name;
    [LabelWidth(80)]
    public Toggle Check;

    public ObdField(int idx)
    {
        Idx = idx;
    }
}
