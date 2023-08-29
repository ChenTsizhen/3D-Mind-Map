using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

namespace XRC.Students.SU2023.IN03.Chen
{
    /// <summary>
    /// This class handles grouping input actions on the controller
    /// </summary>
    public class GroupingInputManager : MonoBehaviour, ICreateCube, ICubeVisualFeedback
    {
        /// <summary>
        /// The action to set the selected cube as parent
        /// </summary>
        [SerializeField] 
        InputActionReference m_SetParentAction;
        
        /// <summary>
        /// The action to set the selected cube as child
        /// </summary>
        [SerializeField] 
        InputActionReference m_SetChildAction;
        
        /// <summary>
        /// The action to detach the selected cube from its parent
        /// If the selected cube is a parent, detach all its children cubes 
        /// </summary>
        [SerializeField]
        InputActionReference m_DetachAction;

        /// <summary>
        /// The action to create a cube on the same layer as the selected cube
        /// </summary>
        [SerializeField] 
        InputActionReference m_NewParallelAction;

        /// <summary>
        /// The action to create a child of the selected cube
        /// </summary>
        [SerializeField] 
        InputActionReference m_NewChildAction;

        /// <summary>
        /// The cube - the simplest unit of grouping interactions.
        /// Since the outer layer of the cube is a grab interactable, it's called Grabbable in Prefabs.
        /// </summary>
        /// <value> The Grabbale prefab </value>
        [SerializeField] 
        public GameObject Grabbale;

        /// <summary>
        /// The size (transform) of the cube after being set as a parent
        /// </summary>
        [SerializeField]
        Vector3 parentScale;

        /// <summary>
        /// The size (transform) of a child cube, which is also the default the cube size.
        /// </summary>
        [SerializeField]
        private Vector3 childScale = new Vector3 (0.1f, 1, 1);

        /// <summary>
        /// The ray interactor of the gameObject to which this script is attached
        /// </summary>
        private XRBaseInteractor m_Interactor;

        /// <summary>
        /// The selected interactable
        /// </summary>
        private IXRSelectInteractable m_Interactable;

        /// <summary>
        /// The gameObject of the selected interactable
        /// </summary>
        private GameObject m_Grabbable;

        /// <summary>
        /// Get the ray interactor attached to
        /// Enable grouping input actions and set up callbacks
        /// </summary>
        public void Start()
        {
            m_Interactor = GetComponent<XRRayInteractor>();
            EnableInputAction();
            SetupCallbacks();
        }

        /// <summary>
        /// Enable all grouping input actions in the asset
        /// </summary>
        public void EnableInputAction()
        {
            m_SetParentAction.asset.Enable();
            m_SetChildAction.asset.Enable();
            m_DetachAction.asset.Enable();
            m_NewParallelAction.asset.Enable();
            m_NewChildAction.asset.Enable();
        }

        /// <summary>
        /// Add the input actions when performed
        /// Remove the input actions when canceled
        /// </summary>
        public void SetupCallbacks()
        {
            m_SetParentAction.action.performed += SetAsParent;
            m_SetParentAction.action.canceled -= SetAsParent;
            
            m_SetChildAction.action.performed += SetAsChild;
            m_SetChildAction.action.canceled -= SetAsChild;
            
            m_DetachAction.action.performed += SetDetach;
            m_DetachAction.action.canceled -= SetDetach;

            m_NewParallelAction.action.performed += NewParallel;
            m_NewParallelAction.action.canceled -= NewParallel;

            m_NewChildAction.action.performed += NewChild;
            m_NewChildAction.action.canceled -= NewChild;
        }

        /// <summary>
        /// Disable all grouping input actions in the asset onDestroy
        /// </summary>
        public void OnDestroy()
        {
            m_SetParentAction.asset.Disable();
            m_SetChildAction.asset.Disable();
            m_DetachAction.asset.Disable();
            m_NewParallelAction.asset.Disable();
            m_NewChildAction.asset.Disable();
        }

        /// <summary>
        /// Set the selected cube as parent
        /// </summary>
        ///<param name="input"> Set parent input action callback </param>
        public void SetAsParent(InputAction.CallbackContext input)
        {
            if (!m_Interactor.isSelectActive) return;
            m_Interactable = m_Interactor.interactablesSelected[0];
            m_Grabbable = m_Interactable.transform.gameObject; 

            // set this cube as center if it's not
            if (!m_Grabbable.GetComponent<CenterController>().enabled)
            {
                m_Grabbable.GetComponent<CenterController>().enabled = true;

                // set children
                var connected = m_Grabbable.GetComponent<BaseCubeController>().connectedCube;
                if (connected == null) 
                {
                    Debug.Log("Please connect to a child FIRST.");
                    return;
                }
                m_Grabbable.GetComponent<CenterController>().AddCube(connected);
                m_Grabbable.GetComponent<CenterController>().OrderChildren();
            }
            SetScale(m_Grabbable.transform, parentScale);
        }

        /// <summary>
        /// Set the selected cube as child
        /// </summary>
        ///<param name="input"> Set child input action callback </param>
        public void SetAsChild(InputAction.CallbackContext input)
        {
            if (!m_Interactor.isSelectActive) return;
            m_Interactable = m_Interactor.interactablesSelected[0];
            m_Grabbable = m_Interactable.transform.gameObject; 
                
            // set self as children, the ONE connected cube as parent
            var connected = m_Grabbable.GetComponent<BaseCubeController>().connectedCube;
            if (connected == null) return;
            
            var parent = connected;
            parent.GetComponent<CenterController>().enabled = true;
            parent.GetComponent<CenterController>().AddCube(m_Grabbable);
            parent.GetComponent<CenterController>().OrderChildren();
        }

        /// <summary>
        /// The action to detach the selected cube from its parent
        /// If the selected cube is a parent, detach all its children cubes 
        /// </summary>
        /// <param name="input"> Set detach input action callback </param>
        public void SetDetach(InputAction.CallbackContext input)
        {
            if (!m_Interactor.isSelectActive) return;
            m_Interactable = m_Interactor.interactablesSelected[0];
            m_Grabbable = m_Interactable.transform.gameObject;

            // If this is a parent, detach all children
            if (m_Grabbable.GetComponent<CenterController>().enabled)
            {
                m_Grabbable.GetComponent<CenterController>().Clear();
                    
                SetScale(m_Grabbable.transform, childScale);

                m_Grabbable.GetComponent<CenterController>().enabled = false;
            }
            
            // If this is a child, detach it from parent.
            if (m_Grabbable.transform.parent == null) return;
            var centerCube = (m_Grabbable.transform).parent.gameObject;
                
            if (m_Grabbable.transform.parent.GetComponent<CenterController>().enabled)
            {
                centerCube.GetComponent<CenterController>().Detach(m_Grabbable.gameObject);
            }

        }

        /// <summary>
        /// Create a cube on the same layer as the selected cube
        /// </summary>
        /// <param name="input"> Create a parallel cube on the same layer as the cube being selected
        /// input action callback </param>
        public void NewParallel(InputAction.CallbackContext input){
            if (!m_Interactor.isSelectActive) return;
            m_Interactable = m_Interactor.interactablesSelected[0];
            m_Grabbable = m_Interactable.transform.gameObject;

            var parallelCube = AddCube(m_Grabbable);

            if (m_Grabbable.transform.parent == null) return;
            var centerCube = (m_Grabbable.transform).parent.gameObject;

            centerCube.GetComponent<CenterController>().AddCube(parallelCube);
            centerCube.GetComponent<CenterController>().OrderChildren();
        }

        /// <summary>
        /// Create a child of the selected cube
        /// </summary>
        /// <param name="input"> Create a child cube of the cube currently being selected
        /// input action callback  </param>
        public void NewChild(InputAction.CallbackContext input){
            if (!m_Interactor.isSelectActive) return;
            m_Interactable = m_Interactor.interactablesSelected[0];
            m_Grabbable = m_Interactable.transform.gameObject;

            var childCube = AddCube(m_Grabbable);

            m_Grabbable.GetComponent<CenterController>().enabled = true;
            m_Grabbable.GetComponent<CenterController>().AddCube(childCube);
            m_Grabbable.GetComponent<CenterController>().OrderChildren();
        }

        /// <summary>
        /// Set the renderer of the cube to the scale
        /// </summary>
        /// <param name="outerCube">The outer cube containing the renderer</param>
        /// <param name="scale">The desired scale</param>
        public static void SetScale(Transform outerCube, Vector3 scale)
        {
            var render = outerCube.GetChild(0);
            render.localScale = scale;
        }

        /// <summary>
        /// Create a cube (prefab) at the position of the the sourceCube
        /// </summary>
        /// <param name="sourceCube">the sourceCube containing the target position of adding a cube</param>
        private GameObject AddCube(GameObject sourceCube)
        {
            var newCube = Instantiate(Grabbale, sourceCube.transform.position, Quaternion.identity);

            SetColor(sourceCube, newCube);

            newCube.SetActive(true);
            return newCube;
        }

        /// <summary>
        /// Set the color of the newCube to be the same as sourceCube
        /// </summary>
        /// <param name="sourceCube">the cube to get color from </param>
        /// <param name="newCube">the cube to add color to /param>
        public void SetColor(GameObject sourceCube, GameObject newCube)
        {
            var sourceColor = sourceCube.transform.GetChild(0).GetComponent<Renderer>().material.color;
            newCube.transform.GetChild(0).GetComponent<Renderer>().material.color = sourceColor;
        }
    }
}