using Imperium.MapObjects;

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

        public StationControllerPersistance(bool constructed, float constructionProgress, MapObjectPersitance mapObjectPersitance, Station station, StationType stationType)
        {
            this.constructed = constructed;
            this.constructionProgress = constructionProgress;
            this.mapObjectPersitance = mapObjectPersitance;
            this.station = station;
            this.stationType = stationType;
        }
    }
}