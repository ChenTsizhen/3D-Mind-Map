using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Center : MonoBehaviour
{
    private Color centerColor;
    private GameObject outerCenter;
    public IXRHoverInteractable interactable;
    
    public List<GameObject> childList;
	public List<LineRenderer> lineList;
	public Color addColor;
	private bool onHover = false;
	private float threshold = 2.0f;
	private float lineWidth = 0.1f;

	public void Start() 
    {
	    outerCenter = transform.gameObject;
	    GameObject innerCenter = outerCenter.transform.GetChild(1).gameObject;
        centerColor = innerCenter.GetComponent<Renderer>().material.color;
        
        interactable = GetComponent<IXRHoverInteractable>();
        interactable.hoverEntered.AddListener(OnHoverEntered());
        interactable.hoverExited.AddListener(OnHoverExited());
    }
	
	public void Update()
    {
	    if (childList != null) {
		    // for (int i = 0; i < outerCenter.transform.Count; i++)
		    foreach (GameObject child in childList)
            {
	            // always update current childList
	            child.transform.SetParent(outerCenter.transform);
	            if (onHover)
	            {
		            LineRenderer lineRenderer = child.GetComponent<LineRenderer>(); // get visual line
		            // update visual line
		            lineRenderer.SetPosition(0, outerCenter.transform.position);
		            lineRenderer.SetPosition(1, child.transform.position);
		            lineRenderer.enabled = true;
	            }
            }
		}
    }
	
	public UnityAction<HoverEnterEventArgs> OnHoverEntered()
	{
		return (args) =>
		{
			onHover = true;
		};
	}
	
	public UnityAction<HoverExitEventArgs> OnHoverExited()
    {
	    return (args) =>
	    {
		    onHover = false;
		    foreach (LineRenderer line in lineList)
		    {	
			    line.enabled = false;
		    }
	    };
    }
    
	/* When a collider enters a trigger's space, the trigger will call OnTriggerEnter */
    public void OnTriggerEnter(Collider collision)
    {
	    
	    if (collision.gameObject.tag == "AddOne" && outerCenter != null)
        {
	        GameObject trigger = collision.gameObject;
			GameObject outerSubItem = trigger.transform.parent.gameObject;
			GameObject innerSubItem = outerSubItem.transform.GetChild(1).gameObject;
			addColor = innerSubItem.GetComponent<Renderer>().material.color;
			
			float distance = (trigger.transform.position - outerCenter.transform.position).magnitude;
			if (distance < threshold && !childList.Contains(outerSubItem)) {
				// set parent
				outerSubItem.transform.SetParent(outerCenter.transform);
				childList.Add(outerSubItem); // add to childList
				
				// set this cube as subItem
				outerSubItem.GetComponent<SubItem>().enabled = true;
				outerSubItem.GetComponent<Center>().enabled = false;

				// create line
				outerSubItem.AddComponent<LineRenderer>();
				LineRenderer lineRenderer = outerSubItem.GetComponent<LineRenderer>();
				lineRenderer.startWidth = lineWidth;
				lineRenderer.endWidth = lineWidth;
				lineRenderer.positionCount = 2;
				lineList.Add(lineRenderer); // add to lineList
				
				// visual feedback
				innerSubItem.GetComponent<Renderer>().material.color = centerColor;
				lineRenderer.material = innerSubItem.GetComponent<Renderer>().material;
			}
        }
	}
}
