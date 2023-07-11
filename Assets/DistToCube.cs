using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class DistToCube : MonoBehaviour
{
    [SerializeField]
    private Transform cube;
    
    [SerializeField]
    private Text inputDistText;
    
    private float inputDist;

    void Update()
    {
       inputDist = (cube.transform.position - transform.position).magnitude;
       inputDistText.text = "Distance: " + inputDist.ToString("F1");
    }
}
