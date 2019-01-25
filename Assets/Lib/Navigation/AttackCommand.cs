using UnityEngine;
using System.Collections;

namespace Imperium.Navigation
{

    public class AttackCommand : FleetCommand
    {
        private ShipController shipController;

        public AttackCommand(GameObject source, GameObject target, ShipMovement shipMovement) : base(source, target, shipMovement)
        {
            this.shipController = source.GetComponent<ShipController>();
            base.destination = target.transform.position;
            this.destinationOffset = this.shipController.lowestTurretRange / 2;
        }

        public override void ExecuteCommand()
        {
            if (base.target != null)
            {
                base.destination = target.transform.position;
                this.destinationOffset = this.shipController.lowestTurretRange / 2;
                

                if (Vector3.Distance(target.transform.position, source.transform.position) <= this.shipController.Ship.ShipStats.FieldOfViewDistance)
                {
                    shipController.FireTurrets(base.target);
                }

                float distance = Vector3.Distance(base.destination, base.source.transform.position);

                if (distance > base.destinationOffset)
                {
                    this.shipMovement.MoveToPosition(base.destination);
                }
            }
        }

        public override bool IsFinished()
        {
            return base.target == null;
        }
    }

}
