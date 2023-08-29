using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRC.Students.SU2023.IN03.Chen
{
    /// <summary>
    /// Smooth the child cube position towards the parent cube position
    /// </summary>
    public interface ISmoothDamp
    {
        /// <summary>
        /// Smooth the child cube position towards the parent cube position
        /// </summary>
        /// 
        /// <param name="parent">The parent cube transform</param>
        /// <param name="self">The child cube transform</param>
        /// <param name="initialPosition">The initial position of the child cube when Update was last called.</param>
        /// <returns> The position of the child cube after applying smooth damp. </returns>
        Vector3 ApplySmoothDamp(Transform parent, Transform self, Vector3 initialPosition)
        {
            return Vector3.one;
        }
    }
}