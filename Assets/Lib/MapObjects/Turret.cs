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
        /// How many bullets will the turret fire in 1 second?
        /// </summary>
        public float fireRate;

        /// <summary>
        /// What is the turret's max firing range?
        /// </summary>
        public float range;

        public Turret()
        {
        }

        /// <summary>
        /// Creates a turret model used by turret controllers.
        /// </summary>
        /// <param name="fireRate">The fire rate of the turret in shoot/second</param>
        /// <param name="accuracy">The accuracy of the turret. Must be bettwen 1 and 100</param>
        /// <param name="range">What is the turret's max firing range?</param>
        /// <param name="bulletType">What type of bullet does the turret fire?</param>
        public Turret(float fireRate, int accuracy, float range, BulletType bulletType)
        {
            this.fireRate = fireRate;
            this.accuracy = accuracy;
            this.range = range;
            bullet = BulletFactory.getInstance().CreateBullet(bulletType);
        }
    }
}