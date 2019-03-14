using Imperium.Misc;

namespace Imperium.MapObjects
{
    public class TurretFactory : Singleton<TurretFactory>
    {
        private TurretFactory()
        {
        }

        public Turret CreateTurret(TurretType turretType)
        {
            switch (turretType)
            {
                case TurretType.Test:
                    return new Turret(0.6f, 70, 40f, BulletType.Test);

                default:
                    throw new System.Exception("Turret type not supported");
            }
        }
    }
}