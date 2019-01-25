using UnityEngine;
using System.Collections;

namespace Imperium.Navigation
{

    public class MoveCommand : FleetCommand
    {
        private float distance;

        public MoveCommand(GameObject source, GameObject target, ShipMovement shipMovement) : base(source, target, shipMovement)
        {
            this.distance = Vector3.Distance(base.destination, base.source.transform.position);
        }

        public MoveCommand(GameObject source, Vector3 destination, float destinationOffset, ShipMovement shipMovement) : base(source, destination, destinationOffset, shipMovement)
        {
            this.distance = Vector3.Distance(base.destination, base.source.transform.position);
        }

        public override void ExecuteCommand()
        {
            if(base.target != null)
            {
                base.destination = target.transform.position;
            }

            this.distance = Vector3.Distance(base.destination, base.source.transform.position);

            if (this.distance > base.destinationOffset)
            {
                this.shipMovement.MoveToPosition(base.destination);
            }
        }

        public override bool IsFinished()
        {
            
            return this.distance <= base.destinationOffset;
        }
    }

}
