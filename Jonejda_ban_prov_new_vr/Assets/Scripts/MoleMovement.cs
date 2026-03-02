using UnityEngine;

public class MoleMovement : MonoBehaviour
{
    [Header("Bewegungseinstellungen")]
    [SerializeField] private float distance = 0.5f; // Wie weit der Wurm sich bewegt
    [SerializeField] private float speed = 1.5f; // Geschwindigkeit des Wurms
    
    [Header("Zeiteinstellungen")]
    [SerializeField] private float minWarteZeitUnten = 1f; // Minimale Zeit unten
    [SerializeField] private float maxWarteZeitUnten = 3f; // Maximale Zeit unten
    [SerializeField] private float minWarteZeitOben = 0.5f; // Minimale Zeit oben
    [SerializeField] private float maxWarteZeitOben = 2f; // Maximale Zeit oben
    
    private Vector3 startPosition;
    private Vector3 zielPosition;
    private float timer;
    private bool isUp = false;
    private bool isMoving = false;

    void Start()
    {
        // Startposition speichern
        startPosition = transform.localPosition;
        // Zielposition berechnen (oben)
        zielPosition = startPosition + Vector3.up * distance;
        
        // Zufällige Startwartezeit, damit nicht alle Würmer gleichzeitig starten
        timer = Random.Range(0f, maxWarteZeitUnten);
    }

    void Update()
    {
        if (isMoving)
        {
            // Wurm bewegt sich
            if (isUp)
            {
                // Nach unten bewegen
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPosition, speed * Time.deltaTime);
                
                // Wenn unten angekommen
                if (Vector3.Distance(transform.localPosition, startPosition) < 0.01f)
                {
                    transform.localPosition = startPosition;
                    isUp = false;
                    isMoving = false;
                    // Neue zufällige Wartezeit unten
                    timer = Random.Range(minWarteZeitUnten, maxWarteZeitUnten);
                }
            }
            else
            {
                // Nach oben bewegen
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, zielPosition, speed * Time.deltaTime);
                
                // Wenn oben angekommen
                if (Vector3.Distance(transform.localPosition, zielPosition) < 0.01f)
                {
                    transform.localPosition = zielPosition;
                    isUp = true;
                    isMoving = false;
                    // Neue zufällige Wartezeit oben
                    timer = Random.Range(minWarteZeitOben, maxWarteZeitOben);
                }
            }
        }
        else
        {
            // Wurm wartet
            timer -= Time.deltaTime;
            if (timer <= 0) isMoving = true;
        }
    }
}
