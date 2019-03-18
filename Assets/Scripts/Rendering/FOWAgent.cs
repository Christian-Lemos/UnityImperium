using Imperium;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapObjectCombatter))]
public class FOWAgent : MonoBehaviour
{
    public List<GameObject> visibleObjects = new List<GameObject>();
    private MapObjectCombatter mapObjectCombatter;

    // Update is called once per frame
    private void Update()
    {
        float fogRadiusSqr = mapObjectCombatter.combatStats.fieldOfViewDistance * mapObjectCombatter.combatStats.fieldOfViewDistance;

        MapObject[] mapObjects = GameObject.FindObjectsOfType<MapObject>();
        List<GameObject> visibleNow = new List<GameObject>();
        for (int i = 0; i < mapObjects.Length; i++)
        {
            float dist = (transform.position - mapObjects[i].gameObject.transform.position).sqrMagnitude;

            if (dist < fogRadiusSqr)
            {
                visibleNow.Add(mapObjects[i].gameObject);
            }
        }

        visibleObjects = visibleNow;
    }

    private void Start()
    {
        mapObjectCombatter = GetComponent<MapObjectCombatter>();
    }
}