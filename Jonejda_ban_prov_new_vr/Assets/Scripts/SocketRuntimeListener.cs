using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketRuntimeListener : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;

    private void Awake()
    {
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
    }

    private void OnEnable()
    {
        if (socket == null) return;
        
        socket.selectEntered.AddListener(OnObjectInserted);
        socket.selectExited.AddListener(OnObjectRemoved);
    }
    
    private void OnDisable()
    {
        if (socket == null) return;
        
        socket.selectEntered.RemoveListener(OnObjectInserted);
        socket.selectExited.RemoveListener(OnObjectRemoved);
    }
    

    private void OnObjectInserted(SelectEnterEventArgs args)
    {
        Debug.Log("Socket Inserted: ");
    }

    private void OnObjectRemoved(SelectExitEventArgs args)
    {
        Debug.Log("Socket Removed: " + args.interactableObject.transform.name);
    }
}