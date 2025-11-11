using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class ProductService : MonoBehaviour
{
    //[Serializable]
    //public class ProjectFullsEvent : UnityEvent<List<ProjectProductFull>> { }

    //[SerializeField]
    //private ProjectFullsEvent onFullsRetreived = null;

    [SerializeField]
    private UnityIntEvent onFractionedRegistered = null;

    [SerializeField]
    private UnityIntEvent onFinancedRegistered = null;

    [SerializeField]
    private UnityIntEvent onPrepaidRegistered = null;

    //[SerializeField]
    //private UnityEvent onUpdated = null;


    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;


    // GET
    //public void GetFulls()
    //{
    //    ProjectFullsGetOperation projectFullsGetOp = new ProjectFullsGetOperation();
    //    try
    //    {
    //        projectFullsGetOp["on-complete"] = (Action<ProjectFullsGetOperation, HttpResponse>)((op, response) =>
    //        {
    //            if (response != null && !response.HasError)
    //                onFullsRetreived.Invoke(op.projectProductFulls);
    //            else
    //                onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
    //        });
    //        projectFullsGetOp.Send();
    //    }
    //    catch (Exception ex)
    //    {
    //        WebManager.Instance.OnSendError(ex.Message);
    //    }
    //}

    // REGISTER
    public void Register(ProductFractionated productFractionated)
    {
        ProductFractionatedPostOperation registerOp = new ProductFractionatedPostOperation();
        try
        {
            registerOp.productFractionated = productFractionated;
            registerOp["on-complete"] = (Action<ProductFractionatedPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFractionedRegistered.Invoke(Convert.ToInt32(op.productId));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            registerOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void Register(ProductFinanced productFinanced)
    {
        ProductFinancedPostOperation registerOp = new ProductFinancedPostOperation();
        try
        {
            registerOp.productFinanced = productFinanced;
            registerOp["on-complete"] = (Action<ProductFinancedPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onFinancedRegistered.Invoke(Convert.ToInt32(op.productId));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            registerOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void Register(ProductPrepaid productPrepaid)
    {
        ProductPrepaidPostOperation registerOp = new ProductPrepaidPostOperation();
        try
        {
            registerOp.productPrepaid = productPrepaid;
            registerOp["on-complete"] = (Action<ProductPrepaidPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onPrepaidRegistered.Invoke(Convert.ToInt32(op.productId));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            registerOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // UPDATE
    //public void UpdateProject(ProjectRequest projectRequest)
    //{
    //    ProjectUpdateOperation updateOp = new ProjectUpdateOperation();
    //    try
    //    {
    //        updateOp.projectRequest = projectRequest;
    //        updateOp["on-complete"] = (Action<ProjectUpdateOperation, HttpResponse>)((op, response) =>
    //        {
    //            if (response != null && !response.HasError)
    //                onUpdated.Invoke();
    //            else
    //                onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
    //        });
    //        updateOp.Send();
    //    }
    //    catch (Exception ex)
    //    {
    //        WebManager.Instance.OnSendError(ex.Message);
    //    }
    //}
}
