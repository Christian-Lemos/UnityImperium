using UnityEngine;

namespace Imperium.Navigation
{
    public class BuildCommand : FleetCommand
    {
        private StationConstructor stationConstructor;
        private StationController targetStationController;

        public BuildCommand(MapObject source, MapObject target) : base(source, target, CommandType.Build)
        {
            targetStationController = target.GetComponent<StationController>();
            stationConstructor = source.GetComponent<StationConstructor>();
            base.destinationOffset = 2f;
        }

        public override void ExecuteCommand()
        {
            if(base.target != null)
            {
                base.destination = base.target.transform.position;

                float distance = Vector3.Distance(base.destination, base.source.transform.position);

                if (distance > base.destinationOffset)
                {
                    sourceShipController.MoveControl(base.destination);
                }
                else if (targetStationController.Constructed == true)
                {
                    stationConstructor.StopBuilding();
                }
                else
                {
                    if (stationConstructor.building == false)
                    {
                        stationConstructor.StartBuilding(base.target.gameObject);
                    }
                }
            }
        }

        public override bool IsFinished()
        {
            return base.target == null || targetStationController.Constructed == true;
        }

        public override void OnRemoved()
        {
            stationConstructor.StopBuilding();
        }
    }
}