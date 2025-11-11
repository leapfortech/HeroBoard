using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Leap.Core.Tools;
using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.Data.Collections;
using Leap.Data.Mapper;

using Sirenix.OdinInspector;

public class ProjectChangeAction : MonoBehaviour
{
    [Serializable]
    public class ProjectBuildEvent : UnityEvent<int, String> { }

    [Title("Field")]
    [SerializeField]
    InputField ifdAddressId = null;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmDescriptionChange = null;
    [SerializeField]
    DataMapper dtmAddressChange = null;

    [Title("Action")]
    [SerializeField]
    Button btnProjectRegister = null;
    [SerializeField]
    Button btnProjectUpdate = null;

    [Title("Events")]
    [SerializeField]
    ProjectBuildEvent onBuild;
    [SerializeField]
    UnityEvent onRegistered = null;
    [SerializeField]
    UnityEvent onUpdated = null;

    ProjectService projectService = null;

    int productCount = 0;
    ProductFractionated productFractionated = null;
    ProductFinanced productFinanced = null;
    ProductPrepaid productPrepaid = null;

    List<CpiRange> cpiRanges = null;
    List<ProjectInformation> informations = null;
    List<String> images = null;

    private void Awake()
    {
        projectService = GetComponent<ProjectService>();
    }

    private void Start()
    {
        btnProjectRegister.AddAction(RegisterProject);
        btnProjectUpdate.AddAction(UpdateProject);
    }

    public void DisplayProjectAdd()
    {
        btnProjectRegister.gameObject.SetActive(true);
        btnProjectUpdate.gameObject.SetActive(false);
        Clear();
        ifdAddressId.Text = "-1";
        ifdAddressId.InputValidate();
    }

    public void DisplayProjectUpdate()
    {
        btnProjectRegister.gameObject.SetActive(false);
        btnProjectUpdate.gameObject.SetActive(true);
        Clear();
        GetInfo();
    }

    public void Clear()
    {
        dtmDescriptionChange.ClearElements();
        dtmAddressChange.ClearElements();
    }

    // Apply
    public void ApplyProductCount(int productCount) => this.productCount = productCount;
    public void ApplyProductFractionated(ProductFractionated productFractionated) => this.productFractionated = productFractionated;
    public void ApplyProductFinanced(ProductFinanced productFinanced) => this.productFinanced = productFinanced;
    public void ApplyProductPrepaid(ProductPrepaid productPrepaid) => this.productPrepaid = productPrepaid;
    public void ApplyCpiRanges(List<CpiRange> cpiRanges) => this.cpiRanges = cpiRanges;
    public void ApplyInformations(List<ProjectInformation> informations) => this.informations = informations;
    public void ApplyImages(List<String> images) => this.images = images;

    // Register
    private void RegisterProject()
    {
        ProjectInfo projectInfo = BuildInfo(-1, "Agregar un proyecto");
        if (projectInfo == null)
            return;

        projectService.Register(projectInfo);
    }

    public void ProjectRegistered()
    {
        ChoiceDialog.Instance.Info("Agregar un proyecto", "Proyecto agregado exitosamente.", () => onRegistered.Invoke());
    }

    // Update
    public void GetInfo()
    {
        ScreenDialog.Instance.Display();
        projectService.GetInfo(StateManager.Instance.ProjectFull.ProjectId, false);
    }

    public void ApplyInfo(ProjectInfo projectInfo)
    {
        dtmDescriptionChange.PopulateClass(projectInfo.Project);
        dtmAddressChange.PopulateClass(projectInfo.Address);
        ScreenDialog.Instance.Hide();
    }

    private void UpdateProject()
    {
        ProjectInfo projectInfo = BuildInfo(StateManager.Instance.ProjectFull.ProjectId, "Editar el proyecto");
        if (projectInfo == null)
            return;

        projectInfo.Project.Id = StateManager.Instance.ProjectFull.ProjectId;
        projectInfo.Project.Code = StateManager.Instance.ProjectFull.Code;
        projectInfo.Project.CpiCount = StateManager.Instance.ProjectFull.CpiCount;
        projectInfo.Project.CurrencyId = 47;
        projectInfo.Project.Status = StateManager.Instance.ProjectFull.Status;

        projectService.UpdateProject(projectInfo);
    }

    public void ProjectUpdated()
    {
        ChoiceDialog.Instance.Info("Editar el proyecto", "Proyecto guardado exitosamente.", () => onUpdated.Invoke());
    }

    // Build
    private ProjectInfo BuildInfo(int projectId, String title)
    {
        if (!dtmDescriptionChange.ValidateElements())
            return null;
        if (!dtmAddressChange.ValidateElements())
            return null;

        productFractionated = null;
        productFinanced = null;
        productPrepaid = null;

        cpiRanges = null;
        informations = null;
        images = null;

        onBuild.Invoke(projectId, title);

        if (informations.Count == 0)
        {
            ChoiceDialog.Instance.Error(title, "Se necesita por lo menos una información del proyecto.");
            return null;
        }

        if (images.Count == 0)
        {
            ChoiceDialog.Instance.Error(title, "Se necesita por lo menos una imagen del proyecto.");
            return null;
        }

        if (productCount == 0)
        {
            ChoiceDialog.Instance.Error(title, "Se necesita por lo menos un producto en el proyecto.");
            return null;
        }

        if (cpiRanges == null)
            return null;

        ScreenDialog.Instance.Display();

        ProjectInfo projectInfo = new ProjectInfo
        {
            Project = dtmDescriptionChange.BuildClass<Project>(),
            Address = dtmAddressChange.BuildClass<Address>(),

            ProductFractionated = productFractionated,
            ProductFinanced = productFinanced,
            ProductPrepaid = productPrepaid,

            CpiRanges = cpiRanges,
            Informations = informations,
            Images = images
        };

        projectInfo.Project.Id = projectId;
        projectInfo.Project.CurrencyId = 47;
        projectInfo.Project.CpiCount = 0;
        projectInfo.Project.RentalGrowthRate /= 100d;
        projectInfo.Project.CapitalGrowthRate /= 100d;
        projectInfo.Project.ImageCount = projectInfo.Images.Count;

        return projectInfo;
    }
}
