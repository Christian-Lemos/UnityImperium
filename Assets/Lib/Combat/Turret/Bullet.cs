using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imperium.Combat.Turret
{
    public class Bullet
    {
        public float Speed { get; set; }
        public int Damage { get; set; }
        public GameObject Prefab { get; set; }

        public Bullet(float speed, int damage, string bulletPrefab)
        {
            Speed = speed;
            Damage = damage;
            Prefab = Resources.Load(bulletPrefab) as GameObject;
        }
    }
}
