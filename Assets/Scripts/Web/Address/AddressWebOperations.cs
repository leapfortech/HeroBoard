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
[HttpPathExt(WebServiceType.Main, "/address/ByAppUserId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class AddressGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpResponseJsonBody]
    public Address address;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/address/HouseholdBills")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class HouseholdBillGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpResponseTextBody]
    public String[] householdBills;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/address/InfoByAppUserId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class AddressInfoGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpQueryString]
    public int status = 1;

    [HttpResponseJsonBody]
    public AddressInfo addressInfo;
}

// REGISTER
[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/address/Register")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class AddressRegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public AddressInfo addressInfo;

    [HttpResponseTextBody]
    public String id;
}

// ADD
[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/address")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class AddressPostOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public Address address;

    [HttpResponseTextBody]
    public String id;
}


// UPDATE
[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/address")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class AddressPutOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public Address address;

    [HttpResponseTextBody]
    public String id;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/address/ByAppUser")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class AddressAppUserPutOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpRequestJsonBody]
    public Address address;

    [HttpResponseTextBody]
    public String id;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/address/Info")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class AddressInfoPutOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public AddressInfo addressInfo;

    [HttpResponseTextBody]
    public String id;
}
