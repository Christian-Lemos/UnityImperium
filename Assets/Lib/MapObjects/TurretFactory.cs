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
                    return new Turret(5f, 70, 40f, 2, 0.75f, BulletType.Test);

                default:
                    throw new System.Exception("Turret type not supported");
            }
        }
    }
}