using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRC.Students.SU2023.IN03.Chen
{
    /// <summary>
    /// Smooth the child cube towards the parent cube position
    /// </summary>
    public class GroupMoveController : MonoBehaviour, ISmoothDamp, IRotateTowards
    {
        /// <summary>
        /// The the transform of the gameObejct to which this script is attached
        /// </summary>
        private Transform m_Self;
        /// <summary>
        /// The parent transform of the gameObject this script is attached to
        /// </summary>
        private Transform m_Parent;

        /// <summary>
        /// To store the initial position of this cube
        /// </summary>
        private Vector3 m_InitialPosition;
        /// <summary>
        /// To store the current position of this cube
        /// </summary>
        private Vector3 m_CurrentPosition;

        /// <summary>
        /// To store the initial position of this cube
        /// </summary>
        [SerializeField]
        private float smoothTime = 0.01f;

        /// <summary>
        /// Approximately the time it will take to reach the target. A smaller value will reach the target faster.
        /// </summary>
        [SerializeField]
        private Vector3 velocity = Vector3.zero;
        
        
        /// <summary>
        /// The grab interactable of the gameObject to which this script is attached
        /// </summary>
        private XRGrabInteractable m_Interactable;

        /// <summary>
        /// The main camera in the scene
        /// </summary>
        private Camera m_Camera;
        
        /// <summary>
        /// The speed of cube rotating from its original quaternion to towards the main camera
        /// </summary>
        [SerializeField] private float rotationSpeed = 75f;

        
        /// <summary>
        /// Set this cube as the transfrom of the attached the gameObject
        /// Set initial position as the position of this cube's transform when started
        /// </summary>
        public void Awake()
        {
            m_Self = transform;
            m_InitialPosition = m_Self.position;
        }
        
        /// <summary>
        /// Set the main camera as camera
        /// Get grab interactable component from the attached gameObject
        /// </summary>
        public void Start()
        {
            m_Camera = Camera.main;
            m_Interactable = GetComponent<XRGrabInteractable>();
        }

        /// <summary>
        /// Continuously update by setting the new initial position as the previous current position
        /// after applying smooth damp
        /// </summary>
        public void Update()
        {
            ApplyRotateTowards(m_Camera.transform, m_Interactable.transform);
            
            if (transform.parent == null) return;
            m_Parent = transform.parent;

            m_CurrentPosition = ApplySmoothDamp(m_Parent, m_Self, m_InitialPosition);

            m_InitialPosition = m_CurrentPosition;
        }

        /// <summary>
        /// Smooth the child cube position towards the parent cube position
        /// </summary>
        /// 
        /// <param name="parent">The parent cube transform</param>
        /// <param name="self">The child cube transform</param>
        /// <param name="initialPosition">The initial position of the child cube when Update was last called.</param>
        /// <returns> The position of the child cube after applying smooth damp. </returns>
        public Vector3 ApplySmoothDamp(Transform parent, Transform self, Vector3 initialPosition)
        {
            // Define a target position above and behind the target transform
            var targetPosition = parent.TransformPoint(self.localPosition);
            // Smoothly move the camera towards that target position
            self.position = Vector3.SmoothDamp(initialPosition, targetPosition, ref velocity, smoothTime);

            return self.position;
        }
        
        /// <summary>
        /// Update the cube so that it always rotates towards the main camera
        /// </summary>
        /// <param name="camera">The main camera, center of rotation</param>
        /// <param name="interactable">The transform of the interactable rotating towards the camera</param>
        public void ApplyRotateTowards(Transform camera, Transform interactable){
            var direction = camera.position - interactable.position;
            direction.y = 0.0f;
            direction = Vector3.Normalize(direction);
            direction = Quaternion.Euler(0, 90, 0) * direction;

            var rotation = Quaternion.LookRotation(direction);
            interactable.rotation = Quaternion.RotateTowards
                (interactable.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
}