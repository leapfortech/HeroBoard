using System.Collections.Generic;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.authorizations;

using Leap.Data.Web;
using System;

// GET

// PROJECTS

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/project/Fulls")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ProjectFullsGetOperation : HttpOperation
{
    [HttpResponseJsonBody]
    public List<ProjectProductFull> projectProductFulls;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/project/FullsByAppUser")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ProjectFullsByAppUserGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpResponseJsonBody]
    public List<ProjectProductFull> projectProductFulls;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/project/Info")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ProjectInfoGetOperation : HttpOperation
{
    [HttpQueryString]
    public int id;

    [HttpQueryString]
    public bool images;

    [HttpResponseJsonBody]
    public ProjectInfo projectInfo;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/project/LikeIdsByAppUserId")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ProjectLikeIdsByAppUserIdGetOperation : HttpOperation
{
    [HttpQueryString]
    public int appUserId;

    [HttpResponseJsonBody]
    public List<int> projectLikeIds;
}

// PROJECT IMAGES

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/project/Images")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
[HttpTimeout(30f)]
public class ProjectImagesGetOperation : HttpOperation
{
    [HttpQueryString]
    public bool first;

    [HttpResponseJsonBody]
    public ProjectImages[] projectImages;
}

[HttpGET]
[HttpPathExt(WebServiceType.Main, "/project/ImagesById")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
[HttpTimeout(20f)]
public class ProjectImagesByIdGetOperation : HttpOperation
{
    [HttpQueryString]
    public int id;

    [HttpResponseJsonBody]
    public String[] images;
}

// REGISTER
[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/project/Register")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ProjectRegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public ProjectInfo projectInfo;

    [HttpResponseTextBody]
    public String id;
}


[HttpPOST]
[HttpPathExt(WebServiceType.Main, "/project/RegisterLike")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ProjectLikeRegisterOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public ProjectLike projectLike;

    [HttpResponseTextBody]
    public String projectLikeId;
}

// UPDATE
[HttpPUT]
[HttpPathExt(WebServiceType.Main, "/project")]
[HttpProvider(typeof(HttpUnityWebAzureClient))]
[HttpContentType("application/json")]
[HttpAccept("application/json")]
[HttpFirebaseAuthorization]
public class ProjectUpdateOperation : HttpOperation
{
    [HttpRequestJsonBody]
    public ProjectInfo projectInfo;
}