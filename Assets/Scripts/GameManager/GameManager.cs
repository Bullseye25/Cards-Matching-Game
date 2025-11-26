using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField][Range(0, 2)] private int gameLevel;
    [SerializeField] private GameState gameState = GameState.AVAILABLE;

    [SerializeField] private ScoringSystem scoringSystem;

    private int count = 0;
    private List<CardManager> cards = new List<CardManager>();

    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject gameWonScreen;

    public GameState GameState
    {
        get => gameState;
        set => gameState = value;
    }

    public int GameLevel
    {
        get => gameLevel;
        private set => gameLevel = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Safety check
        if (scoringSystem == null)
        {
            Debug.LogError("GameManager: ScoringSystem reference is missing!");
        }

        ChangeGameLevel(1); //default
    }

    private void Start()
    {
        scoringSystem.ResetAttempts();
        scoringSystem.ResetMatchCount();
        scoringSystem.GetBestScore(gameLevel);
    }

    public void SetGameOccupied(CardManager card)
    {
        if (GameState == GameState.OCCUPIED)
            return;

        count++;
        cards.Add(card);

        if (count > 1)
        {
            GameState = GameState.OCCUPIED;
            StartCoroutine(ChangeStatus());
        }
    }

    private IEnumerator ChangeStatus()
    {
        loading.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        if (cards[0].CardID != cards[1].CardID)
        {
            cards.ForEach(card => card.Flip(0));
        }
        else
        {
            cards.ForEach(card => card.IsFound = true);
            scoringSystem.AddMatch();
            UIManager.Instance.UpdateMatchUI();
        }

        scoringSystem.AddWrongAttempt();
        UIManager.Instance.UpdateWrongAttemptsUI();

        cards.Clear();
        count = 0;

        yield return new WaitForSeconds(0.5f);

        loading.SetActive(false);

        if (IsGameOver())
        {
            Debug.Log("GAME OVER");
            scoringSystem.SaveBestScore(gameLevel);
            gameWonScreen.SetActive(true);
        }
        else
        {
            GameState = GameState.AVAILABLE;
        }
    }

    private bool IsGameOver()
    {
        var totalPairs = GridSystemManager.Instance.GetActiveSettings().GetTotalPairs();
        return scoringSystem.GetMatchCount() >= totalPairs;
    }

    public void ChangeGameLevel(int value)
    {
        GameLevel = value;
    }
}