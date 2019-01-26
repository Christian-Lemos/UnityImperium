using UnityEngine;

namespace Imperium.Navigation
{
    /// <summary>
    /// This class handles movements of ships
    /// </summary>
    public class ShipMovement
    {
        private Transform transform;

        public ShipMovement(Transform transform, float movementSpeed, float rotationSpeed)
        {
            this.transform = transform;
            MovementSpeed = movementSpeed;
            RotationSpeed = rotationSpeed;
        }

        public float MovementSpeed { get; set; }
        public float RotationSpeed { get; set; }

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