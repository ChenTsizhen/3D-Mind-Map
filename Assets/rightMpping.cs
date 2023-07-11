using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class rightMpping : MonoBehaviour
{
	private IXRSelectInteractable grabInteractable;
    private XRBaseInteractor interactor;
	private ActionBasedSnapTurnProvider snapTurnProvider;

    private InputDevice controller;
    private Vector3 initialPosition;
    private Vector3 currentPosition;
    private bool isSelected = false;
	private float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    public void Awake()
    {
        interactor = GetComponent<XRBaseInteractor>();
		snapTurnProvider = GetComponentInParent<ActionBasedSnapTurnProvider>();
    }

    public void OnSelectEntered()
    {
        isSelected = true;
		snapTurnProvider.enabled = false;

        controller = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        controller.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 handPosition);
        initialPosition = handPosition;
		interactor.attachTransform.localRotation = Quaternion.Euler(0, -90, 0);

		// grabInteractable = interactor.interactablesSelected[0];
		// grabInteractable.transform.localRotation = interactor.attachTransform.localRotation;
    }
    
    public void OnSelectExited()
    {
        isSelected = false;
		snapTurnProvider.enabled = true;
    }
    
	private float transformation(float move)
	{
		float adjustedValue;
		if (move == 0) {
			adjustedValue = 1;
		}
		else {
			adjustedValue = 7500 * Mathf.Log(move + 1);
		}
		return adjustedValue;
	}

    public void Update()
    {
	    if (controller.isValid && isSelected != false)
        {
			controller.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 handPosition);
            currentPosition = handPosition;
            // Calculate the movement vector
            Vector3 movement = currentPosition - initialPosition; 
			float move = movement.magnitude;
			float speed = move / Time.deltaTime;

			Vector3 headPosition = Camera.main.transform.position;
			Vector3 headDirection = transform.TransformDirection(headPosition);
			float diffAngle = Vector3.Dot(movement, headDirection);
            
			// if (diffAngle < 0.2 && diffAngle > -0.5 && move > 0.001f) {
			//     float trans = transformation(move);
			//     movement *= trans;
			// }
          	if (diffAngle < 0.2 && diffAngle > -0.2 && speed > 0.3) {
				float trans = transformation(move);
			    Vector3 targetPosition = movement * trans;
				// Smooth damp
				Vector3 smoothDampedPosition = Vector3.SmoothDamp(targetPosition, currentPosition, ref velocity, smoothTime);
				interactor.attachTransform.localPosition += smoothDampedPosition;
			}
            // Apply the movement to the interactable object's position
            interactor.attachTransform.localPosition += movement;
            initialPosition = currentPosition;

			controller.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);
			// Map rotation of input device to virtual world
			interactor.attachTransform.localRotation = rotation;
        }
    }
}
