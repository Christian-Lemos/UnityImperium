using Imperium.MapObjects;
using UnityEngine;

namespace Imperium.AI
{
    [System.Serializable]
    public class StationScout : MapObjectScout
    {
        public Station station;
        public StationType stationType;
        public float constructionProgress;

        public StationScout(long mapObjectId, float time, Vector3 position, Station station, StationType stationType, float constructionProgress) : base(mapObjectId, position, time)
        {
            this.station = station;
            this.stationType = stationType;
            this.constructionProgress = constructionProgress;
        }

        public bool Constructed
        {
            get
            {
                return constructionProgress >= 100;
            }
        }

    }
        
}