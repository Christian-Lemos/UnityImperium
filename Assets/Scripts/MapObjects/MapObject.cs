using Imperium;
using Imperium.MapObjects;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MapObject : MonoBehaviour, ISerializable<MapObjectPersitance>
{
    public long id;
    public MapObjectType mapObjectType;

    private static HashSet<MapObject> mapObjects;
        

    public MapObject()
    {
        if(mapObjects == null)
        {
            mapObjects = new HashSet<MapObject>();
        }
        mapObjects.Add(this);
    }

    private void OnDestroy()
    {
        try
        {
            Player thisPlayer = PlayerDatabase.Instance.GetObjectPlayer(gameObject);
            PlayerDatabase.Instance.RemoveFromPlayer(gameObject, thisPlayer);
           
        }
        catch
        {
        }
        finally
        {
             mapObjects.Remove(this);
        }
    }

    public static HashSet<MapObject> GetMapObjects()
    {
        return mapObjects;
    }

    public static MapObject FindByID(long id)
    {
        MapObject[] mapObjects = Resources.FindObjectsOfTypeAll<MapObject>();

        foreach (MapObject mapObject in mapObjects)
        {
            if (mapObject.id == id)
            {
                return mapObject;
            }
        }
        return null;
    }

    public static long GetID(MonoBehaviour monoBehaviour)
    {
        return GetID(monoBehaviour.gameObject);
    }

    public static long GetID(GameObject gameObject)
    {
        return gameObject.GetComponent<MapObject>().id;
    }

    public MapObjectPersitance Serialize()
    {
        return new MapObjectPersitance(id, transform.localPosition, transform.localScale, transform.localRotation);
    }

    public ISerializable<MapObjectPersitance> SetObject(MapObjectPersitance serializedObject)
    {
        this.id = serializedObject.id;
        this.transform.localPosition = serializedObject.localPosition;
        this.transform.rotation = serializedObject.localRotation;
        this.transform.localScale = serializedObject.localScale;
        return this;
    }
}