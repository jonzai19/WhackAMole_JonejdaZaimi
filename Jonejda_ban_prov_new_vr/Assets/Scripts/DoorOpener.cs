using System.Collections;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public float openAngle = 90f;     // Wie weit sich die Tür öffnet
    public float openSpeed = 2f;

    private bool shouldOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(
            transform.eulerAngles.x,
            transform.eulerAngles.y + openAngle,
            transform.eulerAngles.z
        );
    }

    void Update()
    {
        if (shouldOpen)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                openRotation,
                Time.deltaTime * openSpeed
            );
        }
    }

    public void OpenDoor()
    {
        shouldOpen = true;
    }
}

