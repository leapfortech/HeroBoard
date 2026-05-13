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


public class TreatmentDisplayAction : MonoBehaviour
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
    Text txtIngredients = null;
    [SerializeField]
    Text txtPreparation = null;
    [SerializeField]
    Text txtUsage = null;
    [SerializeField]
    Text txtAnnotation = null;
    [SerializeField]
    ListScroller lstDisease = null;

    [Space]
    [Title("Values")]
    [SerializeField]
    ValueList vllCountry = null;
    [SerializeField]
    ValueList vllState = null;
    //[SerializeField]
    //ValueList vllCity = null;
    [SerializeField]
    ValueList vllDisease = null;

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

    TreatmentService treatmentService;

    long postId = -1, treatmentId = -1;

    private void Awake()
    {
        treatmentService = GetComponent<TreatmentService>();
    }

    public void Display(long postId)
    {
        this.postId = postId;

        TreatmentFull treatmentFull = StateManager.Instance.GetTreatmentFullByPostId(postId);
        if (treatmentFull != null)
        {
            treatmentId = treatmentFull.Id;
            Display(treatmentFull);
            return;
        }

        ScreenDialog.Instance.Display();
        treatmentService.GetFullByPostId(postId);
    }

    public void ApplyFull(TreatmentFull treatmentFull)
    {
        treatmentId = treatmentFull.Id;
        StateManager.Instance.AddTreatmentFull(treatmentFull);
        StateManager.Instance.AddTreatmentImages(treatmentFull.Id, treatmentFull.Images);
        Display(treatmentFull);
    }

    private void Display(TreatmentFull treatmentFull)
    {       
        if (treatmentFull == null)
            return;

        // Post
        txtAlias.TextValue = $"Publicado por: <b>@{treatmentFull.AppUserAlias}</b>";
        txtTitle.TextValue = String.IsNullOrWhiteSpace(treatmentFull.Title) ? "-" : treatmentFull.Title;
        txtDateTime.TextValue = treatmentFull.PublicationDateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
        txtSummary.TextValue = String.IsNullOrWhiteSpace(treatmentFull.Summary) ? "-" : treatmentFull.Summary;
        txtDescription.TextValue = String.IsNullOrWhiteSpace(treatmentFull.Description) ? "-" : treatmentFull.Description;

        txtCountry.TextValue = treatmentFull.PostCountryId == -1 ? "-" : vllCountry.FindRecordCellString(treatmentFull.PostCountryId, "Name");
        txtState.TextValue = treatmentFull.PostStateId == -1 ? "-" : vllState.FindRecordCellString(treatmentFull.PostStateId, "Name");
        txtCity.TextValue = "-";

        // Treatment
        txtIngredients.TextValue = String.IsNullOrWhiteSpace(treatmentFull.Ingredients) ? "-" : treatmentFull.Ingredients;
        txtPreparation.TextValue = String.IsNullOrWhiteSpace(treatmentFull.Preparation) ? "-" : treatmentFull.Preparation;
        txtUsage.TextValue = String.IsNullOrWhiteSpace(treatmentFull.Usage) ? "-" : treatmentFull.Usage;
        txtAnnotation.TextValue = String.IsNullOrWhiteSpace(treatmentFull.Annotation) ? "-" : treatmentFull.Annotation;

        // Disease
        for (int i = 0; i < treatmentFull.DiseaseFulls.Count; i++)
        {
            ListScrollerValue value = new ListScrollerValue(1, true);
            value.SetText(0, vllDisease.FindRecordCellString(treatmentFull.DiseaseFulls[i].DiseaseTypeId, "Name"));

            lstDisease.AddValue(value);
        }

        lstDisease.ApplyValues();

        // Images
        List<Sprite> images = StateManager.Instance.GetTreatmentImagesById(treatmentId);
        onImagesDisplay.Invoke(images);
        onDisplayed.Invoke(new long[2] {treatmentFull.PostId, treatmentFull.Id});

        pnlCtr.ChangePanel(pnlDetail);
        StateManager.Instance.BoardLoadHide();
    }
}