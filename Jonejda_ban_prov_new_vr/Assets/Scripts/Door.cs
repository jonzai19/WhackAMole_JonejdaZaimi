using System;
using UnityEngine;

public class Door : MonoBehaviour
{
      private float openAngleY = -120f;
      private float rotationSpeed = 3f;
      private bool isOpen;
      private Quaternion closedRotation;
      private Quaternion openRotation;
      
      private void Awake()
      {
          //optional, you can also just write the values you want in the Quaternion
          closedRotation = transform.rotation;
          Vector3 eulerAngles = closedRotation.eulerAngles;
          eulerAngles.y += openAngleY;
          openRotation = Quaternion.Euler(eulerAngles);
      }

      private void Update()
      {
          Quaternion targetRotation = isOpen ? openRotation : closedRotation;
          transform.rotation = Quaternion.Slerp(
              transform.rotation, 
              targetRotation, 
              Time.deltaTime * rotationSpeed
              );
      }
      
      public void ToggleDoor()
      {
          Debug.Log("Toggle door");
            isOpen = !isOpen;
      }
}
