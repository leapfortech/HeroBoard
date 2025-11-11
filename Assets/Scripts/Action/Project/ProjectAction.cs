using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

using Leap.Graphics.Tools;
using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.UI.Page;
using Leap.UI.Extensions;
using Leap.Data.Mapper;
using Leap.Data.Collections;

using Sirenix.OdinInspector;

public class ProjectAction : MonoBehaviour
{
    [Title("Buttons")]
    [SerializeField]
    RectTransform rctProjectAdd = null;
    [SerializeField]
    RectTransform rctProjectUpdate = null;

    [Title("Projects")]
    [SerializeField]
    ListScroller lstProjects = null;
    [SerializeField]
    Text txtProjectsEmpty = null;
    [Space]
    [SerializeField]
    DataMapper dtmProjectFull = null;
    [SerializeField]
    DataMapper dtmAddressFull = null;

    [Title("Fields")]
    [SerializeField]
    Text txtTotalValue = null;
    [SerializeField]
    Text txtRentalGrowthRate = null;
    [SerializeField]
    Text txtCapitalGrowthRate = null;
    [SerializeField]
    Text txtManagementCost = null;
    [SerializeField]
    Text txtCpiValue = null;

    [Title("Informations")]
    [SerializeField]
    ComboAdapter cmbProjectInformationType = null;
    [SerializeField]
    Text txtProjectInformation = null;
    [Space]
    [SerializeField]
    ValueList vllProjectInformation = null;
    [SerializeField]
    ValueList vllInformationType = null;

    [Title("Images")]
    [SerializeField]
    ListScroller lstImages = null;
    [SerializeField]
    Text txtImagesEmpty = null;

    [Title("Products")]
    [SerializeField]
    ComboAdapter cmbProjectProductType = null;
    [SerializeField]
    ListScroller lstCpiRanges = null;
    [Space]
    [SerializeField]
    ValueList vllProjectProductType = null;
    [SerializeField]
    ValueList vllProductType = null;

    private readonly CultureInfo cultureInfo = new CultureInfo("en-US");
    private ScrolledText[] sclTexts = null;
    private Vector2 btnAddPos;
    private Vector2 btnUpdatePos;

    public bool Selected { get; set; } = false;

    private ProjectService projectService = null;
    private ProjectFull projectFull = null;

    private void Awake()
    {
        projectService = GetComponent<ProjectService>();
        sclTexts = transform.parent.GetComponentsInChildren<ScrolledText>();
        btnAddPos = rctProjectAdd.anchoredPosition;
        btnUpdatePos = rctProjectUpdate.anchoredPosition;
    }

    public void Clear()
    {
        StateManager.Instance.ProjectProductFulls = new List<ProjectProductFull>();
        dtmProjectFull.ClearElements();
        dtmAddressFull.ClearElements();
    }

    public void GetProjects()
    {
        ScreenDialog.Instance.Display();

        lstProjects.ApplyClearValues();
        txtProjectsEmpty.gameObject.SetActive(false);
        txtImagesEmpty.gameObject.SetActive(false);

        StateManager.Instance.ProjectProductFulls = new List<ProjectProductFull>();
        projectService.GetFulls();
        projectService.GetImages(false);
    }

    public void FillProjects(List<ProjectProductFull> projectProductFulls)
    {
        StateManager.Instance.ProjectProductFulls = projectProductFulls;

        if (StateManager.Instance.ProjectProductFulls.Count == 0)
        {
            lstProjects.ApplyClearValues();
            txtProjectsEmpty.gameObject.SetActive(true);

            rctProjectAdd.anchoredPosition = btnUpdatePos;
            rctProjectUpdate.gameObject.SetActive(false);

            StateManager.Instance.BoardLoadHide();
            return;
        }

        lstProjects.ClearValues();

        ListScrollerValue lstProjectValue;
        for (int i = 0; i < projectProductFulls.Count; i++)
        {
            lstProjectValue = new ListScrollerValue(2, true);
            lstProjectValue.SetSprite(0, projectProductFulls[i].ProjectFull.Sprites[0]);
            lstProjectValue.SetText(1, projectProductFulls[i].ProjectFull.Name);

            lstProjects.AddValue(lstProjectValue);
        }

        lstProjects.ApplyValues();

        rctProjectAdd.anchoredPosition = btnAddPos;
        rctProjectUpdate.gameObject.SetActive(true);
    }

    ProjectImages[] projectImages = null;

    public void FillImages(ProjectImages[] projectImages)
    {
        this.projectImages = projectImages;

        FillImages();
    }

    private void FillImages()
    {
        if (StateManager.Instance.ProjectProductFulls == null || StateManager.Instance.ProjectProductFulls.Count == 0)
        {
            Invoke(nameof(FillImages), 0.2f);
            return;
        }

        for (int i = 0; i < projectImages.Length; i++)
            for (int k = 0; k < projectImages[i].Images.Count; k++)
                StateManager.Instance.ProjectProductFulls[i].ProjectFull.Sprites.Add(projectImages[i].Images[k]?.CreateSprite($"Project{projectImages[i].Id:D02}|{k+1:D02}"));

        projectImages = null;

        Display(0);

        StateManager.Instance.BoardLoadHide();
    }

    public void Display(int idx)
    {
        projectFull = StateManager.Instance.ProjectProductFulls[idx].ProjectFull;
        StateManager.Instance.ProjectIdx = idx;

        dtmProjectFull.PopulateClass(projectFull);
        dtmAddressFull.PopulateClass(projectFull.AddressFull);

        txtTotalValue.TextValue = GetStringAmount(projectFull.CurrencySymbol, projectFull.TotalValue);
        txtRentalGrowthRate.TextValue = GetStringPourcent(projectFull.RentalGrowthRate, 1);
        txtCapitalGrowthRate.TextValue = GetStringPourcent(projectFull.CapitalGrowthRate, 1);
        txtManagementCost.TextValue = GetStringAmount(projectFull.CurrencySymbol, projectFull.ManagementCost);
        txtCpiValue.TextValue = GetStringAmount(projectFull.CurrencySymbol, projectFull.CpiValue);

        vllProjectInformation.ClearRecords();
        for (int i = 0; i < projectFull.Informations.Count; i++)
            vllProjectInformation.AddRecord(projectFull.Informations[i].ProjectInformationTypeId, vllInformationType.FindRecordCellString(projectFull.Informations[i].ProjectInformationTypeId, "Name"), projectFull.Informations[i].Information);
        cmbProjectInformationType.SelectIndex(0);

        vllProjectProductType.ClearRecords();
        for (int i = 0; i < projectFull.CpiRanges.Count; i++)
            if (!vllProjectProductType.HasId(projectFull.CpiRanges[i].ProductTypeId))
                vllProjectProductType.AddRecord(projectFull.CpiRanges[i].ProductTypeId, vllProductType.FindRecordCellString(projectFull.CpiRanges[i].ProductTypeId, "Name"));
        cmbProjectProductType.SelectIndex(0);

        Invoke(nameof(ScaleTexts), 0.1f);

        DisplaySprites(projectFull.Sprites);
    }

    private void ScaleTexts()
    {
        for (int i = 0; i < sclTexts.Length; i++)
            sclTexts[i].Scale();
    }

    private string GetStringAmount(String currencySymbol, double value, int decimals = 0)
    {
        return $"{currencySymbol} {value.ToString($"N{decimals}", cultureInfo)}";
    }

    private string GetStringPourcent(double value, int decimals = 0)
    {
        return $"{(value * 100d).ToString($"N{decimals}", cultureInfo)} %";
    }

    public void DisplayInformation()
    {
        if (cmbProjectInformationType.SelectedIndex == -1)
            return;

        txtProjectInformation.TextValue = cmbProjectInformationType.GetSelectedCellString("Information");
    }

    public void DisplayCpiRanges()
    {
        int productTypeId = cmbProjectProductType.GetSelectedId();

        lstCpiRanges.ClearValues();

        ListScrollerValue lstAppUserValue;
        for (int i = 0; i < projectFull.CpiRanges.Count; i++)
        {
            if (projectFull.CpiRanges[i].ProductTypeId != productTypeId)
                continue;

            CpiRange cpiRange = projectFull.CpiRanges[i];

            lstAppUserValue = new ListScrollerValue(4, false);
            lstAppUserValue.SetText(0, $"{cpiRange.AmountMin}");
            lstAppUserValue.SetText(1, $"{cpiRange.AmountMax}");
            lstAppUserValue.SetText(2, GetStringPourcent(cpiRange.DiscountRate, 2));
            lstAppUserValue.SetText(3, GetStringPourcent(cpiRange.ProfitablityRate, 2));

            lstCpiRanges.AddValue(lstAppUserValue);
        }

        lstCpiRanges.ApplyValues();
    }

    private void DisplaySprites(List<Sprite> sprites)
    {
        if (sprites.Count == 0)
        {
            lstImages.ApplyClearValues();
            txtImagesEmpty.gameObject.SetActive(true);
            ScreenDialog.Instance.Hide();
            return;
        }

        lstImages.ClearValues();

        ListScrollerValue lstImageValue;
        for (int i = 0; i < sprites.Count; i++)
        {
            lstImageValue = new ListScrollerValue(1, false);
            lstImageValue.SetSprite(0, sprites[i]);

            lstImages.AddValue(lstImageValue);
        }

        lstImages.ApplyValues();
    }

    public void ZoomSprite(int idx)
    {
        ZoomDialog.Instance.Display(1, lstImages[idx].GetSprite(0));
    }
}
