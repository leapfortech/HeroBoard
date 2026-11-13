using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Leap.Data.Mapper;
using Leap.UI.Elements;
using Leap.UI.Dialog;

using Sirenix.OdinInspector;


public class PuzzleChangeAction : MonoBehaviour
{
    [Serializable]
    public class PuzzleEventEvent : UnityEvent<int, PuzzleFull> { }

    [Title("Params")]
    [SerializeField]
    long puzzleGameId = -1;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmPuzzleFull = null;

    [Title("Fields")]
    [SerializeField]
    InputField ifdAnswerOk = null;
    [SerializeField]
    InputField ifdAnswerNok1 = null;
    [SerializeField]
    InputField ifdAnswerNok2 = null;
    [SerializeField]
    GameObject imgPuzzleChange = null;

    [Title("Actions")]
    [SerializeField]
    Button btnChange = null;

    [Title("Event")]
    [SerializeField]
    UnityEvent onRegistered = null;
    [SerializeField]
    PuzzleEventEvent onUpdated = null;

    PuzzleService puzzleService = null;

    PuzzleFull puzzleFullUpdate = null;

    //PuzzleFull puzzleFullRequest = null;
    int idx = -1;

    private void Awake()
    {
        puzzleService = GetComponent<PuzzleService>();
    }

    // Clear
    public void Clear()
    {
        dtmPuzzleFull.ClearElements();
        ifdAnswerOk.Clear();
        ifdAnswerNok1.Clear();
        ifdAnswerNok2.Clear();
    }

    // Set
    public void ChangePuzzle()
    {
        if (btnChange.Title[0] == 'A')
            RegisterPuzzle();
        else
            UpdatePuzzle();
    }

    // Add

    public void DisplayAdd()
    {
        btnChange.Title = "Agregar";

        Clear();

        imgPuzzleChange.gameObject.SetActive(true);
    }


    private void RegisterPuzzle()
    {
        if (!dtmPuzzleFull.ValidateElements() || !ElementHelper.Validate(ifdAnswerOk) || !ElementHelper.Validate(ifdAnswerNok1) || !ElementHelper.Validate(ifdAnswerNok2))
            return;

        ScreenDialog.Instance.Display();

        //PostFull postFull = new PostFull() {PostTypeId = 8};

        PuzzleFull puzzleFull = dtmPuzzleFull.BuildClass<PuzzleFull>();
        puzzleFull.PuzzleGameId = puzzleGameId;
        puzzleFull.PostTypeId = 8;

        puzzleFull.PuzzleAnswerFulls = new PuzzleAnswerFull[]
        {
            new() { Description = ifdAnswerOk.Text, IsCorrect = 1 },
            new() { Description = ifdAnswerNok1.Text, IsCorrect = 0 },
            new() { Description = ifdAnswerNok2.Text, IsCorrect = 0 }
        };

        puzzleService.Register(puzzleFull);
    }

    public void ApplyRegister(long puzzleId)
    {
        ChoiceDialog.Instance.Info("Nuevo reto", "Reto agregado exitosamente.", () => CloseModal(true));
    }

    // Update

    public void DisplayUpdate(int index, PuzzleFull puzzleFull)
    {
        puzzleFullUpdate = puzzleFull;
        idx = index;

        btnChange.Title = "Guardar";

        dtmPuzzleFull.PopulateClass<PuzzleFull>(puzzleFull);
        ifdAnswerOk.Text = puzzleFull.PuzzleAnswerFulls[0].Description;
        ifdAnswerNok1.Text = puzzleFull.PuzzleAnswerFulls[1].Description;
        ifdAnswerNok2.Text = puzzleFull.PuzzleAnswerFulls[2].Description;

        ifdAnswerOk.Revalidate(true);
        ifdAnswerNok1.Revalidate(true);
        ifdAnswerNok2.Revalidate(true);

        imgPuzzleChange.gameObject.SetActive(true);
    }


    private void UpdatePuzzle()
    {
        if (!dtmPuzzleFull.ValidateElements() || !ElementHelper.Validate(ifdAnswerOk) || !ElementHelper.Validate(ifdAnswerNok1) || !ElementHelper.Validate(ifdAnswerNok2))
            return;

        ScreenDialog.Instance.Display();

        puzzleFullUpdate.Update(dtmPuzzleFull.BuildClass<PuzzleFull>());

        if (puzzleFullUpdate.PuzzleAnswerFulls == null)
            puzzleFullUpdate.PuzzleAnswerFulls = new PuzzleAnswerFull[0];

        PuzzleAnswerFull[] puzzleAnswerFulls = new PuzzleAnswerFull[]
        {
            new()
            {
                Id = puzzleFullUpdate.PuzzleAnswerFulls.Length > 0 ? puzzleFullUpdate.PuzzleAnswerFulls[0].Id : -1,
                Description = ifdAnswerOk.Text,
                IsCorrect = 1,
                Status = 1
            },
            new()
            {
                Id = puzzleFullUpdate.PuzzleAnswerFulls.Length > 1 ? puzzleFullUpdate.PuzzleAnswerFulls[1].Id : -1,
                Description = ifdAnswerNok1.Text,
                IsCorrect = 0,
                Status = 1
            },
            new()
            {
                Id = puzzleFullUpdate.PuzzleAnswerFulls.Length > 2 ? puzzleFullUpdate.PuzzleAnswerFulls[2].Id : -1,
                Description = ifdAnswerNok2.Text,
                IsCorrect = 0,
                Status = 1
            }
        };

        puzzleFullUpdate.PuzzleAnswerFulls = puzzleAnswerFulls;

        puzzleService.UpdatePuzzle(puzzleFullUpdate);
    }

    public void ApplyUpdate(bool response)
    {
        ChoiceDialog.Instance.Info("Actualización de reto", "Reto actualizado exitosamente.", () => CloseModal(false));
    }

    private void CloseModal(bool isRegister)
    {
        imgPuzzleChange.gameObject.SetActive(false);

        if (isRegister)
            onRegistered.Invoke();
        else
            onUpdated.Invoke(idx, puzzleFullUpdate);
    }
}