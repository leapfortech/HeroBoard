using System;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.authorizations;

using Leap.Data.Web;

// Renap

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/renap/IdentityInfo")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class RenapIdentityInfoGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpResponseJsonBody]
    public RenapIdentityInfo renapIdentityInfo;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/renap/IdentityInfoByCui")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class RenapIdentityInfoCuiGetOperation : HttpOperation
{
    [HttpQueryString]
    public String cui;

    [HttpResponseJsonBody]
    public RenapIdentityInfo renapIdentityInfo;
}
