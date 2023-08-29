using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRC.Students.SU2023.IN03.Chen
{
    /// <summary>
    /// Create a cube (prefab) at the position of the the sourceCube
    /// </summary>
    public interface ICreateCube
    {
        /// <summary>
        /// Create a cube (prefab) at the position of the the sourceCube
        /// </summary>
        /// <param name="cube">the gameObject containing the target position of adding a new cube</param>
        public void AddCube(GameObject cube)
        {

        }
    }
}