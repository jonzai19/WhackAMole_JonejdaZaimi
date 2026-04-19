using UnityEngine;
using TMPro;

/// <summary>
/// Singleton-ScoreManager: verwaltet den Spielstand und zeigt ihn auf einem UI-Text an.
/// Ziehe das GameObject mit diesem Script in die Szene und weise ein TMP_Text-Element zu.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI")]
    [Tooltip("TMP Text-Element das den Score anzeigt")]
    [SerializeField] private TMP_Text scoreText;

    [Header("Punkte pro Treffer")]
    [SerializeField] private int punkte = 10;

    private int score = 0;

    void Awake()
    {
        // Singleton-Muster
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        AktualisierUI();
    }

    /// <summary>
    /// Wird vom MoleMovement aufgerufen wenn ein Wurm getroffen wird.
    /// </summary>
    public void WurmGetroffen()
    {
        score += punkte;
        AktualisierUI();
        Debug.Log($"Treffer! Punkte: {score}");
    }

    /// <summary>
    /// Setzt den Score zurück (z.B. beim Neustart).
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
    }
}

