using UnityEngine;

namespace Imperium.Navigation
{
    public class AttackCommand : FleetCommand
    {
        private MapObjectCombatter mapObjectCombatter;

        public AttackCommand(MapObject source, MapObject target) : base(source, target, CommandType.Attack)
        {
            mapObjectCombatter = source.GetComponent<MapObjectCombatter>();
            base.destination = target.transform.position;
            destinationOffset = mapObjectCombatter.lowestTurretRange / 2;
        }

        public override void ExecuteCommand()
        {
            if (base.target != null)
            {
                base.destination = target.transform.position;
                destinationOffset = mapObjectCombatter.lowestTurretRange / 2;

                if (Vector3.Distance(target.transform.position, source.transform.position) <= sourceShipController.Ship.combatStats.fieldOfViewDistance)
                {
                    sourceShipController.FireTurrets(base.target.gameObject);
                }

                float distance = Vector3.Distance(base.destination, base.source.transform.position);

                if (distance > base.destinationOffset)
                {
                    sourceShipController.MoveControl(base.destination);
                }
            }
        }

        public override bool IsFinished()
        {
            return base.target == null;
        }
    }
}