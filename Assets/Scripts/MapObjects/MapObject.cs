using Imperium.MapObjects;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using UnityEngine;

public class MapObject : MonoBehaviour, ISerializable<MapObjectPersitance>
{
    public long id;
    public MapObjectType mapObjectType;

    private void OnDestroy()
    {
        try
        {
            int thisPlayer = PlayerDatabase.Instance.GetObjectPlayer(gameObject);
            PlayerDatabase.Instance.RemoveFromPlayer(gameObject, thisPlayer);
        }
        catch
        {
        }
    }

    public static MapObject FindByID(long id)
    {
        MapObject[] mapObjects = GameObject.FindObjectsOfType<MapObject>();

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
        return new MapObjectPersitance(id, transform.position, transform.localScale, transform.rotation);
    }

    public ISerializable<MapObjectPersitance> SetObject(MapObjectPersitance serializedObject)
    {
        throw new System.NotImplementedException();
    }
}