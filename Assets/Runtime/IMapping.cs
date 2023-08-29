using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRC.Students.SU2023.IN03.Chen
{
    /// <summary>
    /// Helpers for mapping the controller position to certain world position by applying a log transformation
    /// </summary>
    public interface IMapping
    {
        /// <summary>
        /// Map the controller position to certain world position (see Transformation for details)
        /// if you're pulling an object towards the main camera
        /// </summary>
        /// <param name="initialPosition">The initial position of controller when Update was last called.</param>
        /// <param name="currentPosition">The current position of the controller when Update is now called.</param>
        public Vector3 ApplyMapping(Vector3 initialPosition, Vector3 currentPosition)
        {
            return Vector3.one;
        }

        /// <summary>
        /// Take in the controller movement amount on each axis
        /// and return the mapped value after applying a log function
        /// </summary>
        ///
        /// <param name="move">The controller movement amount on each axis</param>
        /// <returns> The movement amount after applying log transformation. </returns>
        public static float Transformation(float move)
        {
            return move;
        }
    }
}