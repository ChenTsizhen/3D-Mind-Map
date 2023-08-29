using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRC.Students.SU2023.IN03.Chen
{
    /// <summary>
    /// Displays a line between the selected cube and its nearest neighbour cube.
    /// </summary>
    public class LineController : MonoBehaviour, ILine
    {
        /// <summary>
        /// The width of the line
        /// </summary>
        private const float k_LineWidth = 0.1f;

        /// <summary>
        /// The material of the line
        /// </summary>
        private static Material m_LineRendererMaterial;

        /// <summary>
        /// Add Unlit shader to the line material
        /// </summary>
        public void Awake()
        {
            m_LineRendererMaterial = new Material(Shader.Find("Unlit/Color"));
            
        }
        
        /// <summary>
        /// Create a line with the the line renderer and set this line renderer to the color
        /// </summary>
        ///
        /// <param name="lineRenderer">The lineRenderer to be initialized with two points and lineWidth </param>
        /// <param name="color"> The color to be added on the lineRenderer</param>
        public void InitLine(LineRenderer lineRenderer, Color color )
        {
            lineRenderer.startWidth = k_LineWidth;
            lineRenderer.endWidth = k_LineWidth;
            lineRenderer.positionCount = 2;

            m_LineRendererMaterial.color = color;
        }

        /// <summary>
        /// Update the line renderer by setting the start and end positions.
        /// </summary>
        ///
        /// <param name="lineRenderer">The line renderer to be updated. </param>
        /// <param name="startPosition">The start position of the line renderer.</param>
        /// <param name="endPosition">The end position of the line renderer.</param>
        public void UpdateLine(LineRenderer lineRenderer, Vector3 startPosition, Vector3 endPosition)
        {
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, endPosition);
            lineRenderer.material = m_LineRendererMaterial;
            lineRenderer.enabled = true;
        }
    }
}