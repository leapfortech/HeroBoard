using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Leap.UI.Elements;
using Leap.UI.Page;
using Leap.UI.Dialog;
using Leap.Core.Tools;

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

    private void Awake()
    {
        radioService = GetComponent<RadioService>();
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

        List<Sprite> images = StateManager.Instance.GetRadioImagesById(radioId);
        
        onImagesDisplay.Invoke(images);
        onDisplayed.Invoke(new long[2] {radioFull.PostId, radioFull.Id});

        pnlCtr.ChangePanel(pnlDetail);

        StateManager.Instance.BoardLoadHide();
    }
}