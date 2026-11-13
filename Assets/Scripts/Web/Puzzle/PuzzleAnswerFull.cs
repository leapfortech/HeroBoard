using System;

public class PuzzleAnswerFull
{
    public long Id { get; set; } = -1L;
    public String Description { get; set; } = null;
    public int IsCorrect { get; set; } = -1;
    public int Status { get; set; } = -1;

    public PuzzleAnswerFull()
    {
    }

    public PuzzleAnswerFull(long id, String description, int isCorrect, int status)
    {
        Id = id;
        Description = description;
        IsCorrect = isCorrect;
        Status = status;
    }
}
