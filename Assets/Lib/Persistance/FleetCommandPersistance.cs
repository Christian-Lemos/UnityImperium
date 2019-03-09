using Imperium.Navigation;
using UnityEngine;

namespace Imperium.Persistence
{
    [System.Serializable]
    public class FleetCommandPersistance
    {
        public Vector3 destination;
        public float destinationOffset;
        public long sourceID;
        public long targetID;
        public CommandType commandType;

        public FleetCommandPersistance(Vector3 destination, float destinationOffset, long sourceID, long targetID, CommandType commandType)
        {
            this.destination = destination;
            this.destinationOffset = destinationOffset;
            this.sourceID = sourceID;
            this.targetID = targetID;
            this.commandType = commandType;
        }
    }
}