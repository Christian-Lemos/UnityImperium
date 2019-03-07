using System.Collections.Generic;
using UnityEngine;

namespace Imperium.Persistence
{
    [System.Serializable]
    public class AsteroidFieldPersistance
    {
        public AsteroidFieldAsteroidSettingsPersistance AsteroidFieldAsteroidSettingsPersistance;
        public List<AsteroidPersistance> asteroids;
        public bool initialized;
        public Vector3 position;
        public Vector3 size;

        public AsteroidFieldPersistance(AsteroidFieldAsteroidSettingsPersistance asteroidFieldAsteroidSettingsPersistance, List<AsteroidPersistance> asteroids, Vector3 position, Vector3 size, bool initialized)
        {
            AsteroidFieldAsteroidSettingsPersistance = asteroidFieldAsteroidSettingsPersistance;
            this.asteroids = asteroids;
            this.position = position;
            this.size = size;
            this.initialized = initialized;
        }
    }
}