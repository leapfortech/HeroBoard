using System;
using System.Collections.Generic;
using UnityEngine;

using Leap.Data.Collections;
using Leap.Data.Mapper;
using Leap.Data.Web;
using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.UI.Extensions;

using Sirenix.OdinInspector;

public class BoardUserChangeAction : MonoBehaviour
{
    [Title("Fields")]
    [SerializeField]
    Text lblBoardUser = null;
    [SerializeField]
    InputField ifdEmail = null;
    [SerializeField]
    InputField ifdPassword = null;
    [SerializeField]
    InputField ifdConfirm = null;
    [SerializeField]
    RectTransform trfPhonePrefix = null;

    [Title("Roles")]
    [SerializeField]
    ListScroller lstRoles = null;
    [SerializeField]
    ComboAdapter cmbRole = null;
    //[SerializeField]
    //Text txtRolesEmpty = null;
    [SerializeField]
    int maxRoles = 10;
    [SerializeField]
    ValueList vllRole = null;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmRegisterBoardRequest = null;
    [SerializeField]
    DataMapper dtmBoardUserUpdate = null;
    [SerializeField]
    DataMapper dtmWebSysUserUpdate = null;

    [Title("Actions")]
    [SerializeField]
    Button btnChange = null;

    public bool Selected { get; set; } = false;

    AccessService accessService = null;
    BoardUserService boardUserService = null;

    BoardUserFull boardUserFull = null;
    RegisterBoardRequest registerRequest = null;

    Vector2 posPassword, posPhonePrefix;
    List<String> roles = new List<String>();

    private void Awake()
    {
        accessService = GetComponent<AccessService>();
        boardUserService = GetComponent<BoardUserService>();

        posPassword = ifdPassword.GetComponent<RectTransform>().anchoredPosition;
        posPhonePrefix = trfPhonePrefix.anchoredPosition;
    }

    public void Clear()
    {
        dtmRegisterBoardRequest.ClearElements();
        dtmBoardUserUpdate.ClearElements();
        dtmWebSysUserUpdate.ClearElements();
        ifdConfirm.Clear();

        roles.Clear();
        DisplayRoles();
    }

    public void ChangeUserBoard()
    {
        if (btnChange.Title[0] == 'A')
            RegisterUserBoard();
        else
            UpdateUserBoard();
    }

    public void SetBoardUserFull(BoardUserFull boardUser)
    {
        boardUserFull = boardUser;
    }

    // Add

    public void DisplayAdd()
    {
        lblBoardUser.TextValue = "Registrar un usuario";
        btnChange.Title = "Agregar";

        Clear();

        Invoke(nameof(_DisplayAdd), 0.05f);
    }

    private void _DisplayAdd()
    {
        ifdEmail.Interactable = true;
        ifdPassword.gameObject.SetActive(true);
        ifdConfirm.gameObject.SetActive(true);
        trfPhonePrefix.anchoredPosition = posPhonePrefix;
    }

    private void RegisterUserBoard()
    {
        if (!dtmRegisterBoardRequest.ValidateElements())
            return;

        if (ifdPassword.Text != ifdConfirm.Text)
        {
            ChoiceDialog.Instance.Error("Los campos de contraseña no coinciden.\r\nPor favor, ingrésalos de nuevo.");
            return;
        }

        if (roles.Count == 0)
        {
            ChoiceDialog.Instance.Error("Es requerido agregar por lo menos un rol al usuario.");
            return;
        }

        ScreenDialog.Instance.Display();

        registerRequest = dtmRegisterBoardRequest.BuildClass<RegisterBoardRequest>();
        registerRequest.Roles = String.Join("|", roles);

        accessService.RegisterBoard(registerRequest);
    }

    // Update

    public void DisplayUpdate()
    {
        lblBoardUser.TextValue = "Editar un usuario";
        btnChange.Title = "Guardar";

        dtmBoardUserUpdate.PopulateClass(boardUserFull.BoardUser);
        dtmWebSysUserUpdate.PopulateClass(boardUserFull.WebSysUser);

        roles = new List<String>(boardUserFull.WebSysUser.Roles.Split('|'));
        DisplayRoles();

        Invoke(nameof(_DisplayUpdate), 0.05f);
    }

    private void _DisplayUpdate()
    {
        ifdEmail.Interactable = false;
        ifdPassword.gameObject.SetActive(false);
        ifdConfirm.gameObject.SetActive(false);
        trfPhonePrefix.anchoredPosition = posPassword;
    }

    private void DisplayRoles()
    {
        if (roles.Count == 0)
        {
            lstRoles.ApplyClearValues();
            //txtRolesEmpty.gameObject.SetActive(true);
            return;
        }

        lstRoles.ClearValues();

        ListScrollerValue lstRoleValue;
        for (int idx = 0; idx < roles.Count; idx++)
        {
            //Idx[lstRoles[idx].Id] = idx;

            lstRoleValue = new ListScrollerValue(1, true);
            lstRoleValue.SetText(0, vllRole.FindRecordCellString(0, roles[idx], 1));

            lstRoles.AddValue(lstRoleValue);
        }

        lstRoles.ApplyValues();
        //txtRolesEmpty.gameObject.SetActive(false);
    }

    private void UpdateUserBoard()
    {
        if (!dtmBoardUserUpdate.ValidateElements())
            return;
        if (!dtmWebSysUserUpdate.ValidateElements())
            return;

        if (roles.Count == 0)
        {
            ChoiceDialog.Instance.Error("Es requerido agregar por lo menos un rol al usuario.");
            return;
        }

        ScreenDialog.Instance.Display();

        BoardUserFull updBoardUserFull = new BoardUserFull();
        updBoardUserFull.BoardUser = dtmBoardUserUpdate.BuildClass<BoardUser>();
        updBoardUserFull.WebSysUser = dtmWebSysUserUpdate.BuildClass<WebSysUser>();

        updBoardUserFull.BoardUser.Id = boardUserFull.BoardUser.Id;
        updBoardUserFull.BoardUser.WebSysUserId = boardUserFull.BoardUser.WebSysUserId;
        updBoardUserFull.BoardUser.BoardUserStatusId = boardUserFull.BoardUser.BoardUserStatusId;

        updBoardUserFull.WebSysUser.Id = boardUserFull.WebSysUser.Id;
        updBoardUserFull.WebSysUser.AuthUserId = boardUserFull.WebSysUser.AuthUserId;
        updBoardUserFull.WebSysUser.Roles = String.Join('|', roles);
        updBoardUserFull.WebSysUser.WebSysUserStatusId = boardUserFull.WebSysUser.WebSysUserStatusId;

        boardUserService.UpdateFull(updBoardUserFull);
    }

    // Roles
    public void AddRole()
    {
        if (cmbRole.Combo.IsEmpty())
            return;

        if (roles.Count >= maxRoles)
        {
            ChoiceDialog.Instance.Error("No se pueden ingresar más de " + maxRoles + " roles.");
            return;
        }

        int cmbRoleId = cmbRole.GetSelectedId();
        String role = vllRole.FindRecordCellString(cmbRoleId, 0);

        if (roles.Contains(role))
        {
            String roleName = vllRole.FindRecordCellString(cmbRoleId, 1);
            ChoiceDialog.Instance.Error($"El rol <b>{roleName}</b> ya fue agregado.");
            return;
        }

        roles.Add(role);

        cmbRole.Clear();
        DisplayRoles();
    }

    public void RemoveRole(int idx)
    {
        roles.RemoveAt(idx);
        DisplayRoles();
    }
}