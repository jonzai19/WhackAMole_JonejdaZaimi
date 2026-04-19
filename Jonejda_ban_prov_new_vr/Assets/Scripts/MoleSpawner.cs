using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawnt 9 Würmer in einem 3x3-Raster (Whack-a-Mole Layout)
/// und weist jedem das MoleMovement-Script zu.
/// </summary>
public class MoleSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [Tooltip("Das Wurm-Prefab (muss MoleMovement-Script haben oder es wird automatisch hinzugefügt)")]
    [SerializeField] private GameObject molePrefab;

    [Header("Raster Einstellungen")]
    [SerializeField] private int columns = 3;           // Spalten
    [SerializeField] private int rows = 3;              // Reihen
    [SerializeField] private float spacingX = 0.5f;    // Abstand in X-Richtung
    [SerializeField] private float spacingZ = 0.5f;    // Abstand in Z-Richtung
    [SerializeField] private float spawnOffsetY = 0f;  // Y-Versatz (negativ = tiefer)

    [Header("Bewegungseinstellungen (für alle Würmer)")]
    [SerializeField] private float moveDistance = 0.3f;     // Wie weit der Wurm hochkommt
    [SerializeField] private float moveSpeed = 1.5f;        // Bewegungsgeschwindigkeit
    [SerializeField] private float minWaitDown = 1f;        // Min. Wartezeit unten
    [SerializeField] private float maxWaitDown = 3f;        // Max. Wartezeit unten
    [SerializeField] private float minWaitUp = 0.5f;        // Min. Wartezeit oben
    [SerializeField] private float maxWaitUp = 2f;          // Max. Wartezeit oben

    [Header("Gleichzeitige Würmer begrenzen")]
    [Tooltip("Wie viele Würmer dürfen gleichzeitig oben sein")]
    [SerializeField] private int maxMolesUp = 2;

    private List<MoleMovement> allMoles = new List<MoleMovement>();

    void Start()
    {
        // Würmer werden erst gespawnt wenn der Poke-Button gedrückt wird
        if (molePrefab == null)
            Debug.LogError("MoleSpawner: Kein Prefab zugewiesen! Bitte molePrefab im Inspector setzen.");
    }

    /// <summary>
    /// Diese Methode dem Poke-Button (UnityEvent „OnClick" / „OnPokeDown") zuweisen.
    /// </summary>
    public void StartGame()
    {
        if (molePrefab == null)
        {
            Debug.LogError("MoleSpawner: Kein Prefab zugewiesen!");
            return;
        }

        // Verhindert doppeltes Starten
        if (allMoles.Count > 0)
        {
            Debug.LogWarning("MoleSpawner: Spiel läuft bereits!");
            return;
        }

        SpawnMoles();
        StartCoroutine(MoleController());
        Debug.Log("MoleSpawner: Spiel gestartet!");
    }

    void SpawnMoles()
    {
        // Mittelpunkt des Rasters berechnen, damit es zentriert spawnt
        float totalWidth  = (columns - 1) * spacingX;
        float totalDepth  = (rows - 1) * spacingZ;
        Vector3 offset = new Vector3(-totalWidth / 2f, 0f, -totalDepth / 2f);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Position im Raster (spawnOffsetY verschiebt alle Würmer nach oben/unten)
                Vector3 localPos = offset + new Vector3(col * spacingX, spawnOffsetY, row * spacingZ);
                Vector3 worldPos = transform.position + transform.TransformDirection(localPos);

                // Wurm instantiieren mit X-Rotation -180 (damit er richtig ausgerichtet ist)
                Quaternion spawnRotation = Quaternion.Euler(-180f, 0f, 0f);
                GameObject mole = Instantiate(molePrefab, worldPos, spawnRotation, transform);
                mole.name = $"Mole_{row * columns + col + 1}";

                // MoleMovement-Script holen oder hinzufügen
                MoleMovement movement = mole.GetComponent<MoleMovement>();
                if (movement == null)
                    movement = mole.AddComponent<MoleMovement>();

                // Einstellungen per Reflection übernehmen (einfacher: public Methode)
                movement.SetSettings(moveDistance, moveSpeed, minWaitDown, maxWaitDown, minWaitUp, maxWaitUp);
                allMoles.Add(movement);
            }
        }

        Debug.Log($"MoleSpawner: {allMoles.Count} Würmer gespawnt.");
    }

    /// <summary>
    /// Steuert wie viele Würmer gleichzeitig hochkommen dürfen.
    /// Wählt zufällig einen Wurm aus und lässt ihn hoch, wenn Platz ist.
    /// </summary>
    IEnumerator MoleController()
    {
        // Kurz warten damit alle Start()-Methoden der Moles laufen
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            // Zähle wie viele gerade oben sind
            int upCount = 0;
            foreach (var m in allMoles)
                if (m.IsUp || (!m.IsHit && m.IsUp)) upCount++;

            // Wenn noch Platz ist, einen zufälligen unten-Wurm nach oben schicken
            if (upCount < maxMolesUp)
            {
                // Alle Würmer die gerade unten warten sammeln
                List<MoleMovement> available = new List<MoleMovement>();
                foreach (var m in allMoles)
                    if (!m.IsUp && !m.IsHit)
                        available.Add(m);

                if (available.Count > 0)
                {
                    // Zufälligen auswählen und hochschicken
                    MoleMovement chosen = available[Random.Range(0, available.Count)];
                    chosen.PopUp();
                }
            }

            // Kurz warten bevor wir wieder prüfen
            yield return new WaitForSeconds(Random.Range(minWaitDown * 0.5f, maxWaitDown * 0.5f));
        }
    }
}
