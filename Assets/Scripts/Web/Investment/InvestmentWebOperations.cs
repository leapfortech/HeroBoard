using System;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.authorizations;

using Leap.Data.Web;

// GET
[HttpGET]
[HttpPathExt(WebServiceType.Main, "/investment/FullsByStatus")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class FullsByStatusGetOperation : HttpOperation
{
    [HttpQueryString]
    public int status;

    [HttpResponseJsonBody]
    public InvestmentResponse investmentResponse;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/investment/FullsByAppUserId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class FullsByAppUserIdGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpResponseJsonBody]
    public InvestmentResponse investmentResponse;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/investment/FractionatedFullsByStatus")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class FractionatedFullsByStatusGetOperation : HttpOperation
{
    [HttpQueryString]
    public int status;

    [HttpResponseJsonBody]
    public InvestmentFractionatedFull[] investmentFractionatedFulls;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/investment/FractionatedFullsByAppUserId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class FractionatedFullsByAppUserIdGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpQueryString]
    public int status;

    [HttpResponseJsonBody]
    public InvestmentFractionatedFull[] investmentIntallmentFulls;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/investment/FinancedFullsByStatus")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class FinancedFullsByStatusGetOperation : HttpOperation
{
    [HttpQueryString]
    public int status;

    [HttpResponseJsonBody]
    public InvestmentFinancedFull[] investmentFinancedFulls;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/investment/FinancedFullsByAppUserId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class FinancedFullsByAppUserIdGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpQueryString]
    public int status;

    [HttpResponseJsonBody]
    public InvestmentFinancedFull[] investmentFinancedFulls;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/investment/PrepaidFullsByStatus")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class PrepaidFullsByStatusGetOperation : HttpOperation
{
    [HttpQueryString]
    public int status;

    [HttpResponseJsonBody]
    public InvestmentPrepaidFull[] investmentPrepaidFulls;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/investment/PrepaidFullsByAppUserId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class PrepaidFullsByAppUserIdGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpQueryString]
    public int status;

    [HttpResponseJsonBody]
    public InvestmentPrepaidFull[] investmentPrepaidFulls;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/investment/BankPaymentFullsByStatus")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class BankPaymentFullsStatusGetOperation : HttpOperation
{
    [HttpQueryString]
    public int status;

    [HttpResponseJsonBody]
    public InvestmentPaymentBankFull[] bankPaymentFulls;
}

// REGISTER
[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/investment/Register")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class RegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public Investment investment;

    [HttpResponseTextBody]
    public String id;
}

// DOCS
[HttpGET]
[HttpPathExt(WebServiceType.Main, "/investment/DocInfosByStatus")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class DocInfosByStatusGetOperation : HttpOperation
{
    [HttpQueryString]
    public int status;

    [HttpResponseJsonBody]
    public InvestmentDocInfo[] investmentDocInfos;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/investment/RegisterDocRtu")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class DocRtuRegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public InvestmentDocRequest investmentDocRequest;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/investment/RegisterDocIncome")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class DocIncomeRegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public InvestmentDocRequest investmentDocRequest;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/investment/RegisterDocBank")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class DocBankRegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public InvestmentDocRequest investmentDocRequest;
}

// REFERENCES

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/investment/RegisterReference")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ReferencesRegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public InvestmentReference[] investmentReferences;

    [HttpResponseJsonBody]
    public int[] ids;
}

// SIGNATORY

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/investment/RegisterSignatory")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class SignatoryRegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public InvestmentIdentityRequest[] investmentIdentityRequests;

    [HttpResponseJsonBody]
    public int[] ids;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/investment/CreateSignatory")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class SignatoryCreateOperation : HttpOperation
{
    [HttpQueryString]
    public int investmentId;
}

// BENEFICIARY

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/investment/RegisterBeneficiary")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class BeneficiaryRegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public InvestmentIdentityRequest[] investmentIdentityRequests;

    [HttpResponseJsonBody]
    public int[] ids;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/investment/CreateBeneficiary")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class BeneficiaryCreateOperation : HttpOperation
{
    [HttpQueryString]
    public int investmentId;
}

// VALIDATION

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/investment/RequestUpdate")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class RequestUpdateOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public InvestmentBoardResponse boardResponse;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/investment/Authorize")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class AuthorizeOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public InvestmentBoardResponse boardResponse;
}

[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/investment/Reject")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class RejectOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public InvestmentBoardResponse boardResponse;
}

// PAYMENT

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/investment/PaymentBank")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class InvestmentPaymentBankPostOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public InvestmentBankPayment request;

    [HttpResponseJsonBody]
    public InvestmentPayment investmentPayment;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/investment/PaymentCard")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class InvestmentPaymentCardPostOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public InvestmentCardPayment request;

    [HttpResponseJsonBody]
    public InvestmentPayment investmentPayment;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/investment/PaymentAuthorize")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class PaymentAuthorizePostOperation : HttpOperation
{
    [HttpQueryString]
    public int boardUserId;

    [HttpQueryString]
    public int paymentId;
}

[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/investment/PaymentReject")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class PaymentRejectPostOperation : HttpOperation
{
    [HttpQueryString]
    public int boardUserId;

    [HttpQueryString]
    public int paymentId;

    [HttpQueryString]
    public int receipt;
}

//[HttpPOST]
//[HttpPathExt(WebServiceType.Main, "/investment/PaymentAcknowledge")]
//[HttpProvider(typeof(HttpUnityWebAzureClient))]
//[HttpAccept("application/json")]
//[HttpFirebaseAuthorization]
//public class PaymentAcknowledgeOperation : HttpOperation
//{
//    [HttpQueryString]
//    public int investmentPaymentId;

//    [HttpResponseJsonBody]
//    public InvestmentPayment investmentPayment;
//}