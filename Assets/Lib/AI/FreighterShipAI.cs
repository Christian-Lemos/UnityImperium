using System.Collections.Generic;
using UnityEngine;

namespace Imperium.AI
{
    public class FreighterShipAI : UnitLevelAI
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
            if (shipController.fleetCommandQueue.CurrentFleetCommand == null)
            {
                Afk = true;
            }

            if (Afk)
            {
                ICollection<GameObject> visibleObjects = strategicAI.scoutData.visibleObjetcs;

                AsteroidController asteroidController = null;
                foreach (GameObject gameObject in visibleObjects)
                {
                    asteroidController = gameObject.GetComponent<AsteroidController>();
                    if (asteroidController != null)
                    {
                        break;
                    }
                }

                if (asteroidController != null)
                {
                    shipController.MineAsteroid(asteroidController.gameObject, true);
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