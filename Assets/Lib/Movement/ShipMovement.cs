using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imperium.Navigation
{
    /// <summary>
    /// This class handles movements of ships
    /// </summary>
    public class ShipMovement
    {
        public float MovementSpeed { get; set; }
        public float RotationSpeed { get; set; }

        private Transform transform;

        public ShipMovement(Transform transform, float movementSpeed, float rotationSpeed)
        {
            this.transform = transform;
            this.MovementSpeed = movementSpeed;
            this.RotationSpeed = rotationSpeed;
        }


        /// <summary>
        /// Moves the ship to the destination
        /// </summary>
        /// <param name="destination">The destination</param>
        public void MoveToPosition(Vector3 destination)
        {
            Quaternion desRotation = Quaternion.LookRotation(destination - transform.position, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desRotation, RotationSpeed * Time.deltaTime);
            transform.position += transform.forward * MovementSpeed * Time.deltaTime;   
        }


    }
}

