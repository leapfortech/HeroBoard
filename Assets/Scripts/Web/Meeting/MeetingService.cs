using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class MeetingService : MonoBehaviour
{
    [Serializable]
    public class MeetingsEvent : UnityEvent<Meeting[]> { }
    [Serializable]
    public class MeetingInfosEvent : UnityEvent<List<MeetingInfo>> { }

    [SerializeField]
    private MeetingsEvent onRetreived = null;

    [SerializeField]
    private MeetingInfosEvent onInfosRetreived = null;

    [SerializeField]
    private UnityIntEvent onRegistered = null;

    [SerializeField]
    private UnityEvent onUpdated = null;

    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;


    // GET
    public void GetByDates(DateTime startDateTime, DateTime endDateTime)
    {
        MeetingGetOperation meetingGetOp = new MeetingGetOperation();
        try
        {
            meetingGetOp.startDateTime = startDateTime;
            meetingGetOp.endDateTime = endDateTime;
            meetingGetOp["on-complete"] = (Action<MeetingGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRetreived.Invoke(op.meetings);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            meetingGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetInfos()
    {
        MeetingInfosGetOperation meetingInfosGetOp = new MeetingInfosGetOperation();
        try
        {
            meetingInfosGetOp["on-complete"] = (Action<MeetingInfosGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onInfosRetreived.Invoke(op.meetingInfos);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            meetingInfosGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetInfosByDates(DateTime startDateTime, DateTime endDateTime)
    {
        MeetingInfosByDatesGetOperation meetingInfosByDatesGetOp = new MeetingInfosByDatesGetOperation();
        try
        {
            meetingInfosByDatesGetOp.startDateTime = startDateTime;
            meetingInfosByDatesGetOp.endDateTime = endDateTime;
            meetingInfosByDatesGetOp["on-complete"] = (Action<MeetingInfosByDatesGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onInfosRetreived.Invoke(op.meetingInfos);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            meetingInfosByDatesGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // REGISTER
    public void Register(Meeting meeting)
    {
        MeetingRegisterOperation meetingRegisterOp = new MeetingRegisterOperation();
        try
        {
            meetingRegisterOp.meeting = meeting;
            meetingRegisterOp["on-complete"] = (Action<MeetingRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt32(op.meetingId));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            meetingRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void RegisterAppointment(Appointment appointment)
    {
        AppointmentRegisterOperation appointmentRegisterOp = new AppointmentRegisterOperation();
        try
        {
            appointmentRegisterOp.appointment = appointment;
            appointmentRegisterOp["on-complete"] = (Action<AppointmentRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt32(op.appointmentId));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            appointmentRegisterOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // UPDATE
    public void UpdateMeeting(Meeting meeting)
    {
        MeetingUpdateOperation meetingUpdateOp = new MeetingUpdateOperation();
        try
        {
            meetingUpdateOp.meeting = meeting;
            meetingUpdateOp["on-complete"] = (Action<MeetingUpdateOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdated.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            meetingUpdateOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
