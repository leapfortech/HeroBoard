using System.Collections.Generic;
using UnityEngine;

using Leap.Data.Mapper;
using Leap.UI.Elements;
using Leap.UI.Dialog;

using Sirenix.OdinInspector;


public class PuzzleChangeAction : MonoBehaviour
{
    [Title("Fields")]
    [SerializeField]
    InputField ifdAnswerOk = null;
    [SerializeField]
    InputField ifdAnswerNok1 = null;
    [SerializeField]
    InputField ifdAnswerNok2 = null;
    [SerializeField]
    GameObject imgPuzzleChange = null;

    [Title("Data")]
    [SerializeField]
    DataMapper dtmPuzzle = null;

    [Title("Actions")]
    [SerializeField]
    Button btnChange = null;

    PuzzleService puzzleService = null;

    PuzzleInfo puzzleInfo = null;

    private void Awake()
    {
        puzzleService = GetComponent<PuzzleService>();
    }

    public void Clear()
    {
        dtmPuzzle.ClearElements();
        ifdAnswerOk.Clear();
        ifdAnswerNok1.Clear();
        ifdAnswerNok2.Clear();
    }

    public void ChangeUserBoard()
    {
        if (btnChange.Title[0] == 'A')
            RegisterPuzzle();
        else
            UpdatePuzzle();
    }

    public void SetPuzzle(PuzzleInfo info)
    {
        puzzleInfo = info;
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
        if (!dtmPuzzle.ValidateElements() || !ifdAnswerOk.Validate() || !ifdAnswerNok1.Validate() || !ifdAnswerNok2.Validate())
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
        ChoiceDialog.Instance.Info("Nuevo reto", "Reto agregado exitosamente.", () => CloseModal());
    }

    // Update

    public void DisplayUpdate()
    {
        btnChange.Title = "Guardar";

        dtmPuzzle.PopulateClass<Puzzle>(puzzleInfo.Puzzle);
        ifdAnswerOk.Text = puzzleInfo.PuzzleAnswers[0].Description;
        ifdAnswerNok1.Text = puzzleInfo.PuzzleAnswers[1].Description;
        ifdAnswerNok2.Text = puzzleInfo.PuzzleAnswers[2].Description;

        imgPuzzleChange.gameObject.SetActive(true);
    }


    private void UpdatePuzzle()
    {
        if (!dtmPuzzle.ValidateElements() || !ifdAnswerOk.Validate() || !ifdAnswerNok1.Validate() || !ifdAnswerNok2.Validate())
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

        puzzleService.UpdatePuzzle(new RegisterPuzzleRequest(new RegisterPostRequest(post, null, null, null),
                                                                                     puzzle, puzzleAnswers));
    }

    public void ApplyUpdate(bool response)
    {
        ChoiceDialog.Instance.Info("Actualización de reto", "Reto actualizado exitosamente.", () => CloseModal());
    }

    private void CloseModal()
    {
        imgPuzzleChange.gameObject.SetActive(false);
    }
}