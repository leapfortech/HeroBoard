using System;
using System.Collections.Generic;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.authorizations;

using Leap.Data.Web;

// Get

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/onboarding")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class OnboardingGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpResponseJsonBody]
    public Onboarding onboarding;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/onboarding/All")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class OnboardingsGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpResponseJsonBody]
    public List<Onboarding> onboardings;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/onboarding")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class OnboardingPostOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public Onboarding onboarding;

    [HttpResponseTextBody]
    public String onboardingId;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/onboarding")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class OnboardingPutOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public Onboarding onboarding;

    [HttpResponseTextBody]
    public String onboardingId;
}

// Update

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/onboarding/DpiFront")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpFirebaseAuthorization]
public class ObdDpiFrontPutOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpRequestTextBody]
    public String dpiPhotos;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/onboarding/DpiBack")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpFirebaseAuthorization]
public class ObdDpiBackPutOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpRequestTextBody]
    public String dpiBack;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/onboarding/IdentityFull")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class ObdIdentityFullPutOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public IdentityInfo identityFull;

    [HttpResponseTextBody]
    public String id;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/onboarding/Portrait")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpFirebaseAuthorization]
public class ObdPortraitPutOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpRequestTextBody]
    public String portrait;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/onboarding/HouseholdBill")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpFirebaseAuthorization]
public class ObdHouseholdBillPutOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpRequestTextBody]
    public String householdBill;
}

// Authorize

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/onboarding/Authorize")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class OnboardingAuthorizeOperation : HttpOperation
{
    [HttpQueryString]
    public int onboardingId;

    [HttpQueryString]
    public int appUserId;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/onboarding/Reject")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class OnboardingRejectOperation : HttpOperation
{
    [HttpQueryString]
    public int onboardingId;

    [HttpQueryString]
    public int appUserId;
}