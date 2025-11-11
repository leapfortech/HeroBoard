using System;
using UnityEngine;
using UnityEngine.Events;

using Leap.Graphics.Tools;
using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.Data.Web;
using Leap.Identity.Vision;
using Leap.Vision.Tools;

using Sirenix.OdinInspector;

public class ObdPortraitAction : MonoBehaviour
{
    [Title("Portraits")]
    [SerializeField]
    Image imgPhotoPortrait = null;
    [SerializeField]
    Image imgRenapPortrait = null;
    [SerializeField]
    Image imgDpiPortrait = null;

    [Title("Verify")]
    [SerializeField]
    Toggle tglDpiBadPhoto = null;
    [SerializeField]
    Toggle tglDpiInvalid = null;

    [Space]
    [SerializeField]
    Button btnVerifyPhoto = null;
    [SerializeField]
    Text txtVerifyPhoto = null;

    [Space]
    [SerializeField]
    Button btnVerifyDpi = null;
    [SerializeField]
    Text txtVerifyDpi = null;

    [Title("Validation")]
    [SerializeField]
    ToggleGroup tggPortraitValidation = null;

    [Title("Events")]
    [SerializeField]
    UnityEvent onAddOnboarding = null;

    [SerializeField]
    UnityEvent onUpdateOnboarding = null;

    IdentityService identityService = null;
    IdentityVisionService visionService;

    bool isPhoto = true;
    int verifyPhoto = 16383;
    int verifyDpi = 16383;

    Onboarding onboarding = null;
    bool valueChange = false, resultChange = false;

    private void Awake()
    {
        identityService = GetComponent<IdentityService>();
        visionService = GetComponent<IdentityVisionService>();
    }

    // Portrait

    public void GetAppUserPhoto(int appUserIdx)
    {
        Clear();

        //ScreenDialog.Instance.Display();
        identityService.GetPortraitByAppUserId(StateManager.Instance.AppUsers[appUserIdx].Id);
    }

    public void ApplyAppUserPhoto(String portrait)
    {
        //Texture2D photo = portrait.ToTexture2D("PhotoPortrait");
        //imgPhotoPortrait.Sprite = photo.Crop(new Rect(0, photo.height / 8, photo.width, photo.height * 3 / 4), new Vector2Int(photo.width, photo.height * 3 / 4), true).CreateSprite();

        imgPhotoPortrait.Sprite?.Destroy();
        imgPhotoPortrait.Sprite = portrait.ToTexture2D("PhotoPortrait").CreateSprite();

        btnVerifyPhoto.Interactable = imgPhotoPortrait.Sprite != null;

        ApplyOnboarding();
    }

    public void ApplyAppUserIdentity(IdentityBoardInfo identityBoardInfo)
    {
        imgDpiPortrait.Sprite?.Destroy();
        imgDpiPortrait.Sprite = identityBoardInfo.DpiBoardPhoto?.DpiPortrait?.CreateSprite("DpiPortrait");

        btnVerifyDpi.Interactable = imgDpiPortrait.Sprite != null;
        tglDpiBadPhoto.Interactable = imgDpiPortrait.Sprite != null;
        tglDpiInvalid.Interactable = imgDpiPortrait.Sprite != null;
    }

    public void ApplyRenapIdentity(RenapIdentityInfo renapIdentityInfo)
    {
        imgRenapPortrait.Sprite?.Destroy();
        imgRenapPortrait.Sprite = renapIdentityInfo.Face?.CreateSprite("RenapPortrait");
    }

    // Verify

    public void VerifyFaces(bool photo)
    {
        isPhoto = photo;

        if (imgRenapPortrait.Sprite == null)
        {
            ChoiceDialog.Instance.Error("Se necesita la fotografía del Renap.");
            return;
        }

        if (photo)
        {
            if (imgPhotoPortrait.Sprite == null)
            {
                ChoiceDialog.Instance.Error("Se necesita la fotografía de la persona.");
                return;
            }
        }
        else if (imgDpiPortrait.Sprite == null)
        {
            ChoiceDialog.Instance.Error("Se necesita la fotografía del DPI.");
            return;
        }

        VisionRequest visionRequest = new VisionRequest();
        visionRequest.Image = imgRenapPortrait.Sprite.ToStrBase64(ImageType.JPG);
        if (photo)
            visionRequest.Image2 = imgPhotoPortrait.Sprite.ToStrBase64(ImageType.JPG);
        else
            visionRequest.Image2 = imgDpiPortrait.Sprite.ToStrBase64(ImageType.JPG);

        ScreenDialog.Instance.Display();
        visionService.CompareFacesVision(visionRequest);
    }

    public void OnFacesVerified(LeapScoresResponse response)
    {
        ScreenDialog.Instance.Hide();

        if (response == null || response.Result == null || response.Result.Scores[0] == 0f)
        {
            ChoiceDialog.Instance.Info("El cotejo no devolvió ningún resultado.");
            return;
        }

        if (isPhoto)
        {
            btnVerifyPhoto.Interactable = false;
            verifyPhoto = Mathf.RoundToInt(response.Result.Scores[1] * 100f);
            txtVerifyPhoto.TextValue = verifyPhoto.ToString() + " %";
        }
        else
        {
            btnVerifyDpi.Interactable = false;
            verifyDpi = Mathf.RoundToInt(response.Result.Scores[1] * 100f);
            txtVerifyDpi.TextValue = verifyDpi.ToString() + " %";
        }
    }

    // Onboarding

    private void ApplyOnboarding()
    {
        if (StateManager.Instance.Onboarding == null)
        {
            Invoke(nameof(ApplyOnboarding), 0.1f);
            return;
        }

        onboarding = StateManager.Instance.Onboarding;

        verifyPhoto = onboarding.GetPortraitValue(true);
        btnVerifyPhoto.Interactable = verifyPhoto > 100 && imgPhotoPortrait.Sprite != null;
        txtVerifyPhoto.TextValue = verifyPhoto > 100 ? "N/A" : verifyPhoto.ToString() + " %";

        verifyDpi = onboarding.GetPortraitValue(false);
        btnVerifyDpi.Interactable = verifyDpi > 100 && imgDpiPortrait.Sprite != null;
        txtVerifyDpi.TextValue = verifyDpi > 100 ? "N/A" : verifyDpi.ToString() + " %";

        tggPortraitValidation.Value = onboarding.GetPortraitResult().ToString();
    }

    public void SendPortrait()
    {
        onboarding = new Onboarding(StateManager.Instance.Onboarding);

        // Value
        valueChange = onboarding.SetPortraitValue(true, verifyPhoto);
        valueChange |= onboarding.SetPortraitValue(false, verifyDpi);

        // Result
        resultChange = onboarding.GetPortraitResult() != Convert.ToInt32(tggPortraitValidation.Value);

        if (resultChange)
        {
            onboarding.SetPortraitResult(Convert.ToInt32(tggPortraitValidation.Value));
            onboarding.BoardUserId = StateManager.Instance.BoardUser.Id;
            onboarding.Status = onboarding.GetStatus(onboarding.GetPortraitResult());

            if (Onboarding.GetResultType(onboarding.GetPortraitResult()) == 1)  // Accepted
            {
                if (verifyPhoto > 100 && imgPhotoPortrait.Sprite != null)
                {
                    ChoiceDialog.Instance.Error("Para aceptar la Selfie, es necesario hacer el cotejo.");
                    onboarding = null;
                    return;
                }
                
                if (verifyDpi > 100 && imgDpiPortrait.Sprite != null)
                {
                    ChoiceDialog.Instance.Error("Para aceptar la fotografía del DPI, es necesario hacer el cotejo.");
                    onboarding = null;
                    return;
                }

                if (verifyPhoto < 60 && imgPhotoPortrait.Sprite != null)
                {
                    ChoiceDialog.Instance.Warning("Selfie", $"¿Estás seguro de que quieres aceptar la Selfie con un resultado del cotejo facial de {verifyPhoto}% ?", VerifyDpi, () => { onboarding = null; }, "Sí", "No");
                    return;
                }

                VerifyDpi();
                return;
            }
        }
        else if (!valueChange)
        {
            ChoiceDialog.Instance.Warning("Fotografía", "No hay ningún cambio.");
            onboarding = null;
            return;
        }

        AskSendPortrait();
    }

    private void VerifyDpi()
    {
        if (verifyDpi < 60 && imgDpiPortrait.Sprite != null)
        {
            ChoiceDialog.Instance.Warning("DPI", $"¿Estás seguro de que quieres aceptar el DPI con un resultado del cotejo facial de {verifyDpi}% ?", AskSendPortrait, () => { onboarding = null; }, "Sí", "No");
            return;
        }

        AskSendPortrait();
    }

    private void AskSendPortrait()
    {
        ChoiceDialog.Instance.Info("Fotografía", "¿Estás seguro de que quieres enviar los cambios?", SendAppUserPortrait, null, "Sí", "No");
    }

    private void SendAppUserPortrait()
    {
        ScreenDialog.Instance.Display();

        if (resultChange)
        {
            onboarding.CreateDateTime = onboarding.UpdateDateTime = DateTime.Now;
            StateManager.Instance.Onboardings.Insert(0, onboarding);
            onAddOnboarding.Invoke();
        }
        else if (valueChange)
        {
            onboarding.UpdateDateTime = DateTime.Now;
            StateManager.Instance.Onboardings[0] = onboarding;
            onUpdateOnboarding.Invoke();
        }

        onboarding = null;
    }

    // Clear

    public void Clear()
    {
        verifyPhoto = 16383;
        txtVerifyPhoto.TextValue = "N/A";

        verifyDpi = 16383;
        txtVerifyDpi.TextValue = "N/A";
    }
}
