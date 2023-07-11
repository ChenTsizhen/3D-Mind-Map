using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Mapping : MonoBehaviour
{
    private XRBaseInteractor interactor;
    private ActionBasedContinuousMoveProvider continuousMoveProvider;

    private Vector3 initialPosition;
    private Vector3 currentPosition;
    private bool isSelected = false;
    private Vector3 addPosition;
    private Transform outerCube;

    public void Start()
    {
        continuousMoveProvider = GetComponentInParent<ActionBasedContinuousMoveProvider>();
        interactor = GetComponent<XRBaseInteractor>();
        
        interactor.selectEntered.AddListener(OnSelectEntered());
        interactor.selectExited.AddListener(OnSelectExited());
    }
    
    public void Update()
    {
        if (isSelected)
        {
            currentPosition = transform.position;

            // Calculate the movement vector
            Vector3 movement = currentPosition - initialPosition;
            float speed = movement.magnitude / Time.deltaTime;

            // apply mapping only if pulling an obj towards
            Vector3 headPosition = Camera.main.transform.position;
            Vector3 headDirection = transform.TransformDirection(headPosition);
            float angle = Vector3.Dot(movement, headDirection);
			
            if ((angle < 0.03 || angle > -0.03) && speed > 0.3)
            {
                // Debug.Log("angle: " + angle);
                float transformed_X = Transformation(movement.x);
                float transformed_Y = Transformation(movement.y);
                float transformed_Z = Transformation(movement.z);
                Vector3 transformedMovement = new Vector3(transformed_X,transformed_Y,transformed_Z);
                interactor.attachTransform.position += transformedMovement;
            }
            initialPosition = currentPosition;
        }
    }
    
    public UnityAction<SelectEnterEventArgs> OnSelectEntered()
    {
        return (args) =>
        {
            isSelected = true;
            continuousMoveProvider.enabled = false;

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
        };
    }
    
    public UnityAction<SelectExitEventArgs> OnSelectExited()
    {
        return (args) =>
        {
            isSelected = false;
            continuousMoveProvider.enabled = true;
        };
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
}
