using UnityEngine;

namespace Imperium.Combat.Turret
{
    public class Bullet
    {
        public Bullet(float speed, int damage, string bulletPrefab)
        {
            Speed = speed;
            Damage = damage;
            Prefab = Resources.Load(bulletPrefab) as GameObject;
        }

        public int Damage { get; set; }
        public GameObject Prefab { get; set; }
        public float Speed { get; set; }
    }
}