using UnityEngine;

namespace Imperium.Navigation
{
    public class MoveCommand : FleetCommand
    {

        public MoveCommand(GameObject source, GameObject target, ShipMovement shipMovement) : base(source, target, shipMovement)
        {
            
        }

        public MoveCommand(GameObject source, Vector3 destination, float destinationOffset, ShipMovement shipMovement) : base(source, destination, destinationOffset, shipMovement)
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
                shipMovement.MoveToPosition(base.destination);
            }
        }

        public override bool IsFinished()
        {
            float distance = Vector3.Distance(base.destination, base.source.transform.position);
            return distance <= base.destinationOffset;
        }

    }
}