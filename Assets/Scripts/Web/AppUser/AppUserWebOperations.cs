using System;
using System.Collections.Generic;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.authorizations;

using Leap.Data.Web;

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/appUser/FullByStatus")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class AppUserFullsGetOperation : HttpOperation
{
    [HttpQueryString]
    public int status;

    [HttpResponseJsonBody]
    public List<AppUserFull> appUserFulls;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/appUser/Named")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class AppUsersGetOperation : HttpOperation
{
    [HttpQueryString]
    public int count = 0;

    [HttpQueryString]
    public int page = 0;

    [HttpResponseJsonBody]
    public AppUserNamed[] appUsersNamed;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/appUser/Count")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class AppUsersCountGetOperation : HttpOperation
{
    [HttpResponseTextBody]
    public String count;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/appUser/NamedByStatus")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class AppUsersByStatusGetOperation : HttpOperation
{
    [HttpQueryString]
    public int status;

    [HttpQueryString]
    public int count = 0;

    [HttpQueryString]
    public int page = 0;

    [HttpResponseJsonBody]
    public AppUserNamed[] appUsersNamed;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/appUser/CountByStatus")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class AppUsersCountByStatusGetOperation : HttpOperation
{
    [HttpQueryString]
    public int status;

    [HttpResponseTextBody]
    public String count;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/appUser/ById")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class AppUserByIdGetOperation : HttpOperation
{
    [HttpQueryString]
    public int id;

    [HttpResponseJsonBody]
    public AppUser appUser;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/appUser")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class AppUserPutOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public AppUser appUser;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/appUser/UpdatePhone")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class AppUserPhonePutOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public PhoneRequest phoneRequest;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/appUser/UpdateStatusId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class AppUserStatusPutOperation : HttpOperation
{
    [HttpQueryString("id")]
    public int appUserId;

    [HttpQueryString("appUserStatusId")]
    public int appUserStatusId;
}
