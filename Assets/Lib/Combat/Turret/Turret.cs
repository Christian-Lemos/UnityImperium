using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;
namespace Imperium.Combat.Turret
{    
    public class Turret
    {
        /// <summary>
        /// How many bullets will the turret fire in 1 second?
        /// </summary>
        private float fireRate;

        /// <summary>
        /// How accurate is the turret? Must be between 1 and 100
        /// </summary>
        private int accuracy;

        /// <summary>
        /// What bullet does the turret fire?
        /// </summary>
        public Bullet Bullet { get; set; }
        /// <summary>
        /// What is the turret's max firing range?
        /// </summary>
        private float range;

        private readonly BulletFactory factory = BulletFactory.getInstance();


        /// <summary>
        /// Creates a turret model used by turret controllers.
        /// </summary>
        /// <param name="fireRate">The fire rate of the turret in shoot/second</param>
        /// <param name="accuracy">The accuracy of the turret. Must be bettwen 1 and 100</param>
        /// <param name="range">What is the turret's max firing range?</param>
        /// <param name="bulletType">What type of bullet does the turret fire?</param>
        public Turret(float fireRate, int accuracy, float range, BulletType bulletType)
        {
            FireRate = fireRate;
            Accuracy = accuracy;
            Range = range;
            Bullet = factory.CreateBullet(bulletType);
        }

        public float FireRate
        {
            get
            {
                return fireRate;
            }

            set
            {
                if (value > 0)
                {
                    fireRate = value;
                }
                else
                {
                    throw new System.Exception("The fire rate must be higher the 0");
                }

            }
        }

        public int Accuracy
        {
            get
            {
                return accuracy;
            }

            set
            {
                if(value > 0 && value <= 100)
                {
                    accuracy = value;
                }
                else
                {
                    throw new System.Exception("The accuracy must be higher then 0 and lower then 101");
                }
                
            }
        }

        public float Range
        {
            get
            {
                return range;
            }

            set
            {
                if (value > 0)
                {
                    range = value;
                }
                else
                {
                    throw new System.Exception("The range must be higher than 0");
                }
            }
        }
    }
}


