using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRC.Students.SU2023.IN03.Chen
{
    /// <summary>
    /// Update the cube so that it always rotates towards the main camera
    /// </summary>
    public interface IRotateTowards
    {
        /// <summary>
        /// Update the cube so that it always rotates towards the main camera
        /// </summary>
        /// <param name="camera">The main camera, center of rotation</param>
        /// <param name="interactable">The transform of the interactable rotating towards the camera</param>
        void ApplyRotateTowards(Transform camera, Transform interactable)
        {

        }
    }
}
