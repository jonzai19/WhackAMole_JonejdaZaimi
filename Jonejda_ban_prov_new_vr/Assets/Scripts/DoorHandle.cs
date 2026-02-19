using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DoorHandle : MonoBehaviour
{
    // Referenz zum DoorOpener (sollte das Tür-Öffnen implementieren)
    public DoorOpener door;
    // Winkel (in Grad) der Klinke, wenn sie nach unten gedrückt wird
    public float handleDownAngle = -30f;
    // Geschwindigkeit des Zurückeffekts
    public float handleSpeed = 5f;

    // Startrotation der Klinke
    private Quaternion startRotation;
    // Zielrotation
    private Quaternion downRotation;
    private bool pressed = false;
    // Shortcut zur XRSimpleInteractable-Komponente am selben GameObject
    private XRSimpleInteractable simpleInteractable;

    void Awake()
    {
        // Initialwerte sichern
        startRotation = transform.localRotation;
        var e = transform.localEulerAngles;
        // downRotation basiert auf dem aktuellen Euler-Winkel, ersetzt nur die Y-Komponente
        downRotation = Quaternion.Euler(e.x, handleDownAngle, e.z);
        simpleInteractable = GetComponent<XRSimpleInteractable>();
    }

    void OnEnable()
    {
        // Event-Handler registrieren – wenn ein Interactor die Klinke auswählt/verlässt
        simpleInteractable?.selectEntered.AddListener(OnSelectEntered);
        simpleInteractable?.selectExited.AddListener(OnSelectExited);
    }

    void OnDisable()
    {
        // Event-Handler entfernen
        simpleInteractable?.selectEntered.RemoveListener(OnSelectEntered);
        simpleInteractable?.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        pressed = true; // Zustand setzen, damit Update die Klinke nach unten rotiert
        door?.OpenDoor(); // Tür öffnen
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        pressed = false; // Klinke soll wieder in Ausgangsposition zurückkehren
    }

    void Update()
    {
        // Interpolation zwischen Start- und Zielrotation
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            pressed ? downRotation : startRotation,
            Time.deltaTime * handleSpeed
        );
    }
}
