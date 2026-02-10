using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Leap.Graphics.Tools;
using Leap.Data.Collections;
using Leap.Data.Mapper;
using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.UI.Extensions;

using Sirenix.OdinInspector;

public class ObdDpiAction : MonoBehaviour
{
    [Title("Fields")]
    [SerializeField, ListDrawerSettings(CustomAddFunction = nameof(AddFrontField), CustomRemoveIndexFunction = nameof(RemoveFrontField), DraggableItems = false)]
    List<ObdField> fldDpiFront = null;

    [Space]
    [SerializeField, ListDrawerSettings(CustomAddFunction = nameof(AddBackField), CustomRemoveIndexFunction = nameof(RemoveBackField), DraggableItems = false)]
    List<ObdField> fldDpiBack = null;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmIdentity = null;

    [Space]
    [SerializeField]
    DataMapper dtmNationalityVll = null;

    [SerializeField]
    DataMapper dtmNationalityLst = null;

    [Title("DPI")]
    [SerializeField]
    Image dpiFront = null;

    [SerializeField]
    Image dpiBack = null;

    [Title("Nationality")]
    [SerializeField]
    ComboAdapter cmbNationality = null;

    [SerializeField]
    int maxNationalities = 10;

    [Space]
    [SerializeField]
    ValueList vllCountryFlagAll = null;

    [SerializeField]
    ValueList vllNationality = null;

    [Title("Version")]
    [SerializeField]
    InputField ifdVersion1 = null;

    [SerializeField]
    InputField ifdVersion2 = null;

    [Title("MRZ")]
    [SerializeField]
    InputField ifdMRZ1 = null;

    [SerializeField]
    InputField ifdMRZ2 = null;

    [SerializeField]
    InputField ifdMRZ3 = null;

    [Title("Validation")]
    [SerializeField]
    ToggleGroup tggFrontValidation = null;

    [SerializeField]
    ToggleGroup tggBackValidation = null;

    [Title("Events")]
    [SerializeField]
    UnityEvent onAddOnboarding = null;

    [SerializeField]
    UnityEvent onUpdateOnboarding = null;

    IdentityService identityService = null;

    List<Sprite> sprDpiFront = new List<Sprite>();
    List<Sprite> sprDpiBack = new List<Sprite>();
    int sprDpiFrontIdx, sprDpiBackIdx;
    Text txtDpiFrontCount, txtDpiBackCount;
    Button btnDpiFrontLeft, btnDpiFrontRight;
    Button btnDpiBackLeft, btnDpiBackRight;

    int nationalityFieldIdx, versionFieldIdx;
    int mrz1FieldIdx, mrz2FieldIdx, mrz3FieldIdx;

    Onboarding onboarding = null;
    bool checkChange = false, fieldChange = false, resultChange = false;

    private void Awake()
    {
        identityService = GetComponent<IdentityService>();

        // Front
        nationalityFieldIdx = FindFrontFieldIdx("-NationalityIds");
        versionFieldIdx = FindFrontFieldIdx("-DpiVersion");

        // Back
        mrz1FieldIdx = FindBackFieldIdx("-DpiMrz1");
        mrz2FieldIdx = FindBackFieldIdx("-DpiMrz2");
        mrz3FieldIdx = FindBackFieldIdx("-DpiMrz3");

        // Count
        txtDpiFrontCount = dpiFront.GetComponentInChildren<Text>();
        btnDpiFrontLeft = txtDpiFrontCount.transform.GetChild(1).GetComponent<Button>();
        btnDpiFrontLeft.SetAction(DpiFrontLeft);
        btnDpiFrontRight = txtDpiFrontCount.transform.GetChild(2).GetComponent<Button>();
        btnDpiFrontRight.SetAction(DpiFrontRight);

        txtDpiBackCount = dpiBack.GetComponentInChildren<Text>();
        btnDpiBackLeft = txtDpiBackCount.transform.GetChild(1).GetComponent<Button>();
        btnDpiBackLeft.SetAction(DpiBackLeft);
        btnDpiBackRight = txtDpiBackCount.transform.GetChild(2).GetComponent<Button>();
        btnDpiBackRight.SetAction(DpiBackRight);
    }

    // Fields

    private ObdField AddFrontField()
    {
        return new ObdField(fldDpiFront.Count);
    }

    private void RemoveFrontField(int idx)
    {
        fldDpiFront.RemoveAt(idx);
        for (int i = idx; i < fldDpiFront.Count; i++)
            fldDpiFront[i].Idx--;
    }

    private ObdField AddBackField()
    {
        return new ObdField(fldDpiBack.Count);
    }

    private void RemoveBackField(int idx)
    {
        fldDpiBack.RemoveAt(idx);
        for (int i = idx; i < fldDpiBack.Count; i++)
            fldDpiBack[i].Idx--;
    }

    private int FindFrontFieldIdx(String name)
    {
        for (int i = 0; i < fldDpiFront.Count; i++)
            if (fldDpiFront[i].Name == name)
                return fldDpiFront[i].Idx;
        return -1;
    }

    private int FindBackFieldIdx(String name)
    {
        for (int i = 0; i < fldDpiBack.Count; i++)
            if (fldDpiBack[i].Name == name)
                return fldDpiBack[i].Idx;
        return -1;
    }

    // AppUser

    public void GetAppUserIdentity(int appUserIdx)
    {
        Clear();

        ScreenDialog.Instance.Display();
        identityService.GetBoardInfoByAppUserId(StateManager.Instance.AppUsers[appUserIdx].Id, 2);
    }

    public void ApplyAppUserIdentity(IdentityBoardInfo identityBoardInfo)
    {
        StateManager.Instance.AppUserIdentity = identityBoardInfo.Identity;
        dtmIdentity.PopulateClass<Identity>(identityBoardInfo.Identity);

        // DpiFront
        for (int i = 0; i < sprDpiFront.Count; i++)
            sprDpiFront[i]?.Destroy();
        sprDpiFront = new List<Sprite>();

        if (identityBoardInfo.DpiBoardPhoto.DpiFronts != null && identityBoardInfo.DpiBoardPhoto.DpiFronts.Length > 0)
        {
            for (int i = 0; i < identityBoardInfo.DpiBoardPhoto.DpiFronts.Length; i++)
                sprDpiFront.Add(identityBoardInfo.DpiBoardPhoto.DpiFronts[i].CreateSprite($"DpiFront|{i:D02}"));
            sprDpiFrontIdx = 0;
            txtDpiFrontCount.gameObject.SetActive(true); // (sprDpiFront.Count > 1);

            DpiFrontDisplay();
        }
        else
        {
            sprDpiFrontIdx = -1;
            dpiFront.Sprite = null;
            txtDpiFrontCount.gameObject.SetActive(false);
        }

        // DpiBack
        for (int i = 0; i < sprDpiBack.Count; i++)
            sprDpiBack[i]?.Destroy();
        sprDpiBack = new List<Sprite>();

        if (identityBoardInfo.DpiBoardPhoto.DpiBacks != null && identityBoardInfo.DpiBoardPhoto.DpiBacks.Length > 0)
        {
            for (int i = 0; i < identityBoardInfo.DpiBoardPhoto.DpiBacks.Length; i++)
                sprDpiBack.Add(identityBoardInfo.DpiBoardPhoto.DpiBacks[i].CreateSprite($"DpiBack|{i:D02}"));
            sprDpiBackIdx = 0;
            txtDpiBackCount.gameObject.SetActive(true); // (sprDpiBack.Count > 1);

            DpiBackDisplay();
        }
        else
        {
            sprDpiBackIdx = -1;
            dpiBack.Sprite = null;
            txtDpiBackCount.gameObject.SetActive(false);
        }

        // Nationality
        String[] countryIds = identityBoardInfo.Identity.NationalityIds.Split('|', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < countryIds.Length; i++)
            AddNationality(Convert.ToInt32(countryIds[i]));

        // Version
        ifdVersion1.Text = identityBoardInfo.Identity.DpiVersion;
        ifdVersion1.InputValidate();
        ifdVersion2.Text = identityBoardInfo.Identity.DpiVersion;
        ifdVersion2.InputValidate();

        // MRZ
        ifdMRZ1.Text = String.IsNullOrEmpty(identityBoardInfo.Identity.DpiMrz) ? "" : identityBoardInfo.Identity.DpiMrz[..30];
        ifdMRZ1.Text = ifdMRZ1.Text.Replace(" ", "");
        ifdMRZ1.InputValidate();
        ifdMRZ2.Text = String.IsNullOrEmpty(identityBoardInfo.Identity.DpiMrz) ? "" : identityBoardInfo.Identity.DpiMrz[30..60];
        ifdMRZ2.Text = ifdMRZ2.Text.Replace(" ", "");
        ifdMRZ2.InputValidate();
        ifdMRZ3.Text = String.IsNullOrEmpty(identityBoardInfo.Identity.DpiMrz) ? "" : identityBoardInfo.Identity.DpiMrz[60..];
        ifdMRZ3.Text = ifdMRZ3.Text.Replace(" ", "");
        ifdMRZ3.InputValidate();

        ScreenDialog.Instance.Hide();

        ApplyOnboarding();
    }

    private void DpiFrontLeft()
    {
        if (sprDpiFrontIdx == 0)
            return;
        sprDpiFrontIdx--;
        DpiFrontDisplay();
    }

    private void DpiFrontRight()
    {
        if (sprDpiFrontIdx == sprDpiFront.Count - 1)
            return;
        sprDpiFrontIdx++;
        DpiFrontDisplay();
    }

    private void DpiFrontDisplay()
    {
        dpiFront.Sprite = sprDpiFront[sprDpiFrontIdx];
        txtDpiFrontCount.TextValue = $"{sprDpiFrontIdx + 1} / {sprDpiFront.Count}";
        btnDpiFrontLeft.Interactable = sprDpiFrontIdx > 0;
        btnDpiFrontRight.Interactable = sprDpiFrontIdx < sprDpiFront.Count - 1;
    }

    private void DpiBackLeft()
    {
        if (sprDpiBackIdx == 0)
            return;
        sprDpiBackIdx--;
        DpiBackDisplay();
    }

    private void DpiBackRight()
    {
        if (sprDpiBackIdx == sprDpiBack.Count - 1)
            return;
        sprDpiBackIdx++;
        DpiBackDisplay();
    }

    private void DpiBackDisplay()
    {
        dpiBack.Sprite = sprDpiBack[sprDpiBackIdx];
        txtDpiBackCount.TextValue = $"{sprDpiBackIdx + 1} / {sprDpiBack.Count}";
        btnDpiBackLeft.Interactable = sprDpiBackIdx > 0;
        btnDpiBackRight.Interactable = sprDpiBackIdx < sprDpiBack.Count - 1;
    }

    // Nationality

    public void AddNationality()
    {
        if (cmbNationality.Combo.IsEmpty())
            return;

        if (vllNationality.RecordCount >= maxNationalities)
        {
            ChoiceDialog.Instance.Error("No se pueden ingresar más de " + maxNationalities + " Nacionalidades.");
            return;
        }

        long cmbNationalityId = cmbNationality.GetSelectedId();
        AddNationality(cmbNationalityId);

        cmbNationality.Clear();
    }

    private void AddNationality(long countryId)
    {
        ValueRecord record = vllCountryFlagAll.FindRecord(countryId);
        //if (record == null)
        //    record = vllCountryFlagAll.FindRecord("Code", countryId);
        if (record == null)
            return;

        vllNationality.AddRecord(record.GetCellString("Name"), record.GetCellString("Code"), record.GetCellSprite("Flag"), record.Id.ToString());
        UpdateNationality();
    }

    public void RemoveNationality(int idx)
    {
        vllNationality.RemoveRecord(idx);
        UpdateNationality();
    }

    public void RemoveAllNationalities()
    {
        vllNationality.ClearRecords();
        UpdateNationality();
    }

    private void UpdateNationality()
    {
        List<CountryFlag> countryFlag = dtmNationalityVll.BuildClassList<CountryFlag>();
        dtmNationalityLst.PopulateClassList(countryFlag);
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

        // DpiFront

        for (int idx = 0; idx < fldDpiFront.Count; idx++)
            if (fldDpiFront[idx].Check != null)
                fldDpiFront[idx].Check.Checked = onboarding.GetDpiFrontCheck(idx);

        tggFrontValidation.Value = onboarding.GetDpiFrontResult().ToString();

        // DpiBack

        for (int idx = 0; idx < fldDpiBack.Count; idx++)
            if (fldDpiBack[idx].Check != null)
                fldDpiBack[idx].Check.Checked = onboarding.GetDpiBackCheck(idx);

        tggBackValidation.Value = onboarding.GetDpiBackResult().ToString();
    }

    public void SendDpiFront()
    {
        if (!DoSendDpiFront())
        {
            onboarding = null;
        }
    }

    private bool DoSendDpiFront()
    {
        Identity identity = StateManager.Instance.AppUserIdentity;
        Identity newIdentity = dtmIdentity.BuildClass<Identity>();

        onboarding = new Onboarding(StateManager.Instance.Onboarding);

        checkChange = fieldChange = false;

        for (int idx = 0; idx < nationalityFieldIdx; idx++)
        {
            if (fldDpiFront[idx].Check != null && fldDpiFront[idx].Check.Checked)
                if (!ElementHelper.Validate(fldDpiFront[idx].Check.transform.parent.GetComponent<ElementValue>()))
                    return false;

            checkChange |= onboarding.SetDpiFrontCheck(fldDpiFront[idx]);
            fieldChange |= onboarding.SetIdentityField(fldDpiFront[idx], identity, newIdentity);
        }

        // Nationality
        checkChange |= onboarding.SetDpiFrontCheck(nationalityFieldIdx, fldDpiFront[nationalityFieldIdx].Check.Checked);

        String ids = "";

        if (vllNationality.RecordCount > 0)
        {
            ids = vllNationality[0].GetCellString("CountryId");
            for (int i = 1; i < vllNationality.RecordCount; i++)
                ids += "|" + vllNationality[i].GetCellString("CountryId");
        }
        else if (fldDpiFront[nationalityFieldIdx].Check.Checked)
        {
            ChoiceDialog.Instance.Error("Es necesario ingresar por lo menos 1 nacionalidad.");
            return false;
        }

        if (identity.NationalityIds != ids)
        {
            identity.NationalityIds = ids;
            fieldChange = true;
        }

        // Version
        checkChange |= onboarding.SetDpiFrontCheck(versionFieldIdx, ifdVersion1.Text.Length > 0 && ifdVersion1.Text == ifdVersion2.Text);

        if (identity.DpiVersion != ifdVersion1.Text || identity.DpiVersion != ifdVersion2.Text)
        {
            if (ifdVersion1.Text.Length > 0 && !ElementHelper.Validate(ifdVersion1))
                return false;
            if (ifdVersion2.Text.Length > 0 && !ElementHelper.Validate(ifdVersion2))
                return false;

            if (ifdVersion1.Text != ifdVersion2.Text)
            {
                ChoiceDialog.Instance.Error("Los campos de número de versión tienen que coincidir.");
                return false;
            }

            identity.DpiVersion = ifdVersion1.Text;
            fieldChange = true;
        }

        // Result
        resultChange = onboarding.GetDpiFrontResult() != Convert.ToInt32(tggFrontValidation.Value);

        if (resultChange)
        {
            onboarding.SetDpiFrontResult(Convert.ToInt32(tggFrontValidation.Value));
            onboarding.BoardUserId = StateManager.Instance.BoardUser.Id;
            onboarding.Status = onboarding.GetStatus(onboarding.GetDpiFrontResult());

            if (Onboarding.GetResultType(onboarding.GetDpiFrontResult()) == 1 && onboarding.GetDpiFrontChecks() != (1 << fldDpiFront.Count) - 1)
            {
                ChoiceDialog.Instance.Error("Para validar una etapa, es necesario chequear todos los campos.");
                return false;
            }
        }
        else if (!fieldChange && !checkChange)
        {
            ChoiceDialog.Instance.Warning("Dpi Frente", "No hay ningún cambio.");
            return false;
        }

        ChoiceDialog.Instance.Info("Dpi Frente", "¿Estás seguro de que quieres enviar los cambios?", SendAppUserIdentity, null, "Sí", "No");
        return true;
    }

    public void SendDpiBack()
    {
        if (!DoSendDpiBack())
        {
            onboarding = null;
        }
    }

    private bool DoSendDpiBack()
    {
        Identity identity = StateManager.Instance.AppUserIdentity;
        Identity newIdentity = dtmIdentity.BuildClass<Identity>();

        onboarding = new Onboarding(StateManager.Instance.Onboarding);

        checkChange = fieldChange = false;

        for (int idx = 0; idx < mrz1FieldIdx; idx++)
        {
            if (fldDpiBack[idx].Check != null && fldDpiBack[idx].Check.Checked)
                if (!ElementHelper.Validate(fldDpiBack[idx].Check.transform.parent.GetComponent<ElementValue>()))
                    return false;

            checkChange |= onboarding.SetDpiBackCheck(fldDpiBack[idx]);
            fieldChange |= onboarding.SetIdentityField(fldDpiBack[idx], identity, newIdentity);
        }

        // MRZ
        checkChange |= onboarding.SetDpiBackCheck(mrz1FieldIdx, fldDpiBack[mrz1FieldIdx].Check.Checked);
        checkChange |= onboarding.SetDpiBackCheck(mrz2FieldIdx, fldDpiBack[mrz2FieldIdx].Check.Checked);
        checkChange |= onboarding.SetDpiBackCheck(mrz3FieldIdx, fldDpiBack[mrz3FieldIdx].Check.Checked);

        if (fldDpiBack[mrz1FieldIdx].Check.Checked && !ElementHelper.Validate(ifdMRZ1))
            return false;
        if (fldDpiBack[mrz2FieldIdx].Check.Checked && !ElementHelper.Validate(ifdMRZ2))
            return false;
        if (fldDpiBack[mrz3FieldIdx].Check.Checked && !ElementHelper.Validate(ifdMRZ3))
            return false;

        String mrz = ifdMRZ1.Text.PadRight(30, ' ') + ifdMRZ2.Text.PadRight(30, ' ') + ifdMRZ3.Text.PadRight(30, ' ');
        if (identity.DpiMrz != mrz)
        {
            identity.DpiMrz = mrz;
            fieldChange = true;
        }

        // Result
        resultChange = onboarding.GetDpiBackResult() != Convert.ToInt32(tggBackValidation.Value);

        if (resultChange)
        {
            onboarding.SetDpiBackResult(Convert.ToInt32(tggBackValidation.Value));
            onboarding.BoardUserId = StateManager.Instance.BoardUser.Id;
            onboarding.Status = onboarding.GetStatus(onboarding.GetDpiBackResult());

            if (Onboarding.GetResultType(onboarding.GetDpiBackResult()) == 1 && onboarding.GetDpiBackChecks() != (1 << fldDpiBack.Count) - 1)
            {
                ChoiceDialog.Instance.Error("Para validar una etapa, es necesario chequear todos los campos.");
                return false;
            }
        }
        else if (!fieldChange && !checkChange)
        {
            ChoiceDialog.Instance.Warning("Dpi Dorso", "No hay ningún cambio.");
            return false;
        }

        ChoiceDialog.Instance.Info("Dpi Dorso", "¿Estás seguro de que quieres enviar los cambios?", SendAppUserIdentity, null, "Sí", "No");
        return true;
    }

    // Identity

    private void SendAppUserIdentity()
    {
        ScreenDialog.Instance.Display();

        if (fieldChange)
            identityService.UpdateIdentity(StateManager.Instance.AppUserIdentity);
        else
            ApplyAppUserIdentity();
    }

    public void ApplyAppUserIdentity(int identityId = -1)
    {
        if (identityId != -1)
            StateManager.Instance.AppUserIdentity.Id = identityId;

        if (resultChange)
        {
            onboarding.CreateDateTime = onboarding.UpdateDateTime = DateTime.Now;
            StateManager.Instance.Onboardings.Insert(0, onboarding);
            onAddOnboarding.Invoke();
        }
        else if (checkChange)
        {
            onboarding.UpdateDateTime = DateTime.Now;
            StateManager.Instance.Onboardings[0] = onboarding;
            onUpdateOnboarding.Invoke();
        }
        else
            ScreenDialog.Instance.Hide();

        onboarding = null;
    }

    // Clear

    public void Clear()
    {
        dtmIdentity.ClearElements();

        dpiFront.Clear();
        dpiBack.Clear();

        ifdMRZ1.Clear();
        ifdMRZ2.Clear();
        ifdMRZ3.Clear();

        RemoveAllNationalities();
    }
}
