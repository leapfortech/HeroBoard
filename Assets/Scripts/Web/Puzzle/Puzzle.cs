using System;

public class Puzzle
{
    public long Id { get; set; }
    public long PostId { get; set; }
    public long PuzzleSubtypeId { get; set; }
    public long CountryId { get; set; }
    public String Question { get; set; }
    public String Hint { get; set; }
    public int Difficulty { get; set; }
    public int Delay { get; set; }
    public int Points { get; set; }
    public int PlayCount { get; set; }
    public DateTime CreateDateTime { get; set; }
    public int Status { get; set; }

    public Puzzle() { }

    public Puzzle(long id, long postId, long puzzleSubtypeId, long countryId, String question, String hint,
                    int difficulty, int delay, int points, int playCount, DateTime createDateTime, int status)
    {
        Id = id;
        PostId = postId;
        PuzzleSubtypeId = puzzleSubtypeId;
        CountryId = countryId;
        Question = question;
        Hint = hint;
        Difficulty = difficulty;
        Delay = delay;
        Points = points;
        PlayCount = playCount;
        CreateDateTime = createDateTime;
        Status = status;
    }

    public Puzzle(PuzzleFull puzzleFull)
    {
        Id = puzzleFull.Id;
        PostId = puzzleFull.PostId;
        PuzzleSubtypeId = puzzleFull.PuzzleSubtypeId;
        CountryId = puzzleFull.CountryId;
        Question = puzzleFull.Question;
        Hint = puzzleFull.Hint;
        Difficulty = puzzleFull.Difficulty;
        Delay = puzzleFull.Delay;
        Points = puzzleFull.Points;
        PlayCount = puzzleFull.PlayCount;
        Status = puzzleFull.Status;
    }

    public void Update(Puzzle puzzle)
    {
        PuzzleSubtypeId = puzzle.PuzzleSubtypeId;
        CountryId = puzzle.CountryId;
        Question = puzzle.Question;
        Hint = puzzle.Hint;
        Difficulty = puzzle.Difficulty;
        Delay = puzzle.Delay;
        Points = puzzle.Points;
        PlayCount = puzzle.PlayCount;
        CreateDateTime = puzzle.CreateDateTime;
    }
}
