using System;
using System.Collections.Generic;

public class PuzzleFull : PostFull
{
    public long Id { get; set; } = -1L;
    public long PuzzleGameId { get; set; } = -1L;
    public long CountryId { get; set; } = -1L;
    public String Question { get; set; } = null;
    public String Hint { get; set; } = null;
    public int Difficulty { get; set; } = -1;
    public int Delay { get; set; } = 0;
    public int Points { get; set; } = 0;
    public int PlayCount { get; set; } = 0;
    public int Status { get; set; } = -1;

    public PuzzleAnswerFull[] PuzzleAnswerFulls { get; set; } = null;


    public PuzzleFull()
    {
    }

    public PuzzleFull(long id, long postId, long appUserId, String appUserAlias,
                        long postTypeId, long postCountryId, long postStateId,
                        String title, String titleImage, String description,
                        int imageCount, int favoriteCount, int[] reactionCounts, long reactionPhraseId, int commentCount,
                        DateTime publicationDateTime, int postStatus,
                        AppUserInfo appUserInfo, ContactFull contactFull, List<LinkFull> linkFulls, List<CommentFull> commentFulls, String[] images,
                        long puzzleGameId, long countryId, String question, String hint,
                        int difficulty, int delay, int points, int playCount, int status,
                        PuzzleAnswerFull[] puzzleAnswerFulls)
        : base(postId, appUserId, appUserAlias, postTypeId, postCountryId, postStateId, title, titleImage, description,
               imageCount, favoriteCount, reactionCounts, reactionPhraseId, commentCount, publicationDateTime, postStatus,
               appUserInfo, contactFull, linkFulls, commentFulls, images)
    {
        Id = id;
        PuzzleGameId = puzzleGameId;
        CountryId = countryId;
        Question = question;
        Hint = hint;
        Difficulty = difficulty;
        Delay = delay;
        Points = points;
        PlayCount = playCount;
        Status = status;

        PuzzleAnswerFulls = puzzleAnswerFulls ?? new PuzzleAnswerFull[0];
    }

    public void Update(PuzzleFull puzzleFull)
    {
        CountryId = puzzleFull.CountryId;
        Question = puzzleFull.Question;
        Hint = puzzleFull.Hint;
        Difficulty = puzzleFull.Difficulty;
        Delay = puzzleFull.Delay;
        Points = puzzleFull.Points;
    }
}
