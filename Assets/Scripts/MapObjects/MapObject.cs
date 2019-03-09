using UnityEngine;
using Imperium.MapObjects;
using System.Collections;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;

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

    public MapObjectPersitance Serialize()
    {
        return new MapObjectPersitance(id, this.transform.position, this.transform.localScale, this.transform.rotation);
    }

    public void SetObject(MapObjectPersitance serializedObject)
    {
        throw new System.NotImplementedException();
    }
}
