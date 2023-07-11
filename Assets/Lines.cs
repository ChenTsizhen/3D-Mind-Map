using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Lines : MonoBehaviour
{
    public XRBaseInteractable[] interactables;
    private float threshold = 3.0f;
    private float lineWidth = 0.1f;
    
    public void Awake()
    {
        interactables = FindObjectsOfType<XRBaseInteractable>();
    }

    public void Update()
    {
        foreach (XRBaseInteractable outerCube in interactables)
        {
            float distance = (transform.position - outerCube.transform.position).magnitude;
            if (outerCube.transform.childCount > 1 && outerCube.tag != "OuterCube")
            {
                GameObject trigger = outerCube.transform.GetChild(0).gameObject;
                GameObject render = outerCube.transform.GetChild(1).gameObject;
                if (distance < threshold && transform.tag != "OuterCube")
                {
                    if (transform.GetComponent<LineRenderer>() == null)
                    {
                        transform.gameObject.AddComponent<LineRenderer>();
                    }
                    LineRenderer lineRenderer = transform.GetComponent<LineRenderer>();
                    drawLine(lineRenderer, outerCube.transform.position, transform.position);
                    lineRenderer.material = render.GetComponent<Renderer>().material;
                }
            }
        }
    }

    private void drawLine(LineRenderer lineRenderer, Vector3 startPosition, Vector3 endPosition)
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }

}
