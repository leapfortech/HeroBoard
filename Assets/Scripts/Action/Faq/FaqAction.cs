using UnityEngine;

using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.Data.Collections;

using Sirenix.OdinInspector;
using System.Collections.Generic;
using Leap.Data.Mapper;


public class FaqAction : MonoBehaviour
{
    [Title("Elements")]
    [SerializeField]
    ElementValue[] newFaqElementValues = null;
    [SerializeField]
    ElementValue[] updateFaqElementValues = null;

    [Title("Lists")]
    [SerializeField]
    ListScroller lstFaqType = null;
    [SerializeField]
    Text txtFaqTypeEmpty = null;
    [SerializeField]
    ListScroller lstFaq = null;
    [SerializeField]
    Text txtFaqEmpty = null;

    [Title("Value")]
    [SerializeField]
    ValueList vllFaqType = null;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmFaq = null;
    [SerializeField]
    DataMapper dtmNewFaq = null;

    [Title("Action")]
    [SerializeField]
    Button btnRegister = null;
    [SerializeField]
    Button btnUpdate = null;
    [SerializeField]
    Button btnDelete = null;

    FaqService faqService = null;

    Dictionary<long, List<Faq>> faqs = new Dictionary<long, List<Faq>>();

    int faqIdx = -1;
    long faqTypeId = -1L;
    Faq newFaq = null;

    private void Awake()
    {
        faqService = GetComponent<FaqService>();
    }

    private void Start()
    {
        btnRegister?.AddAction(NewFaq);
        btnUpdate?.AddAction(UpdateFaq);
        btnDelete?.AddAction(AskDeleteFaq);
    }

    public void Clear()
    {
        dtmFaq.ClearElements();
        lstFaqType.Clear();
        lstFaq.Clear();
        faqs.Clear();
        faqIdx = -1;
        faqTypeId = -1;
        newFaq = null;
    }

    public void FillTypes()
    {
        txtFaqTypeEmpty.gameObject.SetActive(vllFaqType.RecordCount == 0);

        lstFaqType.ClearValues();

        for (int i = 0; i < vllFaqType.RecordCount; i++)
        {
            ListScrollerValue value = new ListScrollerValue(lstFaqType.ListItem, true);

            value.SetText(0, vllFaqType.FindRecordCellString(i + 1, "Name"));

            lstFaqType.AddValue(value);
        }

        DisplayQuestions(0);

        lstFaqType.ApplyValues();
    }


    public void DisplayQuestions(int idx)
    {
        faqTypeId = idx + 1;
        
        if (faqs.ContainsKey(faqTypeId))
        {
            FillQuestions(faqs[faqTypeId]);
            return;
        }

        ScreenDialog.Instance.Display();

        faqService.GetAllByType(faqTypeId);
    }

    public void FillQuestions(List<Faq> faqsType)
    {
        dtmFaq.ClearElements();
        
        faqs[faqTypeId] = faqsType;

        if (faqsType == null || faqsType.Count == 0)
        {
            ShowEmpty();
            return;
        }

        lstFaq.ClearValues();

        txtFaqEmpty.gameObject.SetActive(false);

        for (int i = 0; i < faqsType.Count; i++)
        {
            ListScrollerValue value = new ListScrollerValue(lstFaq.ListItem, true);

            value.SetText(0, faqsType[i].Question);

            lstFaq.AddValue(value);
        }

        lstFaq.ApplyValues();

        if (faqIdx >= 0 && faqIdx < faqsType.Count)
            lstFaq.CheckToggle(faqIdx, true);
        else
            lstFaq.CheckToggle(0, true);

        StateManager.Instance.BoardLoadHide();
    }

    void ShowEmpty()
    {
        txtFaqEmpty.gameObject.SetActive(true);
        lstFaq.ApplyClearValues();

        dtmFaq.ClearElements();

        StateManager.Instance.BoardLoadHide();
    }

    public void Display(int idx)
    {
        faqIdx = idx;
        dtmFaq.PopulateClass(faqs[faqTypeId][idx]);
    }

    // New
    public void NewFaq()
    {
        if (!ElementHelper.Validate(newFaqElementValues))
            return;

        ScreenDialog.Instance.Display();
        newFaq = dtmNewFaq.BuildClass<Faq>();
        newFaq.BoardUserId = StateManager.Instance.BoardUser.Id;

        faqService.Register(newFaq);
    }

    public void ApplyNewFaq(long faqId)
    {
        newFaq.Status = 1;
        newFaq.Id = faqId;

        if (!faqs.ContainsKey(faqTypeId))
            faqs[faqTypeId] = new List<Faq>();

        faqs[faqTypeId].Add(newFaq);

        dtmNewFaq.ClearElements();

        faqIdx = faqs[faqTypeId].Count - 1;

        DisplayQuestions((int)faqTypeId - 1);

        ChoiceDialog.Instance.Info("Nueva pregunta", "Pregunta registrada satisfactoriamente.");

        StateManager.Instance.BoardLoadHide();
    }

    // Update
    public void UpdateFaq()
    {
        if (!ElementHelper.Validate(updateFaqElementValues))
            return;

        ScreenDialog.Instance.Display();
        newFaq = dtmFaq.BuildClass<Faq>();

        Faq faq = faqs[faqTypeId][faqIdx];
        newFaq.Id = faq.Id;
        newFaq.FaqTypeId = faq.FaqTypeId;
        newFaq.BoardUserId = StateManager.Instance.BoardUser.Id;

        faqService.UpdateFaq(newFaq);
    }

    public void ApplyUpdateFaq(long faqId)
    {
        faqs[faqTypeId][faqIdx] = newFaq;

        DisplayQuestions((int)faqTypeId - 1);

        ChoiceDialog.Instance.Info("Actualizar pregunta", "Pregunta actualizada satisfactoriamente.");

        StateManager.Instance.BoardLoadHide();
    }

    // Delete
    public void AskDeleteFaq()
    {
        ChoiceDialog.Instance.Info("Eliminar pregunta", "¿Estás seguro de eliminar la pregunta?", DeleteFaq, null, "Sí", "No");
        return;
    }


    public void DeleteFaq()
    {
        ScreenDialog.Instance.Display();

        Faq faq = faqs[faqTypeId][faqIdx];

        faqService.UpdateStatus(faq.Id, 0);
    }

    public void ApplyDeleteFaq(bool response)
    {
        faqs[faqTypeId].RemoveAt(faqIdx);

        if (faqs[faqTypeId].Count == 0)
            ShowEmpty();
        else
        {
            faqIdx = 0;
            DisplayQuestions((int)faqTypeId - 1);
        }

        ChoiceDialog.Instance.Info("Eliminar pregunta", "Pregunta eliminada satisfactoriamente.");

        StateManager.Instance.BoardLoadHide();
    }
}