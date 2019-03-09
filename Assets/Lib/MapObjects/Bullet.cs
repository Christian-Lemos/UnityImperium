using UnityEngine;

namespace Imperium.MapObjects
{
    public class Bullet
    {
        public Bullet(float speed, int damage, string bulletPrefab)
        {
            this.speed = speed;
            this.damage = damage;
            this.prefab = Resources.Load(bulletPrefab) as GameObject;
        }

        public int damage { get; set; }
        public GameObject prefab { get; set; }
        public float speed { get; set; }
    }
}