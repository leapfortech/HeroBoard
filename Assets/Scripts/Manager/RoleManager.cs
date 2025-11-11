using System;
using UnityEngine;

using Leap.Core.Tools;
using Leap.Data.Web;
using Leap.UI.Elements;

using Sirenix.OdinInspector;

public class RoleManager : SingletonBehaviour<RoleManager>
{
    [Title("Elements")]
    [SerializeField]
    private ElementRole[] elementRoles = null;

    void Start()
    {
    }

    public void ApplyRoles()
    {
        Invoke(nameof(_ApplyRoles), 0.05f);
    }

    public void _ApplyRoles()
    {
        String roles = WebManager.Instance.WebSysUser.Roles;

        for (int i = 0; i < elementRoles.Length; i++)
        {
            bool hasRole = false;
            for (int k = 0; k < elementRoles[i].Roles.Length; k++)
                if (roles.Contains(elementRoles[i].Roles[k]))
                {
                    hasRole = true;
                    break;
                }

            if (elementRoles[i].Element is Toggle toggle)
                toggle.Interactable = hasRole;
            else if (elementRoles[i].Element is Button button)
                button.Interactable = hasRole;

            elementRoles[i].Element.SetStyle();
        }
    }
}
