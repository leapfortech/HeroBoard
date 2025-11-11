using System;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.authorizations;

using Leap.Data.Web;

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/access/LoginBoard")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class AccessLoginBoardPostOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public LoginBoardRequest loginRequest;

    [HttpResponseJsonBody]
    public LoginBoardResponse loginResponse;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/access/RegisterBoard")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class AccessRegisterBoardPostOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public RegisterBoardRequest registerRequest;

    [HttpResponseTextBody]
    public String customerId;
}