using System;
using System.Collections.Generic;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.authorizations;

using Leap.Data.Web;

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/boardUser/FullsByStatus")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class BoardUserFullsByStatusGetOperation : HttpOperation
{
    [HttpQueryString]
    public int status;

    [HttpResponseJsonBody]
    public BoardUserFull[] boardUserFulls;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/boardUser/ById")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class BoardUserByIdGetOperation : HttpOperation
{
    [HttpQueryString]
    public long id;

    [HttpResponseJsonBody]
    public BoardUser boardUser;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/boardUser/Count")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class BoardUsersCountGetOperation : HttpOperation
{
    [HttpResponseTextBody]
    public String count;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/boardUser/CountByStatus")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class BoardUsersCountByStatusGetOperation : HttpOperation
{
    [HttpQueryString]
    public int status;

    [HttpResponseTextBody]
    public String count;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/boardUser/FullAllByName")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class BoardUserFullAllByNamePostOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public BoardUserAllByNameReq req;

    [HttpResponseJsonBody]
    public BoardUserFullAllRsp rsp;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/boardUser/Full")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class BoardUserFullPutOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public BoardUserFull boardUserFull;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/boardUser")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class BoardUserPutOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public BoardUser boardUser;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/appUser/UpdateStatus")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class BoardUserStatusPutOperation : HttpOperation
{
    [HttpQueryString("id")]
    public long boardUserId;

    [HttpQueryString("appUserStatusId")]
    public int boardUserStatusId;
}
