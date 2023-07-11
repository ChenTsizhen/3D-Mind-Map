using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class rotateTowards : MonoBehaviour
{
    private CharacterController characterController;
    public float rotationSpeed = 75f;
    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        XRGrabInteractable[] grabInteractables = FindObjectsOfType<XRGrabInteractable>();
        if (grabInteractables != null)
        {
            foreach (XRGrabInteractable interactable in grabInteractables)
            {
                Vector3 direction =  characterController.transform.position - interactable.transform.position;
				direction.y = 0.0f;
				direction = Vector3.Normalize(direction);
                direction = Quaternion.Euler(0, 90, 0) * direction;

                Quaternion rotation = Quaternion.LookRotation(direction);
                interactable.transform.rotation = Quaternion.RotateTowards
                    (interactable.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
