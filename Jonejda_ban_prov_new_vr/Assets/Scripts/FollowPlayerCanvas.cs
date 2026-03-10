using UnityEngine;

public class FollowPlayerCanvas : MonoBehaviour
{
    public Transform playerCamera;
    public float distanceFromCamera = 2f;
    public float heightOffset = -0.2f;
    public float followSpeed = 5f;
    public float rotationSpeed = 5f;
    [SerializeField] GameObject panel;

    void LateUpdate()
    {
        if (playerCamera == null) return;

        Vector3 targetPosition = playerCamera.position + playerCamera.forward * distanceFromCamera;
        targetPosition.y += heightOffset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        Vector3 lookDirection = transform.position - playerCamera.position;
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    
    public void OnStartButtonPressed()
    {
        panel.SetActive(false);
    }
    
    public void OnExitButtonPressed()
    {
        // Beende die Anwendung
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}