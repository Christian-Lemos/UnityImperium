using Imperium.MapObjects;
using System.Collections.Generic;

namespace Imperium.Persistence.MapObjects
{
    [System.Serializable]
    public class StationControllerPersistance
    {
        public bool constructed;
        public float constructionProgress;
        public MapObjectPersitance mapObjectPersitance;
        public Station station;
        public StationType stationType;
        public List<TurretControllerPersistance> turretControllerPersistances = new List<TurretControllerPersistance>();
        public bool initialized;

        public StationControllerPersistance(bool constructed, float constructionProgress, MapObjectPersitance mapObjectPersitance, Station station, StationType stationType, bool initialized)
        {
            this.constructed = constructed;
            this.constructionProgress = constructionProgress;
            this.mapObjectPersitance = mapObjectPersitance;
            this.station = station;
            this.stationType = stationType;
            this.initialized = initialized;
        }

        public StationControllerPersistance(bool constructed, float constructionProgress, MapObjectPersitance mapObjectPersitance, Station station, StationType stationType, List<TurretControllerPersistance> turretControllerPersistances, bool initialized)
        {
            this.constructed = constructed;
            this.constructionProgress = constructionProgress;
            this.mapObjectPersitance = mapObjectPersitance;
            this.station = station;
            this.stationType = stationType;
            this.turretControllerPersistances = turretControllerPersistances;
            this.initialized = initialized;
        }
    }
}