using UnityEngine;
using TMPro;

/// <summary>
/// Singleton-ScoreManager: verwaltet den Spielstand und Highscore.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI")]
    [Tooltip("TMP Text-Element das den Score anzeigt")]
    [SerializeField] private TMP_Text scoreText;

    [Tooltip("TMP Text-Element das den Highscore anzeigt")]
    [SerializeField] private TMP_Text highscoreText;

    [Header("Punkte pro Treffer")]
    [SerializeField] private int punkte = 10;

    // PlayerPrefs-Schlüssel für den Highscore
    private const string HighscoreKey = "Highscore";

    private int score = 0;
    private int highscore = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // Highscore aus PlayerPrefs laden (bleibt auf der VR-Brille gespeichert)
        highscore = PlayerPrefs.GetInt(HighscoreKey, 0);
        AktualisierUI();
    }

    /// <summary>
    /// Wird vom MoleMovement aufgerufen wenn ein Wurm getroffen wird.
    /// </summary>
    public void WurmGetroffen()
    {
        score += punkte;

        // Highscore aktualisieren falls aktueller Score gleich oder höher ist
        if (score >= highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt(HighscoreKey, highscore);
            PlayerPrefs.Save();
        }

        AktualisierUI();
        Debug.Log($"Treffer! Punkte: {score} | Highscore: {highscore}");
    }

    /// <summary>
    /// Setzt den Score zurück (z.B. beim Neustart). Highscore bleibt erhalten.
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        AktualisierUI();
    }

    private void AktualisierUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";

        if (highscoreText != null)
            highscoreText.text = $"Highscore: {highscore}";
    }
}
