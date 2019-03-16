using Imperium.Persistence;
using System.Collections.Generic;
using UnityEngine;
namespace Imperium.Navigation
{
    [System.Serializable]
    public class FleetCommandQueue : ISerializable<FleetCommandQueuePersistance>
    {
        public List<FleetCommand> fleetCommands = new List<FleetCommand>();
        public bool loopFleetCommands = false;
        private FleetCommand currentFleetCommand;

        public FleetCommand CurrentFleetCommand
        {
            get
            {
                return currentFleetCommand;
            }

            set
            {
                if (currentFleetCommand != null)
                {
                    currentFleetCommand.OnRemoved();
                }
                currentFleetCommand = value;
            }
        }

        public FleetCommand GetNextFleetCommand()
        {
            if (loopFleetCommands)
            {
                if (fleetCommands.Count <= 1)
                {
                    return null;
                }

                int currentIndex = fleetCommands.FindIndex((FleetCommand fleetCommand) =>
                {
                    return fleetCommand.Equals(CurrentFleetCommand);
                });

                if (currentIndex == fleetCommands.Count - 1)
                {
                    return fleetCommands[0];
                }
                else
                {
                    return fleetCommands[currentIndex + 1];
                }
            }
            else
            {
                return (fleetCommands.Count > 1) ? fleetCommands[1] : null;
            }
        }

        public void ResetCommands()
        {
            fleetCommands.Clear();
            CurrentFleetCommand = null;
        }

        public FleetCommandQueuePersistance Serialize()
        {
            List<FleetCommandPersistance> fleetCommandPersistances = new List<FleetCommandPersistance>();
            int currentCommandIndex = 0;
            int i = 0;
            foreach (FleetCommand fleetCommand in fleetCommands)
            {
                fleetCommandPersistances.Add(fleetCommand.Serialize());
                if (fleetCommand.Equals(currentFleetCommand))
                {
                    currentCommandIndex = i;
                }
                else
                {
                    i++;
                }
            }
            return new FleetCommandQueuePersistance(currentCommandIndex, fleetCommandPersistances, loopFleetCommands);
        }

        public FleetCommand SetNextFleetCommand()
        {
            FleetCommand fleetCommand = GetNextFleetCommand();

            CurrentFleetCommand = fleetCommand;

            if (!loopFleetCommands)
            {
                fleetCommands.RemoveAt(0);
            }

            return fleetCommand;
        }

        public ISerializable<FleetCommandQueuePersistance> SetObject(FleetCommandQueuePersistance serializedObject)
        {
            this.loopFleetCommands = serializedObject.loopFleetCommands;
            foreach(FleetCommandPersistance fleetCommandPersistance in serializedObject.fleetCommands)
            {
                fleetCommands.Add(fleetCommandPersistance.ToFleetCommand());

            }
            if(fleetCommands.Count > 0)
            {
                currentFleetCommand = fleetCommands[serializedObject.currentFleetCommand];
            }
            
            return this;
        }
    }
}