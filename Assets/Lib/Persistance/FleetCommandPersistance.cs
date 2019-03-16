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

        public FleetCommand ToFleetCommand()
        {
            switch(this.commandType)
            {
                case CommandType.Attack:
                    return new AttackCommand(MapObject.FindByID(sourceID), MapObject.FindByID(targetID));
                case CommandType.Build:
                    return new BuildCommand(MapObject.FindByID(sourceID), MapObject.FindByID(targetID));
                case CommandType.Mine:
                    return new MineCommand(MapObject.FindByID(sourceID), MapObject.FindByID(targetID));
                case CommandType.Move:
                    if(targetID == -1)
                    {
                        return new MoveCommand(MapObject.FindByID(sourceID), this.destination, this.destinationOffset);
                    }
                    return new MoveCommand(MapObject.FindByID(sourceID), MapObject.FindByID(targetID));
                default:
                    return null;
            }
        }
    }
}