using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class LeftCalibration : MonoBehaviour
{
    private XRBaseInteractor interactor;
    private InputDevice controller;

    private float armLength;
    private bool isSelected = false;

    public void Awake()
    {
        interactor = GetComponent<XRBaseInteractor>();
    }

    public void OnSelectEntered()
    {
        isSelected = true;
        controller = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
		armLength = 0;
    }

    public void OnSelectExited()
    {
        Debug.Log("Max arm length during this grab: " +armLength);
        isSelected = false;
    }
    
    public float GetArmLength()
    {
        return armLength;
    }

    public void Update()
    {
        if (controller.isValid && isSelected != false)
        {
            controller.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 handPosition);
            Vector3 headPosition = Camera.main.transform.position;
            // Calculate the difference between head and hand
            float diff = (handPosition - headPosition).magnitude;
			if (armLength < diff) {
				armLength = diff;
			}
        }
    }
}
