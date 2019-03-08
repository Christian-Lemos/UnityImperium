using UnityEngine;

namespace Imperium.Navigation
{
    public abstract class FleetCommand
    {
        public Vector3 destination;
        public float destinationOffset;
        public GameObject target;
        protected GameObject source;
        protected ShipController sourceShipController;
        protected FleetCommand(GameObject source, GameObject target, Vector3 destination, float destinationOffset)
        {
            this.source = source;
            this.target = target;
            this.destination = destination;
            this.destinationOffset = destinationOffset;
            sourceShipController = source.GetComponent<ShipController>();
        }

        protected FleetCommand(GameObject source, Vector3 destination, float destinationOffset)
        {
            this.source = source;
            this.destination = destination;
            this.destinationOffset = destinationOffset;
            sourceShipController = source.GetComponent<ShipController>();
        }

        protected FleetCommand(GameObject source, GameObject target)
        {
            this.source = source;
            this.target = target;
            sourceShipController = source.GetComponent<ShipController>();
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