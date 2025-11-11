using UnityEngine;

using Leap.Data.Mapper;
using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.UI.Extensions;

using Sirenix.OdinInspector;

public class MeetingChangeAction : MonoBehaviour
{
    [Title("Fields")]
    [SerializeField]
    Text lblMeeting = null;
    [SerializeField]
    ComboAdapter cmbTimeStart = null;
    [SerializeField]
    ComboAdapter cmbTimeEnd = null;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmMeetingChange = null;

    [Title("Actions")]
    [SerializeField]
    Button btnChange = null;

    public bool Selected { get; set; } = false;

    MeetingService meetingService = null;
    Meeting meeting = null;

    public void SetMeeting(MeetingInfo meetingInfo)
    {
        meeting = meetingInfo == null ? null : new Meeting(meetingInfo);
    }

    private void Awake()
    {
        meetingService = GetComponent<MeetingService>();
    }

    public void Clear()
    {
        dtmMeetingChange.ClearElements();
        cmbTimeStart.Clear();
        cmbTimeEnd.Clear();
    }

    public void ChangeMeeting()
    {
        if (btnChange.Title[0] == 'A')
            RegisterMeeting();
        else
            UpdateMeeting();
    }

    // Add

    public void DisplayAdd()
    {
        lblMeeting.TextValue = "Agregar un evento";
        btnChange.Title = "Agregar";
        Clear();
    }

    private void RegisterMeeting()
    {
        if (!dtmMeetingChange.ValidateElements())
            return;

        int startHours = cmbTimeStart.GetSelectedId(0);
        int startMinutes = cmbTimeStart.GetSelectedId(1);

        int endHours = cmbTimeEnd.GetSelectedId(0);
        int endMinutes = cmbTimeEnd.GetSelectedId(1);

        if (startHours > endHours || (startHours == endHours && startMinutes >= endMinutes))
        {
            ChoiceDialog.Instance.Error("Nuevo evento", "La hora de inicio de la reunión tiene que ser anterior la hora final.");
            return;
        }

        ScreenDialog.Instance.Display();

        meeting = dtmMeetingChange.BuildClass<Meeting>();

        meeting.Id = -1;
        meeting.BoardUserId = StateManager.Instance.BoardUser.Id;

        meeting.StartDateTime = meeting.StartDateTime.AddHours(startHours).AddMinutes(startMinutes).ToUniversalTime();
        meeting.EndDateTime = meeting.EndDateTime.AddHours(endHours).AddMinutes(endMinutes).ToUniversalTime();

        meetingService.Register(meeting);
    }

    // Update

    public void DisplayUpdate()
    {
        lblMeeting.TextValue = "Editar un evento";
        btnChange.Title = "Guardar";

        dtmMeetingChange.PopulateClass(meeting);
        cmbTimeStart.Select(meeting.StartDateTime.Hour, meeting.StartDateTime.Minute);
        cmbTimeEnd.Select(meeting.EndDateTime.Hour, meeting.EndDateTime.Minute);
    }

    private void UpdateMeeting()
    {
        if (!dtmMeetingChange.ValidateElements())
            return;

        int startHours = cmbTimeStart.GetSelectedId(0);
        int startMinutes = cmbTimeStart.GetSelectedId(1);

        int endHours = cmbTimeEnd.GetSelectedId(0);
        int endMinutes = cmbTimeEnd.GetSelectedId(1);

        if (startHours > endHours || (startHours == endHours && startMinutes >= endMinutes))
        {
            ChoiceDialog.Instance.Error("Editar un evento", "La hora de inicio de la reunión tiene que ser anterior la hora final.");
            return;
        }

        ScreenDialog.Instance.Display();

        Meeting updMeeting = dtmMeetingChange.BuildClass<Meeting>();

        updMeeting.Id = meeting.Id;
        updMeeting.BoardUserId = StateManager.Instance.BoardUser.Id;

        updMeeting.StartDateTime = updMeeting.StartDateTime.AddHours(startHours).AddMinutes(startMinutes).ToUniversalTime();
        updMeeting.EndDateTime = updMeeting.EndDateTime.AddHours(endHours).AddMinutes(endMinutes).ToUniversalTime();

        meetingService.UpdateMeeting(updMeeting);
    }
}