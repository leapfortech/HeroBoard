using UnityEngine;

using Leap.UI.Elements;
using Leap.UI.Dialog;

using Sirenix.OdinInspector;
using System;
using Leap.Core.Tools;


public class PostDisplayAction : MonoBehaviour
{
    [Title("Params")]
    [SerializeField]
    int postTypeId = -1;
    [SerializeField]
    int status = -1;

    [Title("List")]
    [SerializeField]
    ListScroller lstPost = null;
    [SerializeField]
    Text txtEmpty = null;

    [Title("Navigation")]
    [SerializeField]
    Button btnNext = null;
    [SerializeField]
    Button btnBack = null;
    [SerializeField]
    Text txtPageIndex = null;

    [Title("Config")]
    [SerializeField]
    int pageSize = 10;
    [SerializeField]
    Sprite emptyImage = null;

    [Title("Event")]
    [SerializeField]
    UnityLongEvent onPostSelected = null;

    // Navigation
    int currentPage = 1;
    int totalPages = 1;

    // Filters
    int filterStatus = -1;
    long filterType = -1;

    PostService postService = null;
    PostFullsPagedResponse postFullsPagedResponse = null;

    bool isFirstDisplay = false;

    private void Awake()
    {
        postService = GetComponent<PostService>();
    }

    private void Start()
    {
        btnNext?.AddAction(NextPage);
        btnBack?.AddAction(BackPage);
    }

    public void Clear()
    {
        isFirstDisplay = false;
        postFullsPagedResponse = null;
        txtEmpty.gameObject.SetActive(true);
        lstPost.Clear();
    }

    public void DisplayFirstPage()
    {
        if (!isFirstDisplay)
        { 
            isFirstDisplay = true;
            LoadFirstPage();
        }
    }

    public void LoadFirstPage()
    {
        filterType = postTypeId;
        filterStatus = status;
        currentPage = 1;

        GetPaged(currentPage);
    }

    public void SelectPost(int idx)
    {
        onPostSelected.Invoke(postFullsPagedResponse.PostFulls[idx].PostId);
    }

    // Navigation
    public void NextPage()
    {
        if (currentPage >= totalPages)
            return;
        
        GetPaged(currentPage + 1);
    }

    public void BackPage()
    {
        if (currentPage <= 1)
            return;
        
        GetPaged(currentPage - 1);
    }

    void GetPaged(int page)
    {
        ScreenDialog.Instance.Display();

        currentPage = page;

        btnNext.Interactable = false;
        btnBack.Interactable = false;

        PostTypePagedRequest postTypePagedRequest = new PostTypePagedRequest(page, pageSize, filterType, filterStatus);

        postService.GetFullsPagedByType(postTypePagedRequest);
    }

    public void FillPaged(PostFullsPagedResponse response)
    {
        postFullsPagedResponse = response;

        if (postFullsPagedResponse == null || postFullsPagedResponse.PostFulls.Count == 0)
        {
            ShowEmpty();
            return;
        }

        totalPages = postFullsPagedResponse.TotalPages;
        currentPage = postFullsPagedResponse.Page;

        UpdatePagination();

        lstPost.ClearValues();
        
        txtEmpty.gameObject.SetActive(false);

        for (int i = 0; i < postFullsPagedResponse.PostFulls.Count; i++)
        {
            ListScrollerValue value = new ListScrollerValue(4, true);

            value.SetText(0, postFullsPagedResponse.PostFulls[i].Title);
            value.SetText(1, postFullsPagedResponse.PostFulls[i].PublicationDateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm"));
            value.SetSprite(2, postFullsPagedResponse.PostFulls[i].ImageCount == 0 ? emptyImage : postFullsPagedResponse.PostFulls[i].TitleSprite);

            String description = postFullsPagedResponse.PostFulls[i].Description ?? "";
            value.SetText(3, description.Length > 300 ? description.Substring(0, 300) + "..." : description);

            lstPost.AddValue(value);
        }

        lstPost.ApplyValues();

        StateManager.Instance.BoardLoadHide();
    }

    void UpdatePagination()
    {
        txtPageIndex.TextValue = $"Página {currentPage} / {Mathf.Max(totalPages, 1)}";

        btnBack.Interactable = currentPage > 1;
        btnNext.Interactable = currentPage < totalPages;
    }

    void ShowEmpty()
    {
        txtEmpty.gameObject.SetActive(true);
        lstPost.ApplyClearValues();

        StateManager.Instance.BoardLoadHide();
    }
}