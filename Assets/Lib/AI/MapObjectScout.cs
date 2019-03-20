using UnityEngine;

namespace Imperium.AI
{
    [System.Serializable]
    public class MapObjectScout
    {
        public long mapObjectId;
        public Vector3 position;
        public float time;

        public MapObjectScout(long mapObjectId, Vector3 position, float time)
        {
            this.mapObjectId = mapObjectId;
            this.position = position;
            this.time = time;
        }
    }
}