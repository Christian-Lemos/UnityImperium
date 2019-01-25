using UnityEngine;
using System.Collections;

namespace Imperium.Navigation
{

    public abstract class FleetCommand
    {
        protected GameObject source;
        public GameObject target;
        public Vector3 destination;
        public float destinationOffset;
        protected ShipMovement shipMovement;

        protected FleetCommand(GameObject source, GameObject target, Vector3 destination, float destinationOffset, ShipMovement shipMovement)
        {
            this.source = source;
            this.target = target;
            this.destination = destination;
            this.destinationOffset = destinationOffset;
            this.shipMovement = shipMovement;
        }

        protected FleetCommand(GameObject source, Vector3 destination, float destinationOffset, ShipMovement shipMovement)
        {
            this.source = source;
            this.destination = destination;
            this.destinationOffset = destinationOffset;
            this.shipMovement = shipMovement;
        }

        protected FleetCommand(GameObject source, GameObject target, ShipMovement shipMovement)
        {
            this.source = source;
            this.target = target;
            this.shipMovement = shipMovement;
        }

        public abstract void ExecuteCommand();
        public abstract bool IsFinished();

        


    }

}
