using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Misc;
using Imperium.Enum;
namespace Imperium.Combat.Turret
{
    public class TurretFactory : Singleton<TurretFactory>
    {
        private TurretFactory() { }
            
        public Turret CreateTurret(TurretType turretType)
        {
            switch(turretType)
            {
                case TurretType.Test:
                    return new Turret(0.5f, 70, 15f, BulletType.Test);
                default:
                    throw new System.Exception("Turret type not supported");
            }
        }
    }
}

