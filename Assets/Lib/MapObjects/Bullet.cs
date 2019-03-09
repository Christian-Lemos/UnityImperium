using System.Collections.Generic;
using UnityEngine;

namespace Imperium.MapObjects
{
    [System.Serializable]
    public class Bullet
    {
        public string bulletPrefabName;

        public int damage;

        public GameObject prefab;

        public float speed;

        private static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

        public Bullet(float speed, int damage, string bulletPrefabName)
        {
            this.speed = speed;
            this.damage = damage;
            this.bulletPrefabName = bulletPrefabName;
            LoadPrefab();
        }

        public GameObject LoadPrefab()
        {
            if (prefabs.ContainsKey(bulletPrefabName))
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