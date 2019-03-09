using System.Collections.Generic;
using UnityEngine;

namespace Imperium.Persistence.MapObjects
{
    [System.Serializable]
    public class AsteroidFieldControllerPersistance
    {
        public AsteroidFieldAsteroidSettingsPersistance AsteroidFieldAsteroidSettingsPersistance;
        public List<AsteroidControllerPersistance> asteroids;
        public bool initialized;
        public MapObjectPersitance mapObjectPersitance;
        public Vector3 size;

        public AsteroidFieldControllerPersistance(AsteroidFieldAsteroidSettingsPersistance asteroidFieldAsteroidSettingsPersistance, List<AsteroidControllerPersistance> asteroids, bool initialized, MapObjectPersitance mapObjectPersitance, Vector3 size)
        {
            AsteroidFieldAsteroidSettingsPersistance = asteroidFieldAsteroidSettingsPersistance;
            this.asteroids = asteroids;
            this.initialized = initialized;
            this.mapObjectPersitance = mapObjectPersitance;
            this.size = size;
        }
    }
}