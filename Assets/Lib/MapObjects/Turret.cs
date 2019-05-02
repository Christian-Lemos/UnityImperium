namespace Imperium.MapObjects
{
    [System.Serializable]
    public class Turret
    {
        /// <summary>
        /// How accurate is the turret? Must be between 1 and 100
        /// </summary>
        public int accuracy;

        public Bullet bullet;

        /// <summary>
        /// How many secounds it takes for the turret to reload
        /// </summary>
        public float salvoReloadTime;

        /// <summary>
        /// What is the turret's max firing range?
        /// </summary>
        public float range;


        public int shotsPerSalvo;
        public float shotReloadTime;

        public Turret()
        {
        }

        /// <summary>
        /// Creates a turret model used by turret controllers.
        /// </summary>
        /// <param name="salvoReloadTime">The fire rate of the turret in shoot/second</param>
        /// <param name="accuracy">The accuracy of the turret. Must be bettwen 1 and 100</param>
        /// <param name="range">What is the turret's max firing range?</param>
        /// <param name="bulletType">What type of bullet does the turret fire?</param>
        public Turret(float salvoReloadTime, int accuracy, float range, int shotsPerSalvo, float shotReloadTime, BulletType bulletType)
        {
            this.salvoReloadTime = salvoReloadTime;
            this.accuracy = accuracy;
            this.range = range;
            this.shotsPerSalvo = shotsPerSalvo;
            this.shotReloadTime = shotReloadTime;
            bullet = BulletFactory.getInstance().CreateBullet(bulletType);
        }
    }
}