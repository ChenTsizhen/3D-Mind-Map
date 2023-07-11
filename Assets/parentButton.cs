using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class parentButton : MonoBehaviour
{
    private XRBaseInteractor interactor;
    private IXRSelectInteractable interactable;
    private Vector3 newScale = new Vector3 (0.1f, 2, 2);
    
    [SerializeField] 
    InputActionReference m_SetParentAction;
    
    void Start()
    {
        interactor = GetComponent<XRBaseInteractor>();
        
        m_SetParentAction.asset.Enable();
        m_SetParentAction.action.performed += SetAsParent;
        m_SetParentAction.action.canceled -= SetAsParent;
    }

    public void SetAsParent(InputAction.CallbackContext input)
    {
        interactable = interactor.interactablesSelected[0];
        Transform outerCube = interactable.transform;

        // set this cube as center
        outerCube.gameObject.GetComponent<Center>().enabled = true;
        outerCube.gameObject.GetComponent<SubItem>().enabled = false;
        
        // if (outerCube.childCount > 2)
        // {
        //     for (int i = 0; i < outerCube.childCount; i++)
        //     {
        //         Transform child = outerCube.GetChild(i);
        //
        //         if (child != null && child.GetComponent<Center>().enabled)
        //         {
        //             child.GetComponent<Center>().enabled = false;
        //             child.GetComponent<SubItem>().enabled = true;
        //         }
        //     }
        // }

        // set size to larger
        Transform trigger = outerCube.GetChild(0);
        Transform render = outerCube.GetChild(1);
        trigger.localScale = newScale;
        render.localScale = newScale;
    }
}
