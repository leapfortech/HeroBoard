using System.Collections.Generic;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.authorizations;

using Leap.Data.Web;
using System;

// GET
/*[HttpGET]
[HttpPathExt(WebServiceType.Main, "/product/Fulls")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class FullsGetOperation : HttpOperation
{
    [HttpResponseJsonBody]
    public List<ProjectProductFull> projectProductFulls;
}*/

// REGISTER
[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/product/RegisterFractionated")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ProductFractionatedPostOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public ProductFractionated productFractionated;

    [HttpResponseTextBody]
    public String productId;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/product/RegisterFinanced")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ProductFinancedPostOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public ProductFinanced productFinanced;

    [HttpResponseTextBody]
    public String productId;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/product/RegisterPrepaid")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ProductPrepaidPostOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public ProductPrepaid productPrepaid;

    [HttpResponseTextBody]
    public String productId;
}

// UPDATE
/*[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/product")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class UpdateOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public ProjectRequest projectRequest;
}*/