using Imperium.Enum;
using UnityEngine;

namespace Imperium.Persistence
{
    [System.Serializable]
    public class ShipPersistence
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public ShipType shipType;

        public ShipPersistence(Vector3 position, Vector3 rotation, Vector3 scale, ShipType shipType)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.shipType = shipType;
        }
    }
}