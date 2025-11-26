using UnityEngine;

public class ScoringSystem : SaveAndLoadSystem
{
    private int wrongAttempts = 0;
    private int matchCount = 0; // <-- runtime only, no saving

    private const string SCORE = "BestScore_Level_";

    // ===============================
    // Wrong Attempts
    // ===============================

    public void AddWrongAttempt()
    {
        wrongAttempts++;
    }

    public int GetCurrentAttemptCount()
    {
        return wrongAttempts;
    }

    public void ResetAttempts()
    {
        wrongAttempts = 0;
    }

    // ===============================
    // Match Count (runtime only)
    // ===============================

    /// <summary>
    /// Called when the player successfully matches a pair.
    /// This value is NOT saved, only exists during the game session.
    /// </summary>
    public void AddMatch()
    {
        matchCount++;
    }

    /// <summary>
    /// Gets the number of matches found in the current game.
    /// </summary>
    public int GetMatchCount()
    {
        return matchCount;
    }

    /// <summary>
    /// Reset match counter when a new game/level starts.
    /// </summary>
    public void ResetMatchCount()
    {
        matchCount = 0;
    }

    // ===============================
    // Saving Best Score (per level)
    // Lower mistakes = better score.
    // ===============================

    public void SaveBestScore(int level)
    {
        string key = $"{SCORE}{level}";

        if (!HasKey(key))
        {
            SaveInt(key, wrongAttempts);
            return;
        }

        int prevBest = LoadInt(key);

        // Lower mistakes = better score
        if (wrongAttempts < prevBest)
        {
            SaveInt(key, wrongAttempts);
        }
    }

    public int GetBestScore(int level)
    {
        string key = $"{SCORE}{level}";
        return HasKey(key) ? LoadInt(key) : -1;
    }
}