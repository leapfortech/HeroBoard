using System;
using System.Collections.Generic;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.authorizations;

using Leap.Data.Web;

// GET
[HttpGET]
[HttpPathExt(WebServiceType.Main, "/referred/All")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ReferredsGetOperation : HttpOperation
{
    [HttpResponseJsonBody]
    public List<Referred> referreds;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/referred/FullAll")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ReferredFullsGetOperation : HttpOperation
{
    [HttpResponseJsonBody]
    public List<ReferredFull> referredFulls;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/referred/ByAppUserId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ReferredGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpResponseJsonBody]
    public List<Referred> referreds;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/referred/History")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class HistoryGetOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public ReferredHistoryRequest referredHistoryRequest;

    [HttpResponseJsonBody]
    public List<Referred> referreds;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/referred/IdByCode")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class IdByCodeGetOperation : HttpOperation
{
    [HttpQueryString]
    public String code;

    [HttpResponseTextBody]
    public String response;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/referred/Register")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ReferredRegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public Referred referred;

    [HttpResponseTextBody]
    public String referredIds;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/referred")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class ReferredPutOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public Referred referred;

    [HttpResponseJsonBody]
    public int referredlId;
}
