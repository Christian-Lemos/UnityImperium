﻿using System.Collections.Generic;
using UnityEngine;

namespace Imperium.Navigation
{
    public class FleetCommandQueue
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
    }
}