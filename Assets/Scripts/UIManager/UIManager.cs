using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References (TextMeshPro)")]
    [SerializeField] private TMP_Text currentMatchesText;
    [SerializeField] private TMP_Text currentWrongAttemptsText;
    [SerializeField] private TMP_Text bestScoreText;

    [Space]
    [SerializeField] private ScoringSystem scoringSystem;

    private void Awake()
    {
        // Singleton Setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        scoringSystem = FindAnyObjectByType<ScoringSystem>();

        UpdateAllUI();
    }

    /// <summary>
    /// Updates all UI values.
    /// </summary>
    public void UpdateAllUI()
    {
        UpdateMatchUI();
        UpdateWrongAttemptsUI();
        UpdateBestScoreUI();
    }

    public void UpdateMatchUI()
    {
        if (currentMatchesText != null)
            currentMatchesText.text = scoringSystem.GetMatchCount().ToString();
    }

    public void UpdateWrongAttemptsUI()
    {
        if (currentWrongAttemptsText != null)
            currentWrongAttemptsText.text = scoringSystem.GetCurrentAttemptCount().ToString();
    }

    public void UpdateBestScoreUI()
    {
        if (bestScoreText != null)
        {
            int score = scoringSystem.GetBestScore(GameManager.Instance.GameLevel);
            bestScoreText.text = (score == -1 ? "--" : score.ToString());
        }
    }
}
