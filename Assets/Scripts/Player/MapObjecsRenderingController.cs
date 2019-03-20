using Imperium.AI;
using Imperium.MapObjects;
using Imperium.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjecsRenderingController : MonoBehaviour
{
    public static MapObjecsRenderingController Instance;
    public int[] players;
    public ScoutData scoutData;
    public ICollection<GameObject> visibleObjects = new HashSet<GameObject>();

    private HashSet<DummyRealGameObjectAssociation> dummyRealGameObjectAssociations = new HashSet<DummyRealGameObjectAssociation>();

    private IEnumerator RenderEnumerator()
    {
        while (true)
        {
            ICollection<GameObject> visibleNow = FogOfWarUtility.GetVisibleObjects(players);

            foreach (GameObject gameObject in visibleObjects)
            {
                if (gameObject != null && !visibleNow.Contains(gameObject))
                {
                    if (gameObject.GetComponent<INonExplorable>() == null)
                    {
                        MapObject mapObject = gameObject.GetComponent<MapObject>();
                        switch (mapObject.mapObjectType)
                        {
                            case MapObjectType.Station:
                                StationController stationController = gameObject.GetComponent<StationController>();
                                GameObject station = Spawner.Instance.SpawnDummyStation(stationController.stationType, stationController.Station, gameObject.transform.position, gameObject.transform.rotation, stationController.constructionProgress, true);
                                dummyRealGameObjectAssociations.Add(new DummyRealGameObjectAssociation(gameObject, station));
                                break;

                            case MapObjectType.Asteroid:
                                AsteroidController asteroidController = gameObject.GetComponent<AsteroidController>();
                                GameObject asteroid = Spawner.Instance.SpawnDummyAsteroid(asteroidController.prefabIndex, asteroidController.resourceType, asteroidController.ResourceQuantity, gameObject.transform.position, gameObject.transform.rotation, true);
                                dummyRealGameObjectAssociations.Add(new DummyRealGameObjectAssociation(gameObject, asteroid));
                                break;
                        }
                    }

                    FogOfWarUtility.SetRendering(false, gameObject);
                }
            }

            foreach (GameObject gameObject in visibleNow)
            {
                if (!visibleObjects.Contains(gameObject))
                {
                    FogOfWarUtility.SetRendering(true, gameObject);
                    //if(dummyRealGameObjectAssociations.Conta)

                    dummyRealGameObjectAssociations.RemoveWhere((DummyRealGameObjectAssociation drgoa) =>
                    {
                        bool remove = drgoa.real.Equals(gameObject);
                        if (remove)
                        {
                            Destroy(drgoa.dummy);
                        }
                        return remove;
                    });
                }
            }

            dummyRealGameObjectAssociations.RemoveWhere((DummyRealGameObjectAssociation drgoa) =>
            {
                bool remove = drgoa.real == null;
                if (remove)
                {
                    Destroy(drgoa.dummy);
                }
                return remove;
            });

            visibleObjects = visibleNow;

            dummyRealGameObjectAssociations.RemoveWhere((DummyRealGameObjectAssociation drgoa) =>
            {
                bool remove = drgoa.real.Equals(gameObject);
                if (remove)
                {
                    Destroy(drgoa.dummy);
                }
                return remove;
            });
            yield return null;
            yield return null;
            yield return null;
        }
    }

    private void Start()
    {
        scoutData = new ScoutData(players);
        Instance = this;
        StartCoroutine(RenderEnumerator());
    }

    private class DummyRealGameObjectAssociation
    {
        public GameObject dummy;
        public GameObject real;

        public DummyRealGameObjectAssociation(GameObject real, GameObject dummy)
        {
            this.real = real;
            this.dummy = dummy;
        }
    }
}