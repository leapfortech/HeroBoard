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
    public class PuzzleEventEvent : UnityEvent<int, PuzzleInfo> { }

    [Title("Data")]
    [SerializeField]
    DataMapper dtmPuzzle = null;

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

    PuzzleInfo puzzleInfo = null;

    PuzzleInfo puzzleInfoRequest = null;
    int idx = -1, statusRequest = -1;

    private void Awake()
    {
        puzzleService = GetComponent<PuzzleService>();
    }

    // Clear
    public void Clear()
    {
        dtmPuzzle.ClearElements();
        ifdAnswerOk.Clear();
        ifdAnswerNok1.Clear();
        ifdAnswerNok2.Clear();
    }

    // Set
    public void SetPuzzle(int index, PuzzleInfo info)
    {
        puzzleInfo = info;
        idx = index;
    }

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
        if (!dtmPuzzle.ValidateElements() || !ElementHelper.Validate(ifdAnswerOk) || !ElementHelper.Validate(ifdAnswerNok1) || !ElementHelper.Validate(ifdAnswerNok2))
            return;

        ScreenDialog.Instance.Display();

        Post post = new Post() {PostTypeId = 8};

        Puzzle puzzle = dtmPuzzle.BuildClass<Puzzle>();
        puzzle.PuzzleSubtypeId = 1;

        List<PuzzleAnswer> puzzleAnswers = new List<PuzzleAnswer>()
        {
            new PuzzleAnswer() {Description = ifdAnswerOk.Text, IsCorrect = 1},
            new PuzzleAnswer() {Description = ifdAnswerNok1.Text, IsCorrect = 0},
            new PuzzleAnswer() {Description = ifdAnswerNok2.Text, IsCorrect = 0}
        };

        puzzleService.Register(new RegisterPuzzleRequest(new RegisterPostRequest(post, null, null, null),
                                                         puzzle, puzzleAnswers));
    }

    public void ApplyRegister(long puzzleId)
    {
        ChoiceDialog.Instance.Info("Nuevo reto", "Reto agregado exitosamente.", () => CloseModal(true));
    }

    // Update

    public void DisplayUpdate()
    {
        btnChange.Title = "Guardar";

        dtmPuzzle.PopulateClass<Puzzle>(puzzleInfo.Puzzle);
        ifdAnswerOk.Text = puzzleInfo.PuzzleAnswers[0].Description;
        ifdAnswerNok1.Text = puzzleInfo.PuzzleAnswers[1].Description;
        ifdAnswerNok2.Text = puzzleInfo.PuzzleAnswers[2].Description;

        ifdAnswerOk.Revalidate(true);
        ifdAnswerNok1.Revalidate(true);
        ifdAnswerNok2.Revalidate(true);

        imgPuzzleChange.gameObject.SetActive(true);
    }


    private void UpdatePuzzle()
    {
        if (!dtmPuzzle.ValidateElements() || !ElementHelper.Validate(ifdAnswerOk) || !ElementHelper.Validate(ifdAnswerNok1) || !ElementHelper.Validate(ifdAnswerNok2))
            return;

        ScreenDialog.Instance.Display();

        Post post = puzzleInfo.Post;

        Puzzle puzzle = dtmPuzzle.BuildClass<Puzzle>();

        puzzle.Id = puzzleInfo.Puzzle.Id;
        puzzle.PostId = puzzleInfo.Puzzle.PostId;
        puzzle.PuzzleSubtypeId = puzzleInfo.Puzzle.PuzzleSubtypeId;
        puzzle.PlayCount = puzzleInfo.Puzzle.PlayCount;
        puzzle.Status = puzzleInfo.Puzzle.Status;

        List<PuzzleAnswer> puzzleAnswers = new List<PuzzleAnswer>()
        {
            new PuzzleAnswer()
            {
                Id = puzzleInfo.PuzzleAnswers.Count > 0 ? puzzleInfo.PuzzleAnswers[0].Id : -1,
                PuzzleId = puzzleInfo.Puzzle.Id,
                Description = ifdAnswerOk.Text,
                IsCorrect = 1,
                Status = 1
            },

            new PuzzleAnswer()
            {
                Id = puzzleInfo.PuzzleAnswers.Count > 1 ? puzzleInfo.PuzzleAnswers[1].Id : -1,
                PuzzleId = puzzleInfo.Puzzle.Id,
                Description = ifdAnswerNok1.Text,
                IsCorrect = 0,
                Status = 1
            },

            new PuzzleAnswer()
            {
                Id = puzzleInfo.PuzzleAnswers.Count > 2 ? puzzleInfo.PuzzleAnswers[2].Id : -1,
                PuzzleId = puzzleInfo.Puzzle.Id,
                Description = ifdAnswerNok2.Text,
                IsCorrect = 0,
                Status = 1
            }
        };

        puzzleInfoRequest = new PuzzleInfo(post, puzzle, puzzleAnswers);

        puzzleService.UpdatePuzzle(new RegisterPuzzleRequest(new RegisterPostRequest(post, null, null, null),
                                                                                     puzzle, puzzleAnswers));
    }

    public void ApplyUpdate(bool response)
    {
        puzzleInfo = puzzleInfoRequest;

        ChoiceDialog.Instance.Info("Actualización de reto", "Reto actualizado exitosamente.", () => CloseModal(false));
    }

    private void CloseModal(bool isRegister)
    {
        imgPuzzleChange.gameObject.SetActive(false);

        if (isRegister)
            onRegistered.Invoke();
        else
            onUpdated.Invoke(idx, puzzleInfo);
    }

    // UpdateStatus
    public void Deactivate()
    {
        ChoiceDialog.Instance.Error("Eliminar reto", "¿Estás seguro que deseas elimnar el reto?", () => UpdateStatus(0), null, "Sí" , "Regresar");
    }

    public void Activate()
    {
        ChoiceDialog.Instance.Info("Activar reto", "¿Estás seguro que deseas activar el reto?", () => UpdateStatus(1), null, "Sí", "Regresar");
    }

    private void UpdateStatus(int status)
    {
        statusRequest = status;

        ScreenDialog.Instance.Display();
        
        puzzleService.UpdateStatus(puzzleInfo.Post.Id, puzzleInfo.Puzzle.Id, status);
    }

    public void ApplyUpdateStatus(bool response)
    {
        puzzleInfo.Post.Status = statusRequest;
        puzzleInfo.Puzzle.Status = statusRequest;

        onUpdated.Invoke(idx, puzzleInfo);
    }
}