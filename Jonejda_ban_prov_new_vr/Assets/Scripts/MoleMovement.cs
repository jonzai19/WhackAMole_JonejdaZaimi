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
    
    [Header("Treffer-Einstellungen")]
    [Tooltip("Tag des Hammers (muss am Hammer-Objekt gesetzt sein)")]
    [SerializeField] private string hammerTag = "Hammer";
    [Tooltip("Wie schnell der Wurm nach einem Treffer nach unten geht")]
    [SerializeField] private float hitSpeed = 4f;

    private Vector3 startPosition;
    private Vector3 zielPosition;
    private float waitTimer;
    private bool isUp = false;
    private bool isMovingUp = false;
    private bool isMovingDown = false;
    private bool isHit = false;
    private bool waitingUp = false; // Wartet oben bevor er runtergeht

    // MoleSpawner kann diesen Zustand abfragen
    public bool IsUp => isUp;
    public bool IsHit => isHit;

    /// <summary>
    /// Wird vom MoleSpawner aufgerufen, um die Einstellungen zu setzen (vor Start).
    /// </summary>
    public void SetSettings(float dist, float spd, float minDown, float maxDown, float minUp, float maxUp)
    {
        distance          = dist;
        speed             = spd;
        minWarteZeitUnten = minDown;
        maxWarteZeitUnten = maxDown;
        minWarteZeitOben  = minUp;
        maxWarteZeitOben  = maxUp;
    }

    /// <summary>
    /// Vom MoleSpawner aufgerufen: Wurm soll hochkommen.
    /// </summary>
    public void PopUp()
    {
        if (isUp || isMovingUp || isHit) return;
        isMovingUp = true;
        isMovingDown = false;
    }

    void Start()
    {
        // Startposition speichern
        startPosition = transform.localPosition;
        // Zielposition berechnen (oben)
        zielPosition = startPosition + Vector3.up * distance;
    }

    void Update()
    {
        // Treffer-Animation: sofort nach unten
        if (isHit)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPosition, hitSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, startPosition) < 0.01f)
            {
                transform.localPosition = startPosition;
                isHit = false;
                isUp = false;
                isMovingDown = false;
            }
            return; // Normale Bewegung während Treffer ignorieren
        }

        // Nach oben bewegen
        if (isMovingUp)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, zielPosition, speed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, zielPosition) < 0.01f)
            {
                transform.localPosition = zielPosition;
                isMovingUp = false;
                isUp = true;
                waitingUp = true;
                waitTimer = Random.Range(minWarteZeitOben, maxWarteZeitOben);
            }
            return;
        }

        // Oben warten
        if (waitingUp)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                waitingUp = false;
                isMovingDown = true;
            }
            return;
        }

        // Nach unten bewegen
        if (isMovingDown)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPosition, speed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, startPosition) < 0.01f)
            {
                transform.localPosition = startPosition;
                isMovingDown = false;
                isUp = false;
            }
        }
    }

    /// <summary>
    /// Wird vom HammerHit-Script aufgerufen wenn der Hammer den Wurm trifft.
    /// </summary>
    public void RegisterHit()
    {
        // Treffer erlaubt wenn der Wurm oben ist ODER noch nach oben fährt
        if (isHit) return;
        if (!isUp && !isMovingUp) return;
        isHit = true;
        isMovingUp = false;
        isMovingDown = false;
        waitingUp = false;
        // Punkt vergeben
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.WurmGetroffen();
    }

    /// <summary>
    /// Fallback: Trigger-Kollision (nur falls Rigidbody + Collider vorhanden)
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(hammerTag))
            RegisterHit();
    }
}
