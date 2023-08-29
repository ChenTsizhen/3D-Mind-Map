using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRC.Students.SU2023.IN03.Chen
{
    /// <summary>
    /// Create and Update the lines connecting cubes with potential grouping relationships
    /// and those connecting established parents and children.
    /// </summary>
    public interface ILine
    {
        /// <summary>
        /// Create a line with the the line renderer and set this line renderer to the color
        /// </summary>
        ///
        /// <param name="lineRenderer">The lineRenderer to be initialized with two points and lineWidth </param>
        /// <param name="color"> The color to be added on the lineRenderer</param>
        void InitLine(LineRenderer lineRenderer, Color color )
        {
            
        }

        /// <summary>
        /// Update the line renderer by setting the start and end positions.
        /// </summary>
        ///
        /// <param name="lineRenderer">The line renderer to be updated. </param>
        /// <param name="startPosition">The start position of the line renderer.</param>
        /// <param name="endPosition">The end position of the line renderer.</param>
        void UpdateLine(LineRenderer lineRenderer, Vector3 startPosition, Vector3 endPosition)
        {
            
        }
    }
}