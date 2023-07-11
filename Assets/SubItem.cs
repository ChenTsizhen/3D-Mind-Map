using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SubItem : MonoBehaviour
{
    public IXRSelectInteractable interactable;
    private LineRenderer lineRenderer;
    private Transform trigger;
    private bool onSelect = false;
    public List<GameObject> subItems;
    
    private void Start()
    {
        interactable = GetComponent<IXRSelectInteractable>();
        interactable.selectEntered.AddListener(Grab());
        interactable.selectExited.AddListener(Drop());
    }

    public void Update()
    {
        if (onSelect)
       {
           if (transform.GetComponent<LineRenderer>() != null && transform.parent != null)
           {
               Transform centerCube = transform.parent;
               // update visual line
               lineRenderer = transform.GetComponent<LineRenderer>();
               lineRenderer.SetPosition(0, centerCube.position);
               lineRenderer.SetPosition(1, transform.position);
               lineRenderer.enabled = true;

               UpdateDetach(centerCube);
           }
       }
    }

    public void UpdateDetach(Transform centerCube)
    {
        subItems = centerCube.GetComponent<Center>().childList;
        GameObject innerSubItem = transform.GetChild(1).gameObject;
        float distance = (transform.parent.position - transform.position).magnitude;
        if (distance > 5.0f)
        {
            transform.SetParent(null);
            subItems.Remove(transform.gameObject);
            transform.gameObject.GetComponent<Center>().enabled = false;
            transform.gameObject.GetComponent<SubItem>().enabled = true;
                
            Color oldColor = centerCube.GetComponent<Center>().addColor;
            innerSubItem.GetComponent<Renderer>().material.color = oldColor;
        }
    }
    
    private UnityAction<SelectEnterEventArgs> Grab()
    {
        return (args) =>
        {
            onSelect = true;
            trigger = transform.GetChild(0);
            trigger.GetComponent<Collider>().isTrigger = true;
        };
    }
 
    private UnityAction<SelectExitEventArgs> Drop()
    {
        return (args) =>
        {
            // onDrop?.Invoke(this, interactor);
            onSelect = false;
            trigger.GetComponent<Collider>().isTrigger = false;
            if (lineRenderer != null)
            {
                lineRenderer.enabled = false;
            }
        };
    }
    
    // private XRBaseInteractor selectingInteractor;
    // protected void OnSelectEntered(XRBaseInteractor interactor)
    // {
    //     selectingInteractor = interactor;
}
