using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Leap.Graphics.Tools;
using Leap.Data.Mapper;
using Leap.UI.Elements;
using Leap.UI.Dialog;

using Sirenix.OdinInspector;

public class ObdAddressAction : MonoBehaviour
{
    [Title("Fields")]
    [SerializeField, ListDrawerSettings(CustomAddFunction = nameof(AddAddressField), CustomRemoveIndexFunction = nameof(RemoveAddressField), DraggableItems = false)]
    List<ObdField> fldAddress = null;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmAddress = null;

    [Title("Household Bills")]
    [SerializeField]
    ListScroller lstHouseholdBills = null;

    [Title("Validation")]
    [SerializeField]
    ToggleGroup tggAddressValidation = null;

    [Title("Events")]
    [SerializeField]
    UnityEvent onAddOnboarding = null;

    [SerializeField]
    UnityEvent onUpdateOnboarding = null;

    AddressService addressService = null;

    Onboarding onboarding = null;
    bool checkChange = false, fieldChange = false, resultChange = false;
    int addressId = -1;

    private void Awake()
    {
        addressService = GetComponent<AddressService>();
    }

    // Fields

    private ObdField AddAddressField()
    {
        return new ObdField(fldAddress.Count);
    }

    private void RemoveAddressField(int idx)
    {
        fldAddress.RemoveAt(idx);
        for (int i = idx; i < fldAddress.Count; i++)
            fldAddress[i].Idx--;
    }

    private int FindAddressFieldIdx(String name)
    {
        for (int i = 0; i < fldAddress.Count; i++)
            if (fldAddress[i].Name == name)
                return fldAddress[i].Idx;
        return -1;
    }

    // Address

    public void GetAppUserAddress(int appUserIdx)
    {
        Clear();

        //ScreenDialog.Instance.Display();
        addressService.GetAddressInfo(StateManager.Instance.AppUsers[appUserIdx].Id, 2);
    }

    public void ApplyAppUserAddress(AddressInfo addressInfo)
    {
        StateManager.Instance.AppUserAddress = addressInfo.Address;
        dtmAddress.PopulateClass<Address>(addressInfo.Address);

        ClearHouseholdBills();

        if (addressInfo.HouseholdBills != null && addressInfo.HouseholdBills.Length > 0)
        {
            ListScrollerValue lstHouseholdBillValue;
            for (int i = 0; i < addressInfo.HouseholdBills.Length; i++)
            {
                lstHouseholdBillValue = new ListScrollerValue(1, true);
                lstHouseholdBillValue.SetSprite(0, addressInfo.HouseholdBills[0]?.CreateSprite("HouseholdBill"));

                lstHouseholdBills.AddValue(lstHouseholdBillValue);
            }

        }

        lstHouseholdBills.ApplyValues();

        addressId = addressInfo.Address.Id;

        ApplyOnboarding();
    }

    private void ClearHouseholdBills()
    {
        for (int i = 0; i < lstHouseholdBills.ValuesCount; i++)
            lstHouseholdBills[i].GetSprite(0)?.Destroy();

        lstHouseholdBills.ClearValues();
    }

    public void ZoomHouseholdBill(int idx)
    {
        ZoomDialog.Instance.Display(lstHouseholdBills[idx].GetSprite(0));
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
        onboarding.AddressId = addressId;

        for (int idx = 0; idx < fldAddress.Count; idx++)
            if (fldAddress[idx].Check != null)
                fldAddress[idx].Check.Checked = onboarding.GetAddressCheck(idx);

        tggAddressValidation.Value = onboarding.GetAddressResult().ToString();
    }

    public void SendAddress()
    {
        if (!DoSendAddress())
        {
            onboarding = null;
        }
    }

    private bool DoSendAddress()
    {
        Address newAddress = dtmAddress.BuildClass<Address>();

        onboarding = new Onboarding(StateManager.Instance.Onboarding);

        checkChange = fieldChange = false;

        for (int idx = 0; idx < fldAddress.Count; idx++)
        {
            if (fldAddress[idx].Check != null && fldAddress[idx].Check.Checked)
                if (!ElementHelper.Validate(fldAddress[idx].Check.transform.parent.GetComponent<ElementValue>()))
                    return false;

            checkChange |= onboarding.SetAddressCheck(fldAddress[idx]);
            fieldChange |= onboarding.SetAddressField(fldAddress[idx], StateManager.Instance.AppUserAddress, newAddress);
        }

        // Result
        resultChange = onboarding.GetAddressResult() != Convert.ToInt32(tggAddressValidation.Value);

        if (resultChange)
        {
            onboarding.SetAddressResult(Convert.ToInt32(tggAddressValidation.Value));
            onboarding.BoardUserId = StateManager.Instance.BoardUser.Id;
            onboarding.Status = onboarding.GetStatus(onboarding.GetAddressResult());

            if (Onboarding.GetResultType(onboarding.GetAddressResult()) == 1 && onboarding.GetAddressChecks() != (1 << fldAddress.Count) - 1)
            {
                ChoiceDialog.Instance.Error("Para validar una etapa, es necesario chequear todos los campos.");
                return false;
            }
        }
        else if (!fieldChange && !checkChange)
        {
            ChoiceDialog.Instance.Warning("Dirección", "No hay ningún cambio.");
            return false;
        }

        ChoiceDialog.Instance.Info("Dirección", "¿Estás seguro de que quieres enviar los cambios?", SendAppUserAddress, null, "Sí", "No");
        return true;
    }

    private void SendAppUserAddress()
    {
        ScreenDialog.Instance.Display();

        if (fieldChange)
            addressService.UpdateAddress(StateManager.Instance.AppUser.Id, StateManager.Instance.AppUserAddress);
        else
            ApplyAppUserAddress();
    }

    public void ApplyAppUserAddress(int addressId = -1)
    {
        if (addressId != -1)
            StateManager.Instance.AppUserAddress.Id = addressId;

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
        dtmAddress.ClearElements();

        ClearHouseholdBills();
    }
}
