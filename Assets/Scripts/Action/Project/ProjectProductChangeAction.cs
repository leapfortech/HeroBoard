using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

using Leap.Core.Tools;
using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.Data.Mapper;
using Leap.Data.Collections;

using Sirenix.OdinInspector;

public class ProjectProductChangeAction : MonoBehaviour
{
    [Serializable]
    public class ProductFractionatedEvent : UnityEvent<ProductFractionated> { }
    [Serializable]
    public class ProductFinancedEvent : UnityEvent<ProductFinanced> { }
    [Serializable]
    public class ProductPrepaidEvent : UnityEvent<ProductPrepaid> { }
    [Serializable]
    public class CpiRangesEvent : UnityEvent<List<CpiRange>> { }

    [Title("Products")]
    [SerializeField]
    Toggle tglFractionated = null;
    [SerializeField]
    Toggle tglFinanced = null;
    [SerializeField]
    Toggle tglPrepaid = null;

    [Title("Lists")]
    [SerializeField]
    ListScroller lstFractionatedCpiRange = null;
    [SerializeField]
    ListScroller lstFinancedCpiRange = null;
    [SerializeField]
    ListScroller lstPrepaidCpiRange = null;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmProductFractionatedChange = null;
    [SerializeField]
    DataMapper dtmProductFinancedChange = null;
    [SerializeField]
    DataMapper dtmProductPrepaidChange = null;
    [Space]
    [SerializeField]
    DataMapper dtmCpiRangeAdd = null;
    [SerializeField]
    DataMapper dtmCpiRangeUpdate = null;
    [SerializeField]
    ValueList vllProductType = null;

    [Title("Dialogs")]
    [SerializeField]
    GameObject imgCpiRangeAdd = null;
    [SerializeField]
    GameObject imgCpiRangeUpdate = null;

    [Title("Events")]
    [SerializeField]
    private UnityIntEvent onProductCount = null;
    [SerializeField]
    private ProductFractionatedEvent onProductFractionated = null;
    [SerializeField]
    private ProductFinancedEvent onProductFinanced = null;
    [SerializeField]
    private ProductPrepaidEvent onProductPrepaid = null;
    [SerializeField]
    private CpiRangesEvent onGetCpiRanges = null;

    private readonly List<CpiRange>[] cpiRanges = { null, new List<CpiRange>(), new List<CpiRange>(), new List<CpiRange>() };
    private readonly ListScroller[] lstCpiRange = new ListScroller[4];
    private readonly CultureInfo cultureInfo = new CultureInfo("en-US");

    private GameObject rctFractionated = null, rctFinanced = null, rctPrepaid = null;
    private InputField ifdFractionated = null, ifdFinanced = null, ifdPrepaid = null;

    private int productTypeId = -1;
    private int idx = -1;

    private void Awake()
    {
        lstCpiRange[1] = lstFractionatedCpiRange;
        lstCpiRange[2] = lstFinancedCpiRange;
        lstCpiRange[3] = lstPrepaidCpiRange;

        rctFractionated = tglFractionated.transform.GetChild(2).gameObject;
        rctFinanced = tglFinanced.transform.GetChild(2).gameObject;
        rctPrepaid = tglPrepaid.transform.GetChild(2).gameObject;

        ifdFractionated = rctFractionated.transform.GetChild(0).GetComponent<InputField>();
        ifdFinanced = rctFinanced.transform.GetChild(0).GetComponent<InputField>();
        ifdPrepaid = rctPrepaid.transform.GetChild(0).GetComponent<InputField>();
    }

    private void Start()
    {
        tglFractionated.transform.GetChild(1).GetComponent<Text>().TextValue = vllProductType.FindRecordCellString(1, "Name");
        tglFinanced.transform.GetChild(1).GetComponent<Text>().TextValue = vllProductType.FindRecordCellString(2, "Name");
        tglPrepaid.transform.GetChild(1).GetComponent<Text>().TextValue = vllProductType.FindRecordCellString(3, "Name");
    }

    public void Clear()
    {
        lstFractionatedCpiRange.Clear();
        lstFinancedCpiRange.Clear();
        lstPrepaidCpiRange.Clear();

        dtmProductFractionatedChange.ClearElements();
        dtmProductFinancedChange.ClearElements();
        dtmProductPrepaidChange.ClearElements();

        ifdFractionated.Text = "-1";
        ifdFractionated.InputValidate();
        ifdFinanced.Text = "-1";
        ifdFinanced.InputValidate();
        ifdPrepaid.Text = "-1";
        ifdPrepaid.InputValidate();

        tglFractionated.Checked = false;
        tglFinanced.Checked = false;
        tglPrepaid.Checked = false;

        ToggleFractionated();
        ToggleFinanced();
        TogglePrepaid();

        for (int i = 1; i <= 3; i++)
            cpiRanges[i].Clear();

        dtmCpiRangeAdd.ClearElements();

        imgCpiRangeAdd.SetActive(false);
        imgCpiRangeUpdate.SetActive(false);
    }

    // Products
    public void ToggleFractionated()
    {
        rctFractionated.SetActive(tglFractionated.Checked);
    }

    public void ToggleFinanced()
    {
        rctFinanced.SetActive(tglFinanced.Checked);
    }

    public void TogglePrepaid()
    {
        rctPrepaid.SetActive(tglPrepaid.Checked);
    }

    public void GetProducts(int projectId, String title)
    {
        int count = tglFractionated.Checked ? 1 : 0;
        count += tglFinanced.Checked ? 1 : 0;
        count += tglPrepaid.Checked ? 1 : 0;

        onProductCount.Invoke(count);

        if (tglFractionated.Checked)
        {
            if (!dtmProductFractionatedChange.ValidateElements())
                return;

            if (cpiRanges[1].Count == 0)
            {
                ChoiceDialog.Instance.Error(title, $"{vllProductType.FindRecordCellString(1, "Name")} necesita por lo menos un rango de CPI.");
                return;
            }

            ProductFractionated productFractionated = dtmProductFractionatedChange.BuildClass<ProductFractionated>();
            productFractionated.ProductFractionatedStatusId = 1;
            productFractionated.ProjectId = projectId;
            productFractionated.ReserveRate /= 100d;

            productFractionated.CpiMin = cpiRanges[1][0].AmountMin;
            productFractionated.CpiMax = cpiRanges[1][0].AmountMax;
            for (int i = 1; i < cpiRanges[1].Count; i++)
            {
                productFractionated.CpiMin = Mathf.Min(productFractionated.CpiMin, cpiRanges[1][i].AmountMin);
                productFractionated.CpiMax = Mathf.Max(productFractionated.CpiMax, cpiRanges[1][i].AmountMax);
            }

            onProductFractionated.Invoke(productFractionated);
        }
        else
            onProductFractionated.Invoke(null);

        if (tglFinanced.Checked)
        {
            if (!dtmProductFinancedChange.ValidateElements())
                return;

            if (cpiRanges[2].Count == 0)
            {
                ChoiceDialog.Instance.Error(title, $"{vllProductType.FindRecordCellString(2, "Name")} necesita por lo menos un rango de CPI.");
                return;
            }

            ProductFinanced productFinanced = dtmProductFinancedChange.BuildClass<ProductFinanced>();
            productFinanced.ProductFinancedStatusId = 1;
            productFinanced.ProjectId = projectId;
            productFinanced.ReserveRate /= 100d;
            productFinanced.AdvRate /= 100d;

            productFinanced.CpiMin = cpiRanges[2][0].AmountMin;
            productFinanced.CpiMax = cpiRanges[2][0].AmountMax;
            for (int i = 1; i < cpiRanges[2].Count; i++)
            {
                productFinanced.CpiMin = Mathf.Min(productFinanced.CpiMin, cpiRanges[2][i].AmountMin);
                productFinanced.CpiMax = Mathf.Max(productFinanced.CpiMax, cpiRanges[2][i].AmountMax);
            }

            count++;
            onProductFinanced.Invoke(productFinanced);
        }
        else
            onProductFinanced.Invoke(null);

        if (tglPrepaid.Checked)
        {
            if (!dtmProductPrepaidChange.ValidateElements())
                return;

            if (cpiRanges[3].Count == 0)
            {
                ChoiceDialog.Instance.Error(title, $"{vllProductType.FindRecordCellString(3, "Name")} necesita por lo menos un rango de CPI.");
                return;
            }

            ProductPrepaid productPrepaid = dtmProductPrepaidChange.BuildClass<ProductPrepaid>();
            productPrepaid.ProductPrepaidStatusId = 1;
            productPrepaid.ProjectId = projectId;
            productPrepaid.ReserveRate /= 100d;

            productPrepaid.CpiMin = cpiRanges[3][0].AmountMin;
            productPrepaid.CpiMax = cpiRanges[3][0].AmountMax;
            for (int i = 1; i < cpiRanges[3].Count; i++)
            {
                productPrepaid.CpiMin = Mathf.Min(productPrepaid.CpiMin, cpiRanges[3][i].AmountMin);
                productPrepaid.CpiMax = Mathf.Max(productPrepaid.CpiMax, cpiRanges[3][i].AmountMax);
            }

            onProductPrepaid.Invoke(productPrepaid);
        }
        else
            onProductPrepaid.Invoke(null);

        List<CpiRange> allCpiRanges = new List<CpiRange>();
        for (int i = 1; i <= 3; i++)
            allCpiRanges.AddRange(cpiRanges[i]);
        for (int i = 0; i < allCpiRanges.Count; i++)
        {
            allCpiRanges[i].ProjectId = projectId;
            allCpiRanges[i].DiscountRate /= 100d;
            allCpiRanges[i].ProfitablityRate /= 100d;
        }

        onGetCpiRanges.Invoke(allCpiRanges);
    }

    public void ApplyInfo(ProjectInfo projectInfo)
    {
        tglFractionated.Checked = projectInfo.ProductFractionated != null;
        tglFinanced.Checked = projectInfo.ProductFinanced != null;
        tglPrepaid.Checked = projectInfo.ProductPrepaid != null;

        ToggleFractionated();
        ToggleFinanced();
        TogglePrepaid();

        if (projectInfo.ProductFractionated != null)
            dtmProductFractionatedChange.PopulateClass(projectInfo.ProductFractionated);
        if (projectInfo.ProductFinanced != null)
            dtmProductFinancedChange.PopulateClass(projectInfo.ProductFinanced);
        if (projectInfo.ProductPrepaid != null)
            dtmProductPrepaidChange.PopulateClass(projectInfo.ProductPrepaid);

        cpiRanges[1].Clear();
        cpiRanges[2].Clear();
        cpiRanges[3].Clear();

        for (int i = 0; i < projectInfo.CpiRanges.Count; i++)
            cpiRanges[projectInfo.CpiRanges[i].ProductTypeId].Add(projectInfo.CpiRanges[i]);

        Refresh(1);
        Refresh(2);
        Refresh(3);
    }

    // Add
    public void DisplayFractionatedAdd() => DisplayAdd(1);
    public void DisplayFinancedAdd() => DisplayAdd(2);
    public void DisplayPrepaidAdd() => DisplayAdd(3);

    public void DisplayAdd(int productTypeId)
    {
        this.productTypeId = productTypeId;

        dtmCpiRangeAdd.ClearElements();

        imgCpiRangeAdd.SetActive(true);
    }

    public void AddCpiRange()
    {
        if (!dtmCpiRangeAdd.ValidateElements())
            return;

        CpiRange cpiRange = dtmCpiRangeAdd.BuildClass<CpiRange>();
        if (cpiRange.AmountMax < cpiRange.AmountMin)
        {
            ChoiceDialog.Instance.Error("Agregar un rango de CPI", "La cantidad máxima de CPI tiene que ser superior a la cantidad mínima.");
            return;
        }

        cpiRange.Id = -1;
        cpiRange.ProductTypeId = productTypeId;

        cpiRanges[productTypeId].Add(cpiRange);
        cpiRanges[productTypeId].Sort((cr1, cr2) => { return cr1.AmountMin.CompareTo(cr2.AmountMin); });

        dtmCpiRangeAdd.ClearElements();

        Refresh(productTypeId);
    }

    // Update
    public void DisplayFractionatedUpdate(int idx) => DisplayUpdate(1, idx);
    public void DisplayFinancedUpdate(int idx) => DisplayUpdate(2, idx);
    public void DisplayPrepaidUpdate(int idx) => DisplayUpdate(3, idx);

    private void DisplayUpdate(int productTypeId, int idx)
    {
        this.productTypeId = productTypeId;
        this.idx = idx;

        imgCpiRangeUpdate.SetActive(true);

        dtmCpiRangeUpdate.ClearElements();
        dtmCpiRangeUpdate.PopulateClass(cpiRanges[productTypeId][idx]);
    }

    public void UpdateCpiRange()
    {
        if (!dtmCpiRangeUpdate.ValidateElements())
            return;

        int id = cpiRanges[productTypeId][idx].Id;
        //productTypeId = cpiRanges[productTypeId][idx].ProductTypeId;

        cpiRanges[productTypeId][idx] = dtmCpiRangeUpdate.BuildClass<CpiRange>();

        cpiRanges[productTypeId][idx].Id = id;
        cpiRanges[productTypeId][idx].ProductTypeId = productTypeId;

        cpiRanges[productTypeId].Sort((cr1, cr2) => { return cr1.AmountMin.CompareTo(cr2.AmountMin); });

        dtmCpiRangeUpdate.ClearElements();

        Refresh(productTypeId);
    }

    // Remove
    public void RemoveFractionated(int idx) => Remove(1, idx);
    public void RemoveFinanced(int idx) => Remove(2, idx);
    public void RemovePrepaid(int idx) => Remove(3, idx);

    private void Remove(int productTypeId, int idx)
    {
        this.productTypeId = productTypeId;
        cpiRanges[productTypeId].RemoveAt(idx);

        Refresh(productTypeId);
    }

    // Refresh
    private void Refresh(int productTypeId)
    {
        lstCpiRange[productTypeId].Clear();

        for (int i = 0; i < cpiRanges[productTypeId].Count; i++)
        {
            ListScrollerValue scrollerValue = new ListScrollerValue(4, true);
            scrollerValue.SetText(0, cpiRanges[productTypeId][i].AmountMin.ToString("N0", cultureInfo));
            scrollerValue.SetText(1, cpiRanges[productTypeId][i].AmountMax.ToString("N0", cultureInfo));
            scrollerValue.SetText(2, GetStringDouble(cpiRanges[productTypeId][i].DiscountRate, 1));
            scrollerValue.SetText(3, GetStringDouble(cpiRanges[productTypeId][i].ProfitablityRate, 1));

            lstCpiRange[productTypeId].AddValue(scrollerValue);
        }

        lstCpiRange[productTypeId].ApplyValues();

        imgCpiRangeAdd.SetActive(false);
        imgCpiRangeUpdate.SetActive(false);

        this.productTypeId = -1;
        idx = -1;
    }

    private string GetStringDouble(double value, int decimals = 0)
    {
        return $"{value.ToString($"N{decimals}", cultureInfo)} %";
    }
}