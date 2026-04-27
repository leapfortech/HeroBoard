using System;
using System.Collections.Generic;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.authorizations;

using Leap.Data.Web;


// POST
[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/servicewish/AllByType")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class AllByTypePostOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public ServiceWishAllByTypeReq req;

    [HttpResponseJsonBody]
    public ServiceWishAllRsp rsp;
}


// REGISTER
[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/servicewish/register")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ServiceWishRegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public ServiceWish serviceWish;

    [HttpResponseTextBody]
    public String id;
}