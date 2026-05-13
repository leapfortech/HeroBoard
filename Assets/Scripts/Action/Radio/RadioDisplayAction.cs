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


public class RadioDisplayAction : MonoBehaviour
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

    [SerializeField]
    ListScroller lstRadioType = null;
    [SerializeField]
    ListScroller lstRadioLanguage = null;

    [SerializeField]
    Button btnRadio = null;

    [Space]
    [Title("Values")]
    [SerializeField]
    ValueList vllCountry = null;
    [SerializeField]
    ValueList vllState = null;
    //[SerializeField]
    //ValueList vllCity = null;
    [SerializeField]
    ValueList vllRadioType = null;
    [SerializeField]
    ValueList vllRadioLanguage = null;

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

    RadioService radioService;

    long postId = -1, radioId = -1;
    String url = null;

    private void Awake()
    {
        radioService = GetComponent<RadioService>();
    }

    private void Start()
    {
        btnRadio?.AddAction(OpenRadio);
    }

    private void OpenRadio()
    {
        Application.OpenURL(url);
    }

    public void Display(long postId)
    {
        this.postId = postId;

        RadioFull radioFull = StateManager.Instance.GetRadioFullByPostId(postId);
        if (radioFull != null)
        {
            radioId = radioFull.Id;
            Display(radioFull);
            return;
        }

        ScreenDialog.Instance.Display();
        radioService.GetFullByPostId(postId);
    }

    public void ApplyFull(RadioFull radioFull)
    {
        radioId = radioFull.Id;
        url = radioFull.LinkFulls[0].Url;

        StateManager.Instance.AddRadioFull(radioFull);
        StateManager.Instance.AddRadioImages(radioFull.Id, radioFull.Images);
        Display(radioFull);
    }

    private void Display(RadioFull radioFull)
    {       
        if (radioFull == null)
            return;

        // Post
        txtAlias.TextValue = $"Publicado por: <b>@{radioFull.AppUserAlias}</b>";
        txtTitle.TextValue = String.IsNullOrWhiteSpace(radioFull.Title) ? "-" : radioFull.Title;
        txtDateTime.TextValue = radioFull.PublicationDateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
        txtSummary.TextValue = String.IsNullOrWhiteSpace(radioFull.Summary) ? "-" : radioFull.Summary;
        txtDescription.TextValue = String.IsNullOrWhiteSpace(radioFull.Description) ? "-" : radioFull.Description;

        txtCountry.TextValue = radioFull.PostCountryId == -1 ? "-" : vllCountry.FindRecordCellString(radioFull.PostCountryId, "Name");
        txtState.TextValue = radioFull.PostStateId == -1 ? "-" : vllState.FindRecordCellString(radioFull.PostStateId, "Name");
        txtCity.TextValue = "-";

        // Radio Type
        for (int i = 0; i < radioFull.RadioTypeFulls.Count; i++)
        {
            ListScrollerValue value = new ListScrollerValue(1, true);
            value.SetText(0, vllRadioType.FindRecordCellString(radioFull.RadioTypeFulls[i].RadioTypeId, "Name"));

            lstRadioType.AddValue(value);
        }

        // Radio Language
        for (int i = 0; i < radioFull.RadioLanguageFulls.Count; i++)
        {
            ListScrollerValue value = new ListScrollerValue(1, true);
            value.SetText(0, vllRadioLanguage.FindRecordCellString(radioFull.RadioLanguageFulls[i].LanguageId, "Name"));

            lstRadioLanguage.AddValue(value);
        }

        // Images
        List<Sprite> images = StateManager.Instance.GetRadioImagesById(radioId);
        onImagesDisplay.Invoke(images);
        onDisplayed.Invoke(new long[2] {radioFull.PostId, radioFull.Id});

        pnlCtr.ChangePanel(pnlDetail);
        StateManager.Instance.BoardLoadHide();
    }
}