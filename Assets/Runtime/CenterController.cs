using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRC.Students.SU2023.IN03.Chen
{
	/// <summary>
    /// Interprets properties of the parent cube.
    /// Default is disabled. This script is only enabled when it received set parent input.
    /// </summary>
	public class CenterController : MonoBehaviour, ICenter, ICreateCube, ICubeVisualFeedback
	{
		/// <summary>
        /// The gameObject this script is attached to
        /// </summary>
		private GameObject m_OuterCenter;

		/// <summary>
        /// A list containing all child cubes of this cube.
        /// </summary>
		private List<GameObject> m_ChildList;

		/// <summary>
        /// The distance between this and its children.
        /// </summary>
		[SerializeField]
		private float k_Distance = 1.5f;

		/// <summary>
        /// Get the gameObject this script is attached to
		/// Create an empty list to store the child cubes of this cube.
		/// </summary>
		public void Awake()
		{
			m_OuterCenter = transform.gameObject;
			m_ChildList = new List<GameObject>();
		}

		/// <summary>
        /// Update the line between parent cube and its children cubes.
        /// </summary>
		public void Update()
		{
			if (m_ChildList.Count <= 0) return;
			foreach (var child in m_ChildList)
			{
				child.transform.SetParent(m_OuterCenter.transform);
				
				if (child.GetComponent<LineRenderer>() == null) return;
				var lineRenderer = child.GetComponent<LineRenderer>();
				GetComponent<LineController>().UpdateLine(lineRenderer, child.transform.position, transform.position);

				var centerColor = m_OuterCenter.transform.GetChild(0).GetComponent<Renderer>().material.color;
				lineRenderer.material.color = centerColor;
			}
		}

		/// <summary>
        /// Add the child cube to the parent it connects to
        /// </summary>
		/// <param name="childCube">The child cube.</param>
		public void AddCube(GameObject childCube)
		{
			var renderCenter = m_OuterCenter.transform.GetChild(0).gameObject;
			var centerColor = renderCenter.GetComponent<Renderer>().material.color;

			if (!m_ChildList.Contains(childCube))
			{
				childCube.transform.SetParent(transform);
				m_ChildList.Add(childCube);
				
				if (childCube.GetComponent<LineRenderer>() == null) { childCube.AddComponent<LineRenderer>();}
				var lineRenderer = childCube.GetComponent<LineRenderer>();

				gameObject.GetComponent<LineController>().InitLine(lineRenderer, centerColor);
				lineRenderer.enabled = true;
			}

			SetColor(renderCenter, childCube);
			
		}

		/// <summary>
        /// Set the color of the childCube to be the same as renderCenter
        /// </summary>
        /// <param name="renderCenter">the cube to get color from </param>
        /// <param name="childCube">the cube to add color to /param>
        public void SetColor(GameObject renderCenter, GameObject childCube)
        {
			var centerColor = renderCenter.GetComponent<Renderer>().material.color;
            var childRender = childCube.transform.GetChild(0).gameObject;
			childRender.GetComponent<Renderer>().material.color = centerColor;
        }

		/// <summary>
        /// Spread the children cubes evenly around the center cube.
        /// </summary>
		public void OrderChildren(){
			if (m_ChildList.Count <= 0) return;
			for (int i = 0; i < m_ChildList.Count; i++)
			{
				double radian = (Math.PI * 2) / (i+1);

				var y = Mathf.Cos((float)radian);
				var z = Mathf.Sqrt(1 - Mathf.Pow(y, 2));
				
				var rotation = new Vector3 (0, y, z);
				// var distance = (m_ChildList[i].transform.position - transform.position).magnitude;
				var position = rotation * k_Distance;
				
				m_ChildList[i].transform.localPosition = position;
			}
		}

		/// <summary>
        /// Empty the child list of this parent cube
        /// </summary>
		public void Clear()
		{
			for (int i = 0; i < m_ChildList.Count; i++)
			{
				var child = m_ChildList[i];
				// var originalPosition = child.transform.position;

				DetachChildHelper(child);
				// child.transform.position = originalPosition;
			}

			m_ChildList.Clear();
		}
		
		/// <summary>
        /// Detach this child cube from its parent cube
        /// </summary>
		/// <param name="child">The child cube to be detached from parent.</param>
		public void DetachChildHelper(GameObject child)
		{
			if (child.GetComponent<LineRenderer>() == null) return; 
			child.GetComponent<LineRenderer>().enabled = false;
			
			child.transform.SetParent(null);
		}
		
		/// <summary>
        /// Detach this child cube from its parent cube
        /// and remove this child from the child list
        /// </summary>
        /// <param name="child">The child cube to be detached from parent.</param>
		public void Detach(GameObject child){
			// var originalPosition = child.transform.position;

			DetachChildHelper(child);
			m_ChildList.Remove(child);

			// child.transform.position = originalPosition;
		}
	}
}