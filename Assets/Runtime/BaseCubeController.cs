using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRC.Students.SU2023.IN03.Chen
{
    /// <summary>
    /// Interprets properties of the simplest unit - cube, such as OnSelect.
    /// Displays a line between the selected cube and its nearest neighbour cube.
    /// </summary>
    public class BaseCubeController : MonoBehaviour, IBaseCube
    {
        /// <summary>
        /// The grab interactable of the gameObject to which this script is attached
        /// </summary>
        private XRGrabInteractable m_Interactable;

        /// <summary>
        /// A list containing all cubes that can form potential grouping relationship with the cube being selected.
        /// </summary>
        private XRGrabInteractable[] m_Cubes;

        /// <summary>
        /// The nearest neighbour of this cube being selected
        /// </summary>
        /// <param name="connectedCube">The nearest neighbour that could form potential grouping relationship
        /// with the cube currently being selected.</param>
        public GameObject connectedCube { get; private set; }

        /// <summary>
        /// Whether the gameObject to which this script is attached is being selected
        /// </summary>
        private bool m_Selected = false;

        /// <summary>
        /// Add listeners to the grab interactable on this cube
        /// </summary>
        private void Start()
        {
            m_Interactable = GetComponent<XRGrabInteractable>();

            m_Interactable.selectEntered.AddListener(OnSelectEnter());
            m_Interactable.selectExited.AddListener(OnSelectExit());
        }

        /// <summary>
        /// When this cube is being selected, detects all other cubes in the scene.
        /// Display a line between the selected cube and its nearest cube, indicating a potential grouping relationship.
        /// </summary>
        public void Update()
        {
            if (m_Selected)
            {
                NearestNeighbor();

                if (gameObject.GetComponent<LineRenderer>() == null || connectedCube == null) return;
                var lineRenderer = gameObject.GetComponent<LineRenderer>(); 
        
                gameObject.GetComponent<LineController>().UpdateLine(lineRenderer, connectedCube.transform.position, transform.position);

                // Debug.Log(transform.position);
            }
        }
        
        /// <summary>
        /// Find the nearest neighbor of the cube currently being selected
        /// </summary>
        public void NearestNeighbor()
        {
            m_Cubes = FindObjectsOfType<XRGrabInteractable>();

            float closest = 100.0f;
            foreach (var cube in m_Cubes)
            {
                var distance = (cube.transform.position - transform.position).magnitude;
                if (distance < closest && distance != 0)
                {
                    closest = distance;
                    connectedCube = cube.gameObject;
                }
            }
        }

        /// <summary>
        /// When this cube entered being selectied, create a line.
        /// </summary>
        private UnityAction<SelectEnterEventArgs> OnSelectEnter()
        {
            return (args) =>
            {
                if (gameObject.GetComponent<LineRenderer>() == null) { gameObject.AddComponent<LineRenderer>();}
                var lineRenderer = gameObject.GetComponent<LineRenderer>();
                
                gameObject.GetComponent<LineController>().InitLine(lineRenderer, Color.green);
                lineRenderer.enabled = true;

                m_Selected = true;
            };
        }

        /// <summary>
        /// When this cube exited being selected, disable the line to its nearest neighbour.
        /// </summary>
        private UnityAction<SelectExitEventArgs> OnSelectExit()
        {
            return (args) =>
            {
                m_Selected = false;

                if (GetComponent<LineRenderer>() != null)
                {
                    GetComponent<LineRenderer>().enabled = false;
                }
            };
        }
    }
}
