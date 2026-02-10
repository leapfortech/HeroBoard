using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.Data.Collections;

using Sirenix.OdinInspector;

public class BoardUserAction : MonoBehaviour
{
    [Serializable]
    public class BoardUserFullEvent : UnityEvent<BoardUserFull> { }

    [Title("Meetings")]
    [SerializeField]
    ListScroller lstBoardUsers = null;
    [SerializeField]
    Text txtBoardUsersEmpty = null;

    [Title("Fields")]
    [SerializeField]
    Text txtName = null;
    [SerializeField]
    Text txtAuthUserId = null;
    [SerializeField]
    Text txtEmail = null;
    [SerializeField]
    Text txtPhone = null;

    [Title("Appointments")]
    [SerializeField]
    ListScroller lstRoles = null;
    [SerializeField]
    Text txtRolesEmpty = null;

    [Title("Data")]
    [SerializeField]
    ValueList vllCountry = null;
    [SerializeField]
    ValueList vllRole = null;

    [Title("Actions")]
    [SerializeField]
    Button btnAdd = null;
    [SerializeField]
    Button btnUpdate = null;

    [Title("Event")]
    [SerializeField]
    BoardUserFullEvent onBoardUserFull = null;

    public bool Selected { get; set; } = false;
    public long Id { get; set; } = -1;
    private Dictionary<long, int> Idx = new Dictionary<long, int>();

    private BoardUserService boardUserService = null;
    private BoardUserFull[] boardUserFulls = null;

    private BoardUserFull boardUserFull = null;
    public BoardUserFull BoardUserFull => boardUserFull;

    private List<String> roles = new List<String>();

    RectTransform trfAdd;
    Vector2 posAdd, posUpdate;

    private void Awake()
    {
        boardUserService = GetComponent<BoardUserService>();

        trfAdd = btnAdd.GetComponent<RectTransform>();
        posAdd = trfAdd.anchoredPosition;

        posUpdate = btnUpdate.GetComponent<RectTransform>().anchoredPosition;
    }

    public void Clear()
    {
        txtName.TextValue = "-";
        txtAuthUserId.TextValue = "-";
        txtEmail.TextValue = "-";
        txtPhone.TextValue = "-";

        roles.Clear();
        lstRoles.ApplyClearValues();
        txtRolesEmpty.gameObject.SetActive(false);
    }

    public void GetFulls()
    {
        ScreenDialog.Instance.Display();

        lstBoardUsers.ApplyClearValues();
        txtBoardUsersEmpty.gameObject.SetActive(false);

        boardUserFull = null;
        boardUserService.GetFulls();
    }

    public void FillFulls(BoardUserFull[] boardUsers)
    {
        boardUserFulls = boardUsers;
        Idx.Clear();

        if (boardUserFulls.Length == 0)
        {
            lstBoardUsers.ApplyClearValues();
            txtBoardUsersEmpty.gameObject.SetActive(true);

            trfAdd.anchoredPosition = posUpdate;
            btnUpdate.gameObject.SetActive(false);

            StateManager.Instance.BoardLoadHide();
            return;
        }

        lstBoardUsers.ClearValues();

        ListScrollerValue lstBoardUserValue;
        for (int idx = 0; idx < boardUserFulls.Length; idx++)
        {
            Idx[boardUserFulls[idx].BoardUser.Id] = idx;

            lstBoardUserValue = new ListScrollerValue(4, true);
            lstBoardUserValue.SetText(0, boardUserFulls[idx].WebSysUser.AuthUserId);
            lstBoardUserValue.SetText(1, boardUserFulls[idx].BoardUser.GetCompleteName());
            //lstBoardUserValue.SetText(1, vllRole.FindRecordCellString(boardUserFulls[idx].WebSysUser.Roles, 0));

            lstBoardUsers.AddValue(lstBoardUserValue);
        }

        lstBoardUsers.ApplyValues();

        trfAdd.anchoredPosition = posAdd;
        btnUpdate.gameObject.SetActive(true);
        lstBoardUsers.CheckToggle(Id == -1 ? 0 : Idx[Id], true);

        StateManager.Instance.BoardLoadHide();
    }

    public void Display(int idx)
    {
        boardUserFull = boardUserFulls[idx];
        Id = boardUserFull.BoardUser.Id;

        txtName.TextValue = boardUserFull.BoardUser.GetCompleteName();
        txtAuthUserId.TextValue = boardUserFull.WebSysUser.AuthUserId;
        txtEmail.TextValue = boardUserFull.WebSysUser.Email;
        txtPhone.TextValue = $"{vllCountry.FindRecordCellString(boardUserFull.WebSysUser.PhoneCountryId, 2)} {boardUserFull.WebSysUser.Phone}";  // "PhonePrefix"

        if (String.IsNullOrEmpty(boardUserFull.WebSysUser.Roles))
        {
            roles.Clear();
            lstRoles.ApplyClearValues();
            txtRolesEmpty.gameObject.SetActive(true);

            onBoardUserFull.Invoke(boardUserFull);
            return;
        }

        roles = new List<String>(boardUserFull.WebSysUser.Roles.Split('|'));

        lstRoles.ClearValues();

        ListScrollerValue lstRolesValue;
        for (int i = 0; i < roles.Count; i++)
        {
            lstRolesValue = new ListScrollerValue(1, true);
            lstRolesValue.SetText(0, vllRole.FindRecordCellString(0, roles[i], 1));  // "Code", , "Name"

            lstRoles.AddValue(lstRolesValue);
        }

        lstRoles.ApplyValues();
        txtRolesEmpty.gameObject.SetActive(false);

        onBoardUserFull.Invoke(boardUserFull);
    }
}
