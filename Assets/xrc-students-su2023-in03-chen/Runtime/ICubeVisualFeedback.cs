using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRC.Students.SU2023.IN03.Chen
{
    /// <summary>
    /// The visual feedback of cube after its state changes between parent and child.
    /// </summary>
    public interface ICubeVisualFeedback
    {
        /// <summary>
        /// Set the renderer of the cube to the scale
        /// </summary>
        /// <param name="outerCube">The outer cube containing the renderer</param>
        /// <param name="scale">The desired scale</param>
        void SetScale(Transform outerCube, Vector3 scale)
        {
            
        }

        /// <summary>
        /// Set the color of the newCube to be the same as sourceCube
        /// </summary>
        /// <param name="sourceCube">the cube to get color from </param>
        /// <param name="newCube">the cube to add color to /param>
        void SetColor(GameObject sourceCube, GameObject newCube)
        {
            
        }
    }
}