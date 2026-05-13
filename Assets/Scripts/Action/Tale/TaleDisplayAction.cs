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


public class TaleDisplayAction : MonoBehaviour
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
    Text txtPlace = null;
    [SerializeField]
    Text txtSummary = null;
    [SerializeField]
    Text txtDescription = null;

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

    TaleService taleService;

    long postId = -1, taleId = -1;

    private void Awake()
    {
        taleService = GetComponent<TaleService>();
    }

    public void Display(long postId)
    {
        this.postId = postId;

        TaleFull taleFull = StateManager.Instance.GetTaleFullByPostId(postId);
        if (taleFull != null)
        {
            taleId = taleFull.Id;
            Display(taleFull);
            return;
        }

        ScreenDialog.Instance.Display();
        taleService.GetFullByPostId(postId);
    }

    public void ApplyFull(TaleFull taleFull)
    {
        taleId = taleFull.Id;
        StateManager.Instance.AddTaleFull(taleFull);
        StateManager.Instance.AddTaleImages(taleFull.Id, taleFull.Images);
        Display(taleFull);
    }

    private void Display(TaleFull taleFull)
    {       
        if (taleFull == null)
            return;

        // Post
        txtAlias.TextValue = $"Publicado por: <b>@{taleFull.AppUserAlias}</b>";
        txtTitle.TextValue = String.IsNullOrWhiteSpace(taleFull.Title) ? "-" : taleFull.Title;

        String country = taleFull.PostCountryId == -1 ? "" : vllCountry.FindRecordCellString(taleFull.PostCountryId, "Name");
        String state = taleFull.PostStateId == -1 ? "" : vllState.FindRecordCellString(taleFull.PostStateId, "Name");
        txtPlace.TextValue = country + (!String.IsNullOrWhiteSpace(country) && !String.IsNullOrWhiteSpace(state) ? ", " : "") + state;

        txtDateTime.TextValue = taleFull.PublicationDateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
        txtSummary.TextValue = String.IsNullOrWhiteSpace(taleFull.Summary) ? "-" : taleFull.Summary;
        txtDescription.TextValue = String.IsNullOrWhiteSpace(taleFull.Description) ? "-" : taleFull.Description;

        // Images
        List<Sprite> images = StateManager.Instance.GetTaleImagesById(taleId);
        onImagesDisplay.Invoke(images);
        onDisplayed.Invoke(new long[2] {taleFull.PostId, taleFull.Id});

        pnlCtr.ChangePanel(pnlDetail);
        StateManager.Instance.BoardLoadHide();
    }
}