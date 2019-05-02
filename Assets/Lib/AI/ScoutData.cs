using Imperium.MapObjects;
using Imperium.Rendering;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Imperium.AI
{
    public class ScoutData
    {
        public HashSet<GameObject> visibleObjetcs = new HashSet<GameObject>();
        public ICollection<StationScout> knowScoutedStations = new HashSet<StationScout>(); 
        public ICollection<AsteroidScout> knowScoutedAsteroids = new HashSet<AsteroidScout>(); 

        public Player[] players;

        public ScoutData(params Player[] players)
        {
            this.players = players;
        }


        public void Update()
        {
            HashSet<GameObject> visibleNow = FogOfWarUtility.GetVisibleObjects(players);

            visibleObjetcs.RemoveWhere((GameObject go) =>
            {
                return go == null;
            });

            foreach (GameObject gameObject in visibleObjetcs)
            {
                if (!visibleNow.Contains(gameObject))
                {
                    if(gameObject.GetComponent<INonExplorable>() == null)
                    {
                        MapObject mapObject = gameObject.GetComponent<MapObject>();
                        switch(mapObject.mapObjectType)
                        {
                            case MapObjectType.Station:
                                StationScout removeStation = null;
                                foreach(StationScout stationScout in knowScoutedStations)
                                {
                                    if(stationScout.mapObjectId == mapObject.id)
                                    {
                                        removeStation = stationScout;
                                        break;
                                    }
                                }
                                if(removeStation != null)
                                {
                                    knowScoutedStations.Remove(removeStation);
                                }
                                break;
                            case MapObjectType.Asteroid:
                                AsteroidScout removeAsteroid = null;
                                foreach(AsteroidScout asteroidScout in knowScoutedAsteroids)
                                {
                                    if(asteroidScout.mapObjectId == mapObject.id)
                                    {
                                        removeAsteroid = asteroidScout;
                                        break;
                                    }
                                }
                                if(removeAsteroid != null)
                                {
                                    knowScoutedAsteroids.Remove(removeAsteroid);
                                }
                                break;
                        }
                    }
                }
            }

            visibleObjetcs = visibleNow;
        }
    }
}