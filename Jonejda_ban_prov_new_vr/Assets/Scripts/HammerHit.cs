using UnityEngine;

/// <summary>
/// Auf den Hammer legen. Erkennt Würmer egal auf welchem Child-Objekt
/// der Collider sitzt – sucht MoleMovement immer im Eltern-Baum.
/// </summary>
public class HammerHit : MonoBehaviour
{
    [Header("Einstellungen")]
    [Tooltip("Radius der Trefferzone (roter Kreis im Scene-View)")]
    [SerializeField] private float hitRadius = 0.3f;
    [Tooltip("0 = immer aktiv")]
    [SerializeField] private float minSwingSpeed = 0f;

    [Header("Audio")]
    [Tooltip("Sound der abgespielt wird wenn ein Wurm getroffen wird")]
    [SerializeField] private AudioClip hitSound;
    [Tooltip("Lautstärke des Treffergeräusches")]
    [SerializeField] private float hitVolume = 1f;

    private float cooldown;
    private Vector3 lastPos;

    void Start() => lastPos = transform.position;

    // ── Methode 1: Trigger ──────────────────────────────────────────────────
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("[HammerHit] OnTriggerEnter: " + other.name);
        TryHit(other.transform);
    }

    // ── Methode 2: Normale Kollision ────────────────────────────────────────
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("[HammerHit] OnCollisionEnter: " + col.collider.name);
        TryHit(col.collider.transform);
    }

    // ── Methode 3: OverlapSphere Fallback ───────────────────────────────────
    void Update()
    {
        float speed = (transform.position - lastPos).magnitude / Time.deltaTime;
        lastPos = transform.position;

        if (cooldown > 0f) { cooldown -= Time.deltaTime; return; }
        if (speed < minSwingSpeed) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, hitRadius);
        foreach (Collider col in hits)
        {
            // Eigenen Collider überspringen
            if (col.transform == transform || col.transform.IsChildOf(transform)) continue;

            MoleMovement mole = col.GetComponentInParent<MoleMovement>();
            if (mole != null)
            {
                Debug.Log("[HammerHit] OverlapSphere trifft: " + col.name);
                TryHit(col.transform);
                break;
            }
        }
    }

    // ── Gemeinsame Treffer-Logik ────────────────────────────────────────────
    void TryHit(Transform hit)
    {
        if (cooldown > 0f) return;

        // Eigene Kinder ignorieren
        if (hit == transform || hit.IsChildOf(transform)) return;

        // MoleMovement im Eltern-Baum suchen (egal wie tief der Collider sitzt)
        MoleMovement mole = hit.GetComponentInParent<MoleMovement>();
        if (mole == null)
        {
            // Kein Wurm – ignorieren (kein Log-Spam)
            return;
        }

        Debug.Log("[HammerHit] Wurm getroffen: " + mole.name + " | IsUp=" + mole.IsUp);
        mole.RegisterHit();
        cooldown = 0.4f;

        // Treffergeräusch abspielen
        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, transform.position, hitVolume);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}
