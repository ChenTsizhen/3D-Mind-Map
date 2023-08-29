using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRC.Students.SU2023.IN03.Chen
{
    /// <summary>
    /// Create a new cube prefab when hovering on the source cube
    /// </summary>
    public class SourceCubeController : MonoBehaviour, ICreateCube, ICubeVisualFeedback
    {
        /// <summary>
        /// The grab interactable of the gameObject to which this script is attached
        /// </summary>
        private XRBaseInteractable m_Interactable;
        
        /// <summary>
        /// The cube prefab - the simplest unit of grouping interactions
        /// </summary>
        [SerializeField] 
        public GameObject Grabbale;

        /// <summary>
        /// Get the interactable component of the attached gameObject
        /// Add listener to hoverEntered
        /// </summary>
        public void Awake()
        {
            m_Interactable = GetComponent<XRBaseInteractable>();
            
            m_Interactable.hoverEntered.AddListener(OnHoverEntered());
        }
        
        
        /// <summary>
        /// Create a cube (prefab) at the position of the the sourceCube
        /// Set the new cube as the same color as the source cube.
        /// </summary>
        /// <param name="sourceCube">the sourceCube containing the target position of adding a new cube</param>
        public void AddCube(GameObject sourceCube)
        {
            var newCube = Instantiate(Grabbale, sourceCube.transform.position, Quaternion.identity);

            SetColor(sourceCube, newCube);
            
            newCube.SetActive(true);
        }

        /// <summary>
        /// Set the color of the newCube to be the same as sourceCube
        /// </summary>
        /// <param name="sourceCube">the cube to get color from </param>
        /// <param name="newCube">the cube to add color to /param>
        public void SetColor(GameObject sourceCube, GameObject newCube)
        {
            var sourceColor = sourceCube.GetComponent<Renderer>().material.color;
            newCube.transform.GetChild(0).GetComponent<Renderer>().material.color = sourceColor;
        }
        
        /// <summary>
        /// Create a new cube when hovering on the source cube
        /// </summary>
        private UnityAction<HoverEnterEventArgs> OnHoverEntered()
        {
            return (args) =>
            {
                var sourceCube = m_Interactable.transform.gameObject;
                AddCube(sourceCube);
            };
        }

    }
}