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


public class HappeningDisplayAction : MonoBehaviour
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

    HappeningService happeningService;

    long postId = -1, happeningId = -1;

    private void Awake()
    {
        happeningService = GetComponent<HappeningService>();
    }

    public void Display(long postId)
    {
        this.postId = postId;

        HappeningFull happeningFull = StateManager.Instance.GetHappeningFullByPostId(postId);
        if (happeningFull != null)
        {
            happeningId = happeningFull.Id;
            Display(happeningFull);
            return;
        }

        ScreenDialog.Instance.Display();
        happeningService.GetFullByPostId(postId);
    }

    public void ApplyFull(HappeningFull happeningFull)
    {
        happeningId = happeningFull.Id;
        StateManager.Instance.AddHappeningFull(happeningFull);
        StateManager.Instance.AddHappeningImages(happeningFull.Id, happeningFull.Images);
        Display(happeningFull);
    }

    private void Display(HappeningFull happeningFull)
    {       
        if (happeningFull == null)
            return;

        // Post
        txtAlias.TextValue = $"Publicado por: <b>@{happeningFull.AppUserAlias}</b>";
        txtTitle.TextValue = String.IsNullOrWhiteSpace(happeningFull.Title) ? "-" : happeningFull.Title;
        txtDateTime.TextValue = happeningFull.PublicationDateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
        txtSummary.TextValue = String.IsNullOrWhiteSpace(happeningFull.Summary) ? "-" : happeningFull.Summary;
        txtDescription.TextValue = String.IsNullOrWhiteSpace(happeningFull.Description) ? "-" : happeningFull.Description;

        // Happening
        txtCountry.TextValue = happeningFull.CountryId == -1 ? "-" : vllCountry.FindRecordCellString(happeningFull.CountryId, "Name");
        txtState.TextValue = happeningFull.StateId == -1 ? "-" : vllState.FindRecordCellString(happeningFull.StateId, "Name");
        txtCity.TextValue = "-";

        List<Sprite> images = StateManager.Instance.GetHappeningImagesById(happeningId);
        
        onImagesDisplay.Invoke(images);
        onDisplayed.Invoke(new long[2] {happeningFull.PostId, happeningFull.Id});

        pnlCtr.ChangePanel(pnlDetail);

        StateManager.Instance.BoardLoadHide();
    }
}