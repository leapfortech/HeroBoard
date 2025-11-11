using System.Collections.Generic;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.authorizations;

using Leap.Data.Web;
using System;

// GET
[HttpGET]
[HttpPathExt(WebServiceType.Main, "/meeting/ByDates")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class MeetingGetOperation : HttpOperation
{
    [HttpQueryString]
    public DateTime startDateTime;
    [HttpQueryString]
    public DateTime endDateTime;

    [HttpResponseJsonBody]
    public Meeting[] meetings;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/meeting/Infos")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class MeetingInfosGetOperation : HttpOperation
{
    [HttpResponseJsonBody]
    public List<MeetingInfo> meetingInfos;
}


[HttpGET]
[HttpPathExt(WebServiceType.Main, "/meeting/FullsByDates")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class MeetingInfosByDatesGetOperation : HttpOperation
{
    [HttpQueryString]
    public DateTime startDateTime;
    [HttpQueryString]
    public DateTime endDateTime;

    [HttpResponseJsonBody]
    public List<MeetingInfo> meetingInfos;
}

// REGISTER
[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/meeting/Register")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class MeetingRegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public Meeting meeting;

    [HttpResponseTextBody]
    public String meetingId;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/meeting/Appointment")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class AppointmentRegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public Appointment appointment;

    [HttpResponseTextBody]
    public String appointmentId;
}

// UPDATE
[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/meeting")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class MeetingUpdateOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public Meeting meeting;
}