using Imperium.Persistence;
using UnityEngine;

namespace Imperium.Navigation
{
    [System.Serializable]
    public abstract class FleetCommand : ISerializable<FleetCommandPersistance>
    {
        public Vector3 destination;
        public float destinationOffset;
        public MapObject target;
        
        protected MapObject source;
        protected ShipController sourceShipController;

        public CommandType commandType;

        protected FleetCommand(MapObject source, MapObject target, Vector3 destination, float destinationOffset, CommandType commandType)
        {
            this.source = source;
            this.target = target;
            this.destination = destination;
            this.destinationOffset = destinationOffset;
            this.commandType = commandType;
            sourceShipController = source.GetComponent<ShipController>();
        }

        protected FleetCommand(MapObject source, Vector3 destination, float destinationOffset, CommandType commandType)
        {
            this.source = source;
            this.destination = destination;
            this.destinationOffset = destinationOffset;
            this.commandType = commandType;
            sourceShipController = source.GetComponent<ShipController>();
        }

        protected FleetCommand(MapObject source, MapObject target, CommandType commandType)
        {
            this.source = source;
            this.target = target;
            this.commandType = commandType;
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

        public FleetCommandPersistance Serialize()
        {
            return new FleetCommandPersistance(destination, destinationOffset, source != null ? source.id : -1, target != null ? target.id : -1, commandType);
        }

        public void SetObject(FleetCommandPersistance serializedObject)
        {
            throw new System.NotImplementedException();
        }   

    }
}