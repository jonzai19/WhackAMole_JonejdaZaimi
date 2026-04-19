using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Startet automatisch einen 45-Sekunden-Countdown sobald die Szene geladen wird.
/// Danach wird zur angegebenen Szene gewechselt.
/// </summary>
public class GameTimer : MonoBehaviour
{
    [Header("Timer Einstellungen")]
    [Tooltip("Spielzeit in Sekunden")]
    [SerializeField] private float gameDuration = 45f;

    [Header("Szene")]
    [Tooltip("Name der Szene die nach dem Timer geladen wird")]
    [SerializeField] private string nextSceneName = "EndScene";

    [Header("UI (optional)")]
    [Tooltip("TMP Text der den Countdown anzeigt")]
    [SerializeField] private TMP_Text timerText;

    /// <summary>
    /// Dem Poke-Button UnityEvent zuweisen – startet den Timer.
    /// </summary>
    public void StartTimer()
    {
        if (running) return;
        running = true;
        StartCoroutine(CountDown());
        Debug.Log("[GameTimer] Timer started: " + gameDuration + "s");
    }

    private bool running = false;

    IEnumerator CountDown()
    {
        float timeLeft = gameDuration;

        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            if (timerText != null)
                timerText.text = "Time: " + Mathf.CeilToInt(timeLeft) + "s";
            yield return null;
        }

        if (timerText != null)
            timerText.text = "Time: 0s";

        Debug.Log("[GameTimer] Time ran out! Load Scene: " + nextSceneName);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextSceneName);
    }
}

