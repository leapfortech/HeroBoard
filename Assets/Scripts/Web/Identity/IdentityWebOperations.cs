using System;
using System.Collections.Generic;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.authorizations;

using Leap.Data.Web;

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/identity/All")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class IdentitysGetOperation : HttpOperation
{
    [HttpQueryString]
    public int status = -1;

    [HttpResponseJsonBody]
    public List<Identity> identitys;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/identity/FullAll")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class IdentityFullsGetOperation : HttpOperation
{
    [HttpQueryString]
    public int status = -1;

    [HttpResponseJsonBody]
    public List<IdentityFull> identityFulls;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/identity/ById")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class IdentityGetOperation : HttpOperation
{
    [HttpQueryString]
    public int id;

    [HttpResponseJsonBody]
    public Identity identity;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/identity/ByAppUserId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class IdentityAppUserGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpQueryString]
    public int status;

    [HttpResponseJsonBody]
    public Identity identity;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/identity/InfoByAppUserId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class IdentityInfoAppUserGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpQueryString]
    public int status;

    [HttpResponseJsonBody]
    public IdentityInfo identityInfo;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/identity/BoardInfoByAppUserId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class IdentityBoardInfoAppUserGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpQueryString]
    public int status;

    [HttpResponseJsonBody]
    public IdentityBoardInfo identityBoardInfo;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/identity/PortraitByAppUserId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class PortraitAppUserGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpResponseTextBody]
    public String portrait;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/identity/Register")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class IdentityRegisterPostOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public IdentityRegister identityRegister;

    [HttpResponseTextBody]
    public String id;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/identity")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class IdentityPutOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public Identity identity;

    [HttpResponseTextBody]
    public String id;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/identity/Portrait")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class IdentityPortraitPutOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpRequestTextBody]
    public String portrait;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/identity/Info")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class IdentityInfoPutOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public IdentityInfo identityInfo;

    [HttpResponseTextBody]
    public String id;
}