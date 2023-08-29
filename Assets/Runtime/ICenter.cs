using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRC.Students.SU2023.IN03.Chen
{
	/// <summary>
    /// Interprets properties of the parent cube.
    /// Default is disabled. This script is only enabled when it received set parent input.
    /// </summary>
    public interface ICenter
	{
		/// <summary>
        /// Add the child cube to the parent it connects to
        /// </summary>
		/// <param name="childCube">The child cube.</param>
		// void AddCube(GameObject childCube)
  //       {
  //
  //       }

		/// <summary>
        /// Spread the children cubes evenly around the center cube.
        /// </summary>
		void OrderChildren()
        {
            
        }

		/// <summary>
        /// Empty the child list of this parent cube
        /// </summary>
		void Clear()
		{
			
		}
		
		/// <summary>
        /// Detach this child cube from its parent cube
        /// </summary>
		/// <param name="child">The child cube to be detached from parent.</param>
		void DetachChildHelper(GameObject child)
		{
			
		}
		
		/// <summary>
        /// Detach this child cube from its parent cube
        /// and remove this child from the child list
        /// </summary>
        /// <param name="child">The child cube to be detached from parent.</param>
		void Detach(GameObject child){
			
		}
	}
}
