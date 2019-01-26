using UnityEngine;

namespace Imperium.Navigation
{
    public abstract class FleetCommand
    {
        public Vector3 destination;
        public float destinationOffset;
        public GameObject target;
        protected ShipMovement shipMovement;
        protected GameObject source;

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

        public virtual void OnRemoved()
        {
        }

        public override string ToString()
        {
            return base.ToString() + this.destination;
        }
    }
}