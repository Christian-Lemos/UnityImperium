using System.Collections.Generic;
using UnityEngine;

namespace Imperium.AI
{
    public class MilitaryShipAI : UnitLevelAI
    {
        [SerializeField]
        private bool _afk;

        private ShipController shipController;

        public bool Afk
        {
            get
            {
                return _afk;
            }
            set
            {
                _afk = value;
                if (value == true)
                {
                    shipController.SetIdle();
                }
            }
        }

        public override void Execute()
        {
            if (shipController._fleetCommandQueue.CurrentFleetCommand == null)
            {
                Afk = true;
            }

            if (Afk)
            {
                ICollection<GameObject> visibleObjects = strategicAI.scoutData.visibleObjetcs;

                ShipController target = null;
                float lowestPointDifference = 99900009f;
                foreach (GameObject gameObject in visibleObjects)
                {
                    if(PlayerDatabase.Instance.GetObjectPlayer(gameObject) == this.player)
                    {
                        continue;
                    }

                    ShipController targetShipController = gameObject.GetComponent<ShipController>();

                    if (targetShipController != null)
                    {
                        float thisShipCombatPoints = shipController.Ship.combatStats.HP + shipController.Ship.combatStats.Shields;
                        float targetShipCombatPoints = targetShipController.Ship.combatStats.HP + targetShipController.Ship.combatStats.Shields;
                        float diference = thisShipCombatPoints - targetShipCombatPoints;

                        if(diference >= 0  && diference < lowestPointDifference)
                        {
                            lowestPointDifference = diference;
                            target = targetShipController;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                if (target != null)
                {
                    shipController.AttackTarget(target.gameObject, false, true);
                    Afk = false;
                }
            }
        }

        private new void Start()
        {
            base.Start();
            shipController = GetComponent<ShipController>();
        }
    }
}