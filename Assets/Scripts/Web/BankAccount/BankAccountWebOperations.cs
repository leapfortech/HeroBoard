using System;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.authorizations;

using Leap.Data.Web;

// GET
[HttpGET]
[HttpPathExt(WebServiceType.Main, "/bankaccount/ByAppUserId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class BankAccountGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpResponseJsonBody]
    public BankAccount bankAccount;
}

// REGISTER
[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/bankaccount/Register")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class BankAccountRegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public BankAccount bankAccount;

    [HttpResponseTextBody]
    public String id;
}

// ADD
[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/bankaccount")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class BankAccountPostOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public BankAccount bankAccount;

    [HttpResponseTextBody]
    public String id;
}


// UPDATE
[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/bankaccount")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class BankAccountUpdateOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public BankAccount bankAccount;

    [HttpResponseTextBody]
    public String id;
}
