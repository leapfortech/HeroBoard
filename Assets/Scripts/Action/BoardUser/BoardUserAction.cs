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

    [Title("List")]
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

    [Title("Roles")]
    [SerializeField]
    ListScroller lstRoles = null;
    [SerializeField]
    Text txtRolesEmpty = null;

    [Title("Filters")]
    [SerializeField]
    InputField txtFilterName = null;
    [SerializeField]
    Button btnFilter = null;
    //[SerializeField]
    //ComboAdapter cmbStatus = null;

    [Title("Sort")]
    [SerializeField]
    ToggleGroup tggSort = null;

    [Title("Pagination")]
    [SerializeField]
    Button btnNext = null;
    [SerializeField]
    Button btnBack = null;
    [SerializeField]
    Text txtPage = null;

    [Title("Config")]
    [SerializeField]
    int pageSize = 10;

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

    // State
    int currentPage = 1;
    int totalPages = 1;

    string filterName = null;
    int filterStatus = -1;

    public bool Selected { get; set; } = false;
    public long Id { get; set; } = -1;

    //private Dictionary<long, int> Idx = new Dictionary<long, int>();

    private BoardUserService boardUserService = null;
    private List<BoardUserFull> boardUserFulls = new List<BoardUserFull>();

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

    private void Start()
    {
        btnNext?.AddAction(NextPage);
        btnBack?.AddAction(BackPage);
        btnFilter?.AddAction(Filter);
    }

    public void ClearElements()
    {
        txtName.TextValue = "-";
        txtAuthUserId.TextValue = "-";
        txtEmail.TextValue = "-";
        txtPhone.TextValue = "-";
    }

    public void LoadFirstPage()
    {
        currentPage = 1;
        filterName = null;
        filterStatus = -1;

        txtFilterName.Clear();
        //cmbStatus.SelectIndex(0);

        GetPaged(currentPage);
    }

    public void Filter()
    {
        filterName = String.IsNullOrWhiteSpace(txtFilterName.Text) ? null : txtFilterName.Text;
        //filterStatus = Convert.ToInt32(cmbStatus.GetSelectedId());

        currentPage = 1;

        GetPaged(currentPage);
    }

    public void GetPaged(int page)
    {
        ScreenDialog.Instance.Display();

        currentPage = page;

        btnNext.Interactable = false;
        btnBack.Interactable = false;

        BoardUserAllByNameReq req = new BoardUserAllByNameReq(page, pageSize, filterName, filterStatus);

        boardUserService.GetFullAllByName(req);
    }

    public void FillFulls(BoardUserFullAllRsp rsp)
    {
        if (rsp == null || rsp.BoardUserFulls == null || rsp.BoardUserFulls.Count == 0)
        {
            ShowEmpty();
            return;
        }

        boardUserFulls = rsp.BoardUserFulls;
        totalPages = rsp.TotalPages;
        currentPage = rsp.Page;

        UpdatePagination();

        lstBoardUsers.ClearValues();

        SortItems(boardUserFulls);

        txtBoardUsersEmpty.gameObject.SetActive(false);

        //Idx.Clear();

        for (int i = 0; i < boardUserFulls.Count; i++)
        {
            BoardUserFull item = boardUserFulls[i];

            //Idx[item.BoardUser.Id] = i;

            ListScrollerValue value = new ListScrollerValue(lstBoardUsers.ListItem, true);
            value.SetText(0, item.WebSysUser.AuthUserId);
            value.SetText(1, item.Identity != null ? item.Identity.GetFullName() : "-");

            lstBoardUsers.AddValue(value);
        }

        lstBoardUsers.ApplyValues();
        lstBoardUsers.CheckToggle(0, true);

        Display(0);

        StateManager.Instance.BoardLoadHide();
    }

    public void UpdatePagination()
    {
        txtPage.TextValue = $"Página {currentPage} / {Mathf.Max(totalPages, 1)}";

        btnBack.Interactable = currentPage > 1;
        btnNext.Interactable = currentPage < totalPages;
    }

    public void ShowEmpty()
    {
        ClearElements();

        lstBoardUsers.ApplyClearValues();
        lstRoles.ApplyClearValues();
        txtBoardUsersEmpty.gameObject.SetActive(true);

        StateManager.Instance.BoardLoadHide();
    }

    public void NextPage()
    {
        if (currentPage >= totalPages) return;
        GetPaged(currentPage + 1);
    }

    public void BackPage()
    {
        if (currentPage <= 1) return;
        GetPaged(currentPage - 1);
    }

    public void SortChanged()
    {
        if (boardUserFulls != null)
            FillFulls(new BoardUserFullAllRsp(currentPage, totalPages, boardUserFulls));
    }

    private void SortItems(List<BoardUserFull> items)
    {
        int sortOption = Convert.ToInt32(tggSort.Value);

        for (int i = 0; i < items.Count - 1; i++)
        {
            for (int j = i + 1; j < items.Count; j++)
            {
                BoardUserFull a = items[i];
                BoardUserFull b = items[j];

                int compare = 0;

                // 1-2 Name
                if (sortOption == 1 || sortOption == 2)
                {
                    String nameA = a.Identity != null ? a.Identity.GetFullName() : "";
                    String nameB = b.Identity != null ? b.Identity.GetFullName() : "";

                    compare = String.Compare(nameA, nameB, StringComparison.OrdinalIgnoreCase);
                }
                // 3-4 Alias
                else if (sortOption == 3 || sortOption == 4)
                {
                    compare = String.Compare(
                    a.BoardUser.Alias,
                    b.BoardUser.Alias,
                    StringComparison.OrdinalIgnoreCase);
                }

                // Desc
                if (sortOption % 2 == 0)
                    compare = -compare;

                if (compare > 0)
                {
                    BoardUserFull temp = items[i];
                    items[i] = items[j];
                    items[j] = temp;
                }
            }
        }
    }

    public void Display(int idx)
    {
        boardUserFull = boardUserFulls[idx];
        Id = boardUserFull.BoardUser.Id;

        txtName.TextValue = boardUserFull.Identity != null ? boardUserFull.Identity.GetFullName() : "-";
        txtAuthUserId.TextValue = boardUserFull.WebSysUser.AuthUserId;
        txtEmail.TextValue = boardUserFull.WebSysUser.Email;
        txtPhone.TextValue = $"{vllCountry.FindRecordCellString(boardUserFull.WebSysUser.PhoneCountryId, 2)} {boardUserFull.WebSysUser.Phone}";

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

        for (int i = 0; i < roles.Count; i++)
        {
            ListScrollerValue value = new ListScrollerValue(lstRoles.ListItem, true);
            value.SetText(0, vllRole.FindRecordCellString(0, roles[i], 1));
            lstRoles.AddValue(value);
        }

        lstRoles.ApplyValues();
        txtRolesEmpty.gameObject.SetActive(false);

        onBoardUserFull.Invoke(boardUserFull);
    }
}
