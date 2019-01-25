using UnityEngine;
using System.Collections;

namespace Imperium.Navigation
{

    public class BuildCommand : FleetCommand
    {
        private StationController targetStationController;
        private StationConstructor stationConstructor;

        public BuildCommand(GameObject source, GameObject target, ShipMovement shipMovement) : base(source, target, shipMovement)
        {
            this.targetStationController = target.GetComponent<StationController>();
            this.stationConstructor = source.GetComponent<StationConstructor>();
            base.destinationOffset = 2f;
        }

        public override void ExecuteCommand()
        {
            base.destination = base.target.transform.position;

            float distance = Vector3.Distance(base.destination, base.source.transform.position);

            if (distance > base.destinationOffset)
            {
                shipMovement.MoveToPosition(base.destination);
            }
            else if (targetStationController.constructed == true)
            {
                stationConstructor.StopBuilding();
            }
            else
            {
                if (stationConstructor.Building == false)
                {
                    stationConstructor.StartBuilding(base.target);
                }
            }
        }

        public override bool IsFinished()
        {
            return targetStationController.constructed == true;
        }
    }

}
