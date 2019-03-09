using UnityEngine;

namespace Imperium.Navigation
{
    public class MoveCommand : FleetCommand
    {

        public MoveCommand(MapObject source, MapObject target) : base(source, target, CommandType.Move)
        {
            
        }

        public MoveCommand(MapObject source, Vector3 destination, float destinationOffset) : base(source, destination, destinationOffset, CommandType.Move)
        {
            
        }

        public override void ExecuteCommand()
        {
            if (base.target != null)
            {
                base.destination = target.transform.position;
            }

            float distance = Vector3.Distance(base.destination, base.source.transform.position);

            if (distance > base.destinationOffset)
            {
                sourceShipController.MoveControl(base.destination);
            }
        }

        public override bool IsFinished()
        {
            float distance = Vector3.Distance(base.destination, base.source.transform.position);
            return distance <= base.destinationOffset;
        }

    }
}