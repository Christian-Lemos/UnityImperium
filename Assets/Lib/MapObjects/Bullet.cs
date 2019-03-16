using System.Collections.Generic;
using UnityEngine;

namespace Imperium.MapObjects
{
    [System.Serializable]
    public class Bullet
    {
        public string bulletPrefabName;

        public BulletType bulletType;
        public int damage;

        public GameObject prefab;

        public float speed;
        private static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

        public Bullet(float speed, int damage, string bulletPrefabName, BulletType bulletType)
        {
            this.speed = speed;
            this.damage = damage;
            this.bulletPrefabName = bulletPrefabName;
            this.bulletType = bulletType;
            LoadPrefab(bulletPrefabName);
        }

        public GameObject LoadPrefab(string prefabName)
        {
            if (prefabs.ContainsKey(prefabName))
            {
                prefab = prefabs[bulletPrefabName];
            }
            else
            {
                prefab = Resources.Load(bulletPrefabName) as GameObject;
                prefabs.Add(bulletPrefabName, prefab);
            }
            return prefab;
        }
    }
}