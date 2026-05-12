using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Leap.UI.Elements;
using Leap.UI.Page;
using Leap.UI.Dialog;
using Leap.Core.Tools;
using Leap.Data.Collections;

using Sirenix.OdinInspector;


public class NewsDisplayAction : MonoBehaviour
{
    [Serializable]
    public class ImagesEvent : UnityEvent<List<Sprite>> { }
    [Space]
    [Title("Details")]
    [SerializeField]
    Text txtAlias = null;
    [SerializeField]
    Text txtDateTime = null;
    [SerializeField]
    Text txtTitle = null;
    [SerializeField]
    Text txtSummary = null;
    [SerializeField]
    Text txtDescription = null;
    [SerializeField]
    Text txtCountry = null;
    [SerializeField]
    Text txtState = null;
    [SerializeField]
    Text txtCity = null;

    [Space]
    [Title("Values")]
    [SerializeField]
    ValueList vllCountry = null;
    [SerializeField]
    ValueList vllState = null;
    //[SerializeField]
    //ValueList vllCity = null;

    [Space]
    [Title("Panel")]
    [SerializeField]
    PanelController pnlCtr = null;
    [SerializeField]
    Panel pnlDetail = null;

    [Title("Event")]
    [SerializeField]
    ImagesEvent onImagesDisplay = null;
    [SerializeField]
    UnityLongsEvent onDisplayed = null;

    NewsService newsService;

    long postId = -1, newsId = -1;

    private void Awake()
    {
        newsService = GetComponent<NewsService>();
    }

    public void Display(long postId)
    {
        this.postId = postId;

        NewsFull newsFull = StateManager.Instance.GetNewsFullByPostId(postId);
        if (newsFull != null)
        {
            newsId = newsFull.Id;
            Display(newsFull);
            return;
        }

        ScreenDialog.Instance.Display();
        newsService.GetFullByPostId(postId);
    }

    public void ApplyFull(NewsFull newsFull)
    {
        newsId = newsFull.Id;
        StateManager.Instance.AddNewsFull(newsFull);
        StateManager.Instance.AddNewsImages(newsFull.Id, newsFull.Images);
        Display(newsFull);
    }

    private void Display(NewsFull newsFull)
    {       
        if (newsFull == null)
            return;

        // Post
        txtAlias.TextValue = $"Publicado por: <b>@{newsFull.AppUserAlias}</b>";
        txtTitle.TextValue = String.IsNullOrWhiteSpace(newsFull.Title) ? "-" : newsFull.Title;
        txtDateTime.TextValue = newsFull.PublicationDateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
        txtSummary.TextValue = String.IsNullOrWhiteSpace(newsFull.Summary) ? "-" : newsFull.Summary;
        txtDescription.TextValue = String.IsNullOrWhiteSpace(newsFull.Description) ? "-" : newsFull.Description;

        txtCountry.TextValue = newsFull.PostCountryId == -1 ? "-" : vllCountry.FindRecordCellString(newsFull.PostCountryId, "Name");
        txtState.TextValue = newsFull.PostStateId == -1 ? "-" : vllState.FindRecordCellString(newsFull.PostStateId, "Name");
        txtCity.TextValue = "-";

        List<Sprite> images = StateManager.Instance.GetNewsImagesById(newsId);
        
        onImagesDisplay.Invoke(images);
        onDisplayed.Invoke(new long[2] {newsFull.PostId, newsFull.Id});

        pnlCtr.ChangePanel(pnlDetail);

        StateManager.Instance.BoardLoadHide();
    }
}