using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.Data.Collections;

using Sirenix.OdinInspector;

public class MeetingAction : MonoBehaviour
{
    [Serializable]
    public class MeetingEvent : UnityEvent<MeetingInfo> { }

    [Title("Meetings")]
    [SerializeField]
    ListScroller lstMeetings = null;
    [SerializeField]
    Text txtMeetingsEmpty = null;

    [Title("Fields")]
    [SerializeField]
    Text txtSubject = null;
    [SerializeField]
    Text txtDate = null;
    [SerializeField]
    Text txtTime = null;
    [SerializeField]
    Text txtType = null;
    [SerializeField]
    Text txtDescription = null;

    [Title("Appointments")]
    [SerializeField]
    ListScroller lstAppointments = null;
    [SerializeField]
    Text txtAppointmentsEmpty = null;

    [Title("Data")]
    [SerializeField]
    ValueList vllMeetingType = null;

    [Title("Actions")]
    [SerializeField]
    Button btnAdd = null;
    [SerializeField]
    Button btnUpdate = null;

    [Title("Event")]
    [SerializeField]
    MeetingEvent onMeeting = null;

    public bool Selected { get; set; } = false;
    public int Id { get; set; } = -1;
    private Dictionary<int, int> Idx = new Dictionary<int, int>();

    MeetingService meetingService = null;
    List<MeetingInfo> meetingInfos = null;

    MeetingInfo meetingInfo = null;
    public MeetingInfo MeetingInfo => meetingInfo;

    RectTransform trfAdd;
    Vector2 posAdd, posUpdate;

    private void Awake()
    {
        meetingService = GetComponent<MeetingService>();

        trfAdd = btnAdd.GetComponent<RectTransform>();
        posAdd = trfAdd.anchoredPosition;

        posUpdate = btnUpdate.GetComponent<RectTransform>().anchoredPosition;
    }

    public void Clear()
    {
        txtSubject.TextValue = "-";
        txtDate.TextValue = "-";
        txtTime.TextValue = "-";
        txtType.TextValue = "-";
        txtDescription.TextValue = "";

        lstAppointments.ApplyClearValues();
        txtAppointmentsEmpty.gameObject.SetActive(false);
    }

    public void GetMeetings()
    {
        ScreenDialog.Instance.Display();

        lstMeetings.ApplyClearValues();
        txtMeetingsEmpty.gameObject.SetActive(false);

        meetingInfo = null;
        meetingService.GetInfos();
    }

    public void FillMeetings(List<MeetingInfo> meetings)
    {
        meetingInfos = meetings;
        Idx.Clear();

        if (meetings.Count == 0)
        {
            lstMeetings.ApplyClearValues();
            txtMeetingsEmpty.gameObject.SetActive(true);

            trfAdd.anchoredPosition = posUpdate;
            btnUpdate.gameObject.SetActive(false);

            StateManager.Instance.BoardLoadHide();
            return;
        }

        lstMeetings.ClearValues();

        ListScrollerValue lstMeetingValue;
        for (int idx = 0; idx < meetings.Count; idx++)
        {
            Idx[meetings[idx].Id] = idx;
            meetings[idx].StartDateTime = meetings[idx].StartDateTime.ToLocalTime();
            meetings[idx].EndDateTime = meetings[idx].EndDateTime.ToLocalTime();

            lstMeetingValue = new ListScrollerValue(4, true);
            lstMeetingValue.SetText(0, $"{meetings[idx].StartDateTime:dd/MM/yyyy}");
            lstMeetingValue.SetText(1, $"{meetings[idx].StartDateTime:HH:mm} - {meetings[idx].EndDateTime:HH:mm}");
            lstMeetingValue.SetText(2, vllMeetingType.FindRecordCellString(meetings[idx].MeetingTypeId, 0));
            lstMeetingValue.SetText(3, meetings[idx].Subject);

            lstMeetings.AddValue(lstMeetingValue);
        }

        lstMeetings.ApplyValues();

        trfAdd.anchoredPosition = posAdd;
        btnUpdate.gameObject.SetActive(true);
        lstMeetings.CheckToggle(Id == -1 ? 0 : Idx[Id], true);

        StateManager.Instance.BoardLoadHide();
    }

    public void Display(int idx)
    {
        meetingInfo = meetingInfos[idx];
        Id = meetingInfo.Id;

        txtSubject.TextValue = meetingInfo.Subject;
        txtDate.TextValue = $"{meetingInfo.StartDateTime:dd/MM/yyyy}";
        txtTime.TextValue = $"{meetingInfo.StartDateTime:HH:mm} - {meetingInfo.EndDateTime:HH:mm}";
        txtType.TextValue = vllMeetingType.FindRecordCellString(meetingInfo.MeetingTypeId, 0);
        txtDescription.TextValue = meetingInfo.Description;

        if (meetingInfo.Appointments.Count == 0)
        {
            lstAppointments.ApplyClearValues();
            txtAppointmentsEmpty.gameObject.SetActive(true);

            onMeeting.Invoke(meetingInfo);
            return;
        }

        lstAppointments.ClearValues();

        ListScrollerValue lstAppointmentsValue;
        for (int i = 0; i < meetingInfo.Appointments.Count; i++)
        {
            lstAppointmentsValue = new ListScrollerValue(1, true);
            lstAppointmentsValue.SetText(0, meetingInfo.Appointments[i]);

            lstAppointments.AddValue(lstAppointmentsValue);
        }

        lstAppointments.ApplyValues();
        txtAppointmentsEmpty.gameObject.SetActive(false);

        onMeeting.Invoke(meetingInfo);
    }
}
