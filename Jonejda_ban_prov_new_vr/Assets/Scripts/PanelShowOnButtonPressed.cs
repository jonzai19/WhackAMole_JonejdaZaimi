using UnityEngine;
using UnityEngine.InputSystem;

public class PanelShowOnButtonPressed : MonoBehaviour
{
    [Header("Input Action")] 
    [SerializeField] private InputActionReference secondaryAction;
    
    [Header("UI Panel")] 
    [SerializeField] private GameObject panelObject;
    
    [Header("Positioning of Main Camera in XR Origin")] 
    [SerializeField] private Transform playerTransform;
    
    [SerializeField] private float distanceInFront = 1.5f;
    [SerializeField] private float heightOffset = 0.0f;
    
    private void OnEnable()
    {
        secondaryAction?.action.Enable();
    }
    
    private void OnDisable()
    {
        secondaryAction?.action.Disable();
    }
    
    private void Update()
    {

        if (secondaryAction.action.WasPressedThisFrame())
        {
            panelObject.SetActive(!panelObject.activeSelf);

            if (!panelObject.activeSelf) return;

            Vector3 forward = playerTransform.forward;
            //Optional: Ignore vertical component to keep the panel at a consistent height
            forward.y = 0;
            forward.Normalize();

            Vector3 targetPosition = playerTransform.position + forward * distanceInFront;
            targetPosition.y += heightOffset;

            panelObject.transform.position = targetPosition;

            Vector3 lookDirection = playerTransform.position - targetPosition;
            lookDirection.y = 0;
            //magnitude = x² + y² + z²
            if (lookDirection.magnitude > 0.01) panelObject.transform.rotation = Quaternion.LookRotation(lookDirection);

        }
    }
    
}
