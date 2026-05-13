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
    Text txtHappeningType = null;
    [SerializeField]
    Text txtCountry = null;
    [SerializeField]
    Text txtState = null;
    [SerializeField]
    Text txtCity = null;
    [SerializeField]
    Text txtIsPublic = null;
    [SerializeField]
    Text txtHasSignup = null;
    [SerializeField]
    Text txtHasPayment = null;
    [SerializeField]
    Text txtPaymentDetails = null;
    [SerializeField]
    Text txtStartDateTime = null;
    [SerializeField]
    Text txtEndDateTime = null;
    [SerializeField]
    Text txtLocation = null;

    [Space]
    [Title("Values")]
    [SerializeField]
    ValueList vllCountry = null;
    [SerializeField]
    ValueList vllState = null;
    //[SerializeField]
    //ValueList vllCity = null;
    [SerializeField]
    ValueList vllHappeningType = null;

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
        txtHappeningType.TextValue = happeningFull.HappeningTypeId == -1 ? "-" : vllHappeningType.FindRecordCellString(happeningFull.HappeningTypeId, "Name");
        txtCountry.TextValue = happeningFull.CountryId == -1 ? "-" : vllCountry.FindRecordCellString(happeningFull.CountryId, "Name");
        txtState.TextValue = happeningFull.StateId == -1 ? "-" : vllState.FindRecordCellString(happeningFull.StateId, "Name");
        txtCity.TextValue = "-";
        txtIsPublic.TextValue = happeningFull.IsPublic == -1 ? "-" : happeningFull.IsPublic == 0 ? "No" : "Sí";
        txtHasSignup.TextValue = happeningFull.HasSignup == -1 ? "-" : happeningFull.HasSignup == 0 ? "No" : "Sí";
        txtHasPayment.TextValue = happeningFull.HasPayment == -1 ? "-" : happeningFull.HasPayment == 0 ? "No" : "Sí";
        txtPaymentDetails.TextValue = String.IsNullOrWhiteSpace(happeningFull.PaymentDetails) ? "-" : happeningFull.PaymentDetails;
        txtStartDateTime.TextValue = happeningFull.StartDateTime == null ? "-" : happeningFull.StartDateTime.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
        txtEndDateTime.TextValue = happeningFull.EndDateTime == null ? "-" : happeningFull.EndDateTime.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
        txtLocation.TextValue = String.IsNullOrWhiteSpace(happeningFull.Location) ? "-" : happeningFull.Location;

        // Images
        List <Sprite> images = StateManager.Instance.GetHappeningImagesById(happeningId);
        onImagesDisplay.Invoke(images);
        onDisplayed.Invoke(new long[2] {happeningFull.PostId, happeningFull.Id});

        pnlCtr.ChangePanel(pnlDetail);
        StateManager.Instance.BoardLoadHide();
    }
}