using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Leap.UI.Elements;
using Leap.Data.Mapper;
using Leap.Data.Collections;
using Leap.UI.Dialog;

using Sirenix.OdinInspector;

public class ProjectInformationChangeAction : MonoBehaviour
{
    [Serializable]
    public class ProjectDescriptionEvent : UnityEvent<List<ProjectInformation>> { }

    [Title("Lists")]
    [SerializeField]
    ListScroller lstInformation = null;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmInformationAdd = null;
    [SerializeField]
    DataMapper dtmInformationUpdate = null;
    [SerializeField]
    ValueList vllProjectDescriptionType = null;

    [Title("Dialogs")]
    [SerializeField]
    GameObject imgInformationAdd = null;
    [SerializeField]
    GameObject imgInformationUpdate = null;

    [Title("Event")]
    [SerializeField]
    private ProjectDescriptionEvent onInformations = null;

    private List<ProjectInformation> informations = new List<ProjectInformation>();

    private int idx = -1;

    public void Clear()
    {
        lstInformation.Clear();
        informations.Clear();

        dtmInformationAdd.ClearElements();
        dtmInformationUpdate.ClearElements();

        imgInformationAdd.SetActive(false);
        imgInformationUpdate.SetActive(false);
    }

    public void GetInformations(int projectId, String title)
    {
        for (int i = 0; i < informations.Count; i++)
            informations[i].ProjectId = projectId;
        onInformations.Invoke(informations);
    }

    public void ApplyInfo(ProjectInfo projectInfo)
    {
        Clear();
        informations = projectInfo.Informations;

        Refresh();
    }

    public void DisplayAdd()
    {
        dtmInformationAdd.ClearElements();

        imgInformationAdd.SetActive(true);
    }

    public void AddInformation()
    {
        if (!dtmInformationAdd.ValidateElements())
            return;

        ProjectInformation projectInformation = dtmInformationAdd.BuildClass<ProjectInformation>();
        projectInformation.Id = -1;

        if (ExistsTypeId(projectInformation.ProjectInformationTypeId))
        {
            ChoiceDialog.Instance.Error("Información", "El tipo de información ya fue agregado.");
            return;
        }

        informations.Add(projectInformation);

        dtmInformationAdd.ClearElements();

        Refresh();
    }

    public void DisplayUpdate(int idx)
    {
        this.idx = idx;

        imgInformationUpdate.SetActive(true);

        dtmInformationUpdate.ClearElements();
        dtmInformationUpdate.PopulateClass(informations[idx]);
    }

    public void UpdateInformation()
    {
        if (!dtmInformationUpdate.ValidateElements())
            return;

        int id = informations[idx].Id;
        informations[idx] = dtmInformationUpdate.BuildClass<ProjectInformation>();
        informations[idx].Id = id;

        dtmInformationUpdate.ClearElements();

        Refresh();
    }

    public void Remove(int idx)
    {
        informations.RemoveAt(idx);

        Refresh();
    }

    public void Refresh()
    {
        lstInformation.Clear();

        for (int i = 0; i < informations.Count; i++)
        {
            ListScrollerValue scrollerValue = new ListScrollerValue(2, true);
            scrollerValue.SetText(0, vllProjectDescriptionType.FindRecordCellString(informations[i].ProjectInformationTypeId, "Name"));
            scrollerValue.SetText(1, informations[i].Information.Length <= 80 ? informations[i].Information : (informations[i].Information[..80] + "..."));

            lstInformation.ApplyAddValue(scrollerValue);
        }

        imgInformationAdd.SetActive(false);
        imgInformationUpdate.SetActive(false);
    }

    private bool ExistsTypeId(int projectDescriptionTypeId)
    {
        for (int i = 0; i < informations.Count; i++)
            if (informations[i].ProjectInformationTypeId == projectDescriptionTypeId)
                return true;
        return false;
    }
}
