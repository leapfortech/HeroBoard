using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using hg.ApiWebKit.core.http;

using Leap.Core.Tools;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class OnboardingService : MonoBehaviour
{
    [Serializable]
    public class UnityOnboardingEvent : UnityEvent<Onboarding> { }

    [Serializable]
    public class UnityOnboardingsEvent : UnityEvent<List<Onboarding>> { }

    [SerializeField]
    private UnityOnboardingEvent onOnboardingRetreived = null;

    [SerializeField]
    private UnityOnboardingsEvent onOnboardingsRetreived = null;

    [SerializeField]
    private UnityIntEvent onAdded = null;

    [SerializeField]
    private UnityIntEvent onUpdated = null;

    [SerializeField]
    private UnityEvent onAuthorized = null;

    [SerializeField]
    private UnityEvent onRejected = null;

    [Title("Error")]
    [SerializeField]
    private UnityStringEvent onResponseError = null;

    public void GetOnboarding(int appUserId)
    {
        OnboardingGetOperation onboardingPostOp = new OnboardingGetOperation();
        try
        {
            onboardingPostOp.appUserId = appUserId;
            onboardingPostOp["on-complete"] = (Action<OnboardingGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onOnboardingRetreived.Invoke(op.onboarding);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            onboardingPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void GetOnboardings(int appUserId)
    {
        OnboardingsGetOperation onboardingsPostOp = new OnboardingsGetOperation();
        try
        {
            onboardingsPostOp.appUserId = appUserId;
            onboardingsPostOp["on-complete"] = (Action<OnboardingsGetOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onOnboardingsRetreived.Invoke(op.onboardings);
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            onboardingsPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void AddOnboarding(Onboarding onboarding)
    {
        OnboardingPostOperation onboardingPostOp = new OnboardingPostOperation();
        try
        {
            onboardingPostOp.onboarding = onboarding;
            onboardingPostOp["on-complete"] = (Action<OnboardingPostOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onAdded.Invoke(int.Parse(op.onboardingId));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            onboardingPostOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void UpdateOnboarding(Onboarding onboarding)
    {
        OnboardingPutOperation onboardingPutOp = new OnboardingPutOperation();
        try
        {
            onboardingPutOp.onboarding = onboarding;
            onboardingPutOp["on-complete"] = (Action<OnboardingPutOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onUpdated.Invoke(int.Parse(op.onboardingId));
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            onboardingPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    // Authorize
    public void Authorize(Onboarding onboarding)
    {
        OnboardingAuthorizeOperation onboardingPutOp = new OnboardingAuthorizeOperation();
        try
        {
            onboardingPutOp.onboardingId = onboarding.Id;
            onboardingPutOp.appUserId = onboarding.AppUserId;
            onboardingPutOp["on-complete"] = (Action<OnboardingAuthorizeOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onAuthorized.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            onboardingPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }

    public void Reject(Onboarding onboarding)
    {
        OnboardingRejectOperation onboardingPutOp = new OnboardingRejectOperation();
        try
        {
            onboardingPutOp.onboardingId = onboarding.Id;
            onboardingPutOp.appUserId = onboarding.AppUserId;
            onboardingPutOp["on-complete"] = (Action<OnboardingRejectOperation, HttpResponse>)((op, response) =>
            {
                if (response != null && !response.HasError)
                    onRejected.Invoke();
                else
                    onResponseError.Invoke(response.Text.Length == 0 ? response.Error : response.Text);
            });
            onboardingPutOp.Send();
        }
        catch (Exception ex)
        {
            WebManager.Instance.OnSendError(ex.Message);
        }
    }
}
