using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class draftMapping : MonoBehaviour
{
	private XRBaseInteractor interactor;
	private ActionBasedContinuousMoveProvider continuousMoveProvider;

	private Vector3 initialPosition;
	private Vector3 currentPosition;
	private bool isSelected = false;
	private Vector3 addPosition;
	private Transform outerCube;

	[SerializeField] 
	InputActionReference m_SetParentAction;
	// public InputHelpers.Button button;
	// private InputDevice controller;
	
	public void Awake()
	{
		interactor = GetComponent<XRBaseInteractor>();
		continuousMoveProvider = GetComponentInParent<ActionBasedContinuousMoveProvider>();
		
		m_SetParentAction.asset.Enable();
		m_SetParentAction.action.performed += SetAsParent;
	}
	
	public void OnSelectEntered()
	{
		isSelected = true;
		continuousMoveProvider.enabled = false;

		// controller = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
		// controller.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 handPosition);
		// initialPosition = handPosition;

		initialPosition = transform.position;
		interactor.attachTransform.localRotation = Quaternion.Euler(0, -90, 0);

		// create a new post-it note
		IXRSelectInteractable interactable = interactor.interactablesSelected[0];
		if (interactable.transform.gameObject.CompareTag("OuterCube"))
		{
			addPosition = interactable.transform.position; // get original position
			outerCube = interactable.transform;
			GameObject newCube = Instantiate(outerCube.gameObject);
			newCube.transform.position = addPosition;
		}
		// recursion
		interactable.transform.gameObject.tag = "Untagged";
		GameObject trigger = interactable.transform.GetChild(0).gameObject;
		trigger.tag = "AddOne";
	}

	public void OnSelectExited()
	{
		isSelected = false;
		continuousMoveProvider.enabled = true;
	}

	private float Transformation(float move)
	{
		float adjustedValue;
		if (move == 0) { adjustedValue = 1; }
		else
		{
			adjustedValue = 50 * Mathf.Log(move + 1);
		}
		return adjustedValue;
	}

	public void Update()
	{
		if (isSelected)
		{
			// controller.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 handPosition);
			// currentPosition = handPosition;
			currentPosition = transform.position;

			// Calculate the movement vector
			Vector3 movement = currentPosition - initialPosition;
			float speed = movement.magnitude / Time.deltaTime;

			// apply mapping only if pulling an obj towards
			Vector3 headPosition = Camera.main.transform.position;
			Vector3 headDirection = transform.TransformDirection(headPosition);
			float angle = Vector3.Dot(movement, headDirection);
			
			if ((angle > 0.01 || angle < -0.01) && speed > 0.4)
			{
				// Debug.Log("angle: " + angle);
				float transformedY = Transformation(movement.y);
				float transformedZ = Transformation(movement.z);
				float transformedX = Transformation(movement.x);
				Vector3 transformedMovement = new Vector3(transformedX,transformedY,transformedZ);
				interactor.attachTransform.position += transformedMovement;
			}
			initialPosition = currentPosition;
			
			// controller.inputDevice.IsPressed(button, out pressed);
			// bool value = m_SetParentAction.action.ReadValue<bool>();
			// SetAsParent(value);
		}
	}

	public void SetAsParent(InputAction.CallbackContext input)
	{
		IXRSelectInteractable interactable = interactor.interactablesSelected[0];
		Debug.Log(interactable);

		// set this cube as center
		interactable.transform.gameObject.GetComponent<Center>().enabled = true;
		interactable.transform.gameObject.GetComponent<SubItem>().enabled = false;

		for (int i = 0; i < interactable.transform.childCount; i++)
		{
			Transform child = interactable.transform.GetChild(i);
			if (child.GetComponent<SubItem>().enabled)
			{
				// set the children as subItems
				interactable.transform.GetComponent<SubItem>().enabled = true;
				interactable.transform.GetComponent<Center>().enabled = false;
			}
		}
	}
}