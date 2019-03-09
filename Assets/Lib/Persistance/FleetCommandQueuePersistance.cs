using System.Collections.Generic;

namespace Imperium.Persistence
{
    [System.Serializable]
    public class FleetCommandQueuePersistance
    {
        public int currentFleetCommand;
        public List<FleetCommandPersistance> fleetCommands = new List<FleetCommandPersistance>();
        public bool loopFleetCommands = false;

        public FleetCommandQueuePersistance(int currentFleetCommand, List<FleetCommandPersistance> fleetCommands, bool loopFleetCommands)
        {
            this.currentFleetCommand = currentFleetCommand;
            this.fleetCommands = fleetCommands;
            this.loopFleetCommands = loopFleetCommands;
        }
    }
}