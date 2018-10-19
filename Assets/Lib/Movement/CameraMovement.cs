using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imperium.Movement
{

    /// <summary>
    /// This class is responsable for the execution the cameras movement
    /// </summary>
    public class CameraMovement
    {
        /// <summary>
        /// The camera's movement speed
        /// </summary>
        private readonly float movementSpeed = 30f; 

        private Transform transform;

        /// <param name="transform">The transform component of the camera's gameObject</param>
        public CameraMovement(Transform transform)
        {
            this.transform = transform;
        }

        /// <summary>
        /// Move the transform's gameObject 
        /// </summary>
        /// <param name="horizontalInput">The horizontal Input of Input.GetAxis or Input.GetAxisRaw</param>
        /// <param name="verticalInput">The vertical INput of Input.GetAxis or Input.getAxisRaw></param>
        /// <example>
        /// <code>
        /// float h = Input.GetAxis("Horizontal");
        /// float v = Input.GetAxis("Vertical");
        /// CameraMovement.Move(h, v);
        /// </code>
        /// </example>
        public void Move(float horizontalInput, float verticalInput)
        {
            float x_movement = horizontalInput * movementSpeed * Time.deltaTime;
            float y_movement = verticalInput * movementSpeed * Time.deltaTime;

            transform.Translate(x_movement, y_movement, 0);
        }
    }
}


