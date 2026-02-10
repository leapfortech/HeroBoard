using System;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class AddressService : MonoBehaviour
{
    [Serializable]
    public class AddressEvent : UnityEvent<Address> { }

    [Serializable]
    public class AddressInfoEvent : UnityEvent<AddressInfo> { }

    [SerializeField]
    private AddressEvent onRetreived = null;

    [SerializeField]
    private AddressInfoEvent onInfoRetreived = null;

    [SerializeField]
    private UnityLongEvent onRegistered = null;

    [SerializeField]
    private UnityLongEvent onAdded = null;

    [SerializeField]
    private UnityLongEvent onUpdated = null;

    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;


    // GET
    public void GetAddress(long appUserId)
    {
       AddressGetOperation addressGetOp = new AddressGetOperation();
        try
        {
            addressGetOp.appUserId = appUserId;
            addressGetOp["on-complete"] = (Action<AddressGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRetreived.Invoke(op.address);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            addressGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
    public void GetAddressInfo(long appUserId, int status = 1)
    {
        AddressInfoGetOperation addressInfoGetOp = new AddressInfoGetOperation();
        try
        {
            addressInfoGetOp.appUserId = appUserId;
            addressInfoGetOp.status = status;
            addressInfoGetOp["on-complete"] = (Action<AddressInfoGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onInfoRetreived.Invoke(op.addressInfo);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            addressInfoGetOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // REGISTER
    public void Register(AddressInfo addressFull)
    {
        AddressRegisterOperation addressRegisterPostOp = new AddressRegisterOperation();
        try
        {
            addressRegisterPostOp.addressInfo = addressFull;
            addressRegisterPostOp["on-complete"] = (Action<AddressRegisterOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRegistered.Invoke(Convert.ToInt64(op.id));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            addressRegisterPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // ADD
    public void Add(Address address)
    {
        AddressPostOperation addressPostOp = new AddressPostOperation();
        try
        {
            addressPostOp.address = address;
            addressPostOp["on-complete"] = (Action<AddressPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onAdded.Invoke(Convert.ToInt64(op.id));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            addressPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // UPDATE
    public void UpdateAddress(Address address)
    {
        AddressPutOperation addressPutOp = new AddressPutOperation();
        try
        {
            addressPutOp.address = address;
            addressPutOp["on-complete"] = (Action<AddressPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdated.Invoke(Convert.ToInt64(op.id));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            addressPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void UpdateAddress(long appUserId, Address address)
    {
        AddressAppUserPutOperation addressPutOp = new AddressAppUserPutOperation();
        try
        {
            addressPutOp.appUserId = appUserId;
            addressPutOp.address = address;
            addressPutOp["on-complete"] = (Action<AddressAppUserPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdated.Invoke(Convert.ToInt64(op.id));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            addressPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void UpdateAddressInfo(AddressInfo addressInfo)
    {
        AddressInfoPutOperation addressFullPutOp = new AddressInfoPutOperation();
        try
        {
            addressFullPutOp.addressInfo = addressInfo;
            addressFullPutOp["on-complete"] = (Action<AddressInfoPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdated.Invoke(Convert.ToInt64(op.id));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            addressFullPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
