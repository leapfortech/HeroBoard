using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.authorizations;

using Leap.Data.Web;

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/alert")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class AlertGetOperation : HttpOperation
{
    [HttpQueryString("alertId")]
    public int alertId;

    [HttpResponseJsonBody]
    public Alert alert;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/alert/GetByCustomerId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class AlertGetByCustomerOperation : HttpOperation
{
    [HttpQueryString("customerId")]
    public int customerId;

    [HttpResponseJsonBody]
    public Alert[] alerts;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/alert")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class AlertPostOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public Alert alert;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/alert")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("text/plain")]
[HttpFirebaseAuthorization]
public class AlertPutOperation : HttpOperation
{
    [HttpQueryString("alertId")]
    public int alertId;
}