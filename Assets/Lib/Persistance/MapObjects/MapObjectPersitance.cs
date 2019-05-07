using UnityEngine;

namespace Imperium.Persistence.MapObjects
{
    [System.Serializable]
    public class MapObjectPersitance
    {
        public long id;
        public Vector3 localPosition;
        public Vector3 localScale;
        public Quaternion localRotation;
        public long parentId;

        public MapObjectPersitance(long id, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, long parentId)
        {
            this.id = id;
            this.localPosition = localPosition;
            this.localScale = localScale;
            this.localRotation = localRotation;
            this.parentId = parentId;
        }
    }
}