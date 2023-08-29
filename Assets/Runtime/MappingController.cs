using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRC.Students.SU2023.IN03.Chen
{
    /// <summary>
    /// If you're pulling an object towards the main camera,
    /// map the controller position to certain world position
    /// by applying a log transformation
    /// </summary>
    public class MappingController : MonoBehaviour, IMapping
    {
        /// <summary>
        /// The ray interactor of the gameObject to which this script is attached
        /// </summary>
        private XRBaseInteractor m_Interactor;

        /// <summary>
        /// To store the initial position of the interactor
        /// </summary>
        private Vector3 m_InitialPosition;

        /// <summary>
        /// To store the current position of the interactor
        /// </summary>
        private Vector3 m_CurrentPosition;

        /// <summary>
        /// Whether the interactor is selecting objects
        /// </summary>
        private bool m_IsSelected;

        /// <summary>
        /// The Continuous Move Provider of the interactor
        /// </summary>
        private ActionBasedContinuousMoveProvider m_ContinuousMoveProvider;

        /// <summary>
        /// The main camera of the scene
        /// </summary>
        private Camera m_Camera;

        /// <summary>
        /// Get the main camera and other components
        /// Add listeners to interactor selection events
        /// </summary>
        public void Start()
        {
            m_Camera = Camera.main;
            m_Interactor = GetComponent<XRBaseInteractor>(); 
            m_ContinuousMoveProvider = GetComponentInParent<ActionBasedContinuousMoveProvider>();
            
            m_Interactor.selectEntered.AddListener(OnSelectEntered());
            m_Interactor.selectExited.AddListener(OnSelectExited());
        }

        /// <summary>
        /// Continuously update by setting the new initial position as the previous current position
        /// after applying each mapping
        /// </summary>    
        public void Update()
        {
            if (!m_IsSelected) return;
            m_CurrentPosition = transform.position;
                
            m_Interactor.attachTransform.position += ApplyMapping(m_InitialPosition, m_CurrentPosition);

            m_InitialPosition = m_CurrentPosition;
        }

        /// <summary>
        /// When the interactor entered selecting an object, start updating mapping
        /// rotate the object to facing the main camera
        /// and disable the Continuous Move Provider
        /// </summary>
        /// <returns> Start mapping after entering selecting an object. </returns>
        public UnityAction<SelectEnterEventArgs> OnSelectEntered()
        {
            return (args) =>
            {
                m_InitialPosition = transform.position;
                m_Interactor.attachTransform.localRotation = Quaternion.Euler(0, -90, 0);
                
                m_IsSelected = true;
                m_ContinuousMoveProvider.enabled = false;
            };
        }

        /// <summary>
        /// When the interactor exited selecting an object,
        /// end mapping and enable the Continuous Move Provider
        /// </summary>
        /// <returns> End mapping after exiting selecting an object. </returns>
        public UnityAction<SelectExitEventArgs> OnSelectExited()
        {
            return (args) =>
            {
                m_IsSelected = false;
                m_ContinuousMoveProvider.enabled = true;
            };
        }

        /// <summary>
        /// Map the controller position to certain world position (see Transformation for details)
        /// if you're pulling an object towards the main camera
        /// </summary>
        /// <param name="initialPosition">The initial position of controller when Update was last called.</param>
        /// <param name="currentPosition">The current position of the controller when Update is now called.</param>
        public Vector3 ApplyMapping(Vector3 initialPosition, Vector3 currentPosition)
        {
            var transformedMovement = Vector3.zero;
            
            // Calculate the movement vector
            var movement = currentPosition - initialPosition;
            var speed = movement.magnitude / Time.deltaTime;

            // apply mapping only if pulling an obj towards
            if (m_Camera == null) return transformedMovement;
            var headPosition = m_Camera.transform.position;
            var headDirection = transform.TransformDirection(headPosition);
            var angle = Vector3.Dot(movement, headDirection);

            if ((!(angle < 0.03) && !(angle > -0.03)) || !(speed > 0.3)) return transformedMovement;
            var transformedX = Transformation(movement.x);
            var transformedY = Transformation(movement.y);
            var transformedZ = Transformation(movement.z);
            transformedMovement = new Vector3(transformedX, transformedY, transformedZ);

            return transformedMovement;
        }

        /// <summary>
        /// Take in the controller movement amount on each axis
        /// and return the mapped value after applying a log function
        /// </summary>
        ///
        /// <param name="move">The controller movement amount on each axis</param>
        /// <returns> The movement amount after applying log transformation. </returns>
        public static float Transformation(float move)
        {
            float adjustedValue;
            if (move == 0)
            {
                adjustedValue = 1;
            }
            else
            {
                adjustedValue = 50 * Mathf.Log(move + 1);
            }

            return adjustedValue;
        }
    }
}