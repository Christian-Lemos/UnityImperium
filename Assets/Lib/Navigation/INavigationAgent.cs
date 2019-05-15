using Imperium.Navigation;
using UnityEngine;

namespace Assets.Lib.Navigation
{
    public interface INavigationAgent
    {
        FleetCommandQueue FleetCommandQueue { get; }

        void AddCommand(bool resetCommands, FleetCommand fleetCommand);

        void AttackTarget(GameObject target, bool resetCommands, bool loopCommands);

        void BuildStation(GameObject station, bool resetCommands, bool loopCommands);

        void MineAsteroid(GameObject asteroid, bool resetCommands);

        void MoveControl(Vector3 destination);

        void MoveToPosition(Vector3 destination, float destinationOffset, bool resetCommands, bool loopCommands);

        void SetIdle();
    }
}