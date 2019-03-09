using Imperium.MapObjects;

namespace Imperium.Persistence.MapObjects
{
    [System.Serializable]
    public class BulletControllerPersistance
    {
        public Bullet bullet;
        public bool initialized;
        public MapObjectPersitance mapObjectPersitance;
        public long sourceID;

        public BulletControllerPersistance(Bullet bullet, bool initialized, MapObjectPersitance mapObjectPersitance, long sourceID)
        {
            this.bullet = bullet;
            this.initialized = initialized;
            this.mapObjectPersitance = mapObjectPersitance;
            this.sourceID = sourceID;
        }
    }
}