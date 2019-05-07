using Imperium;
using Imperium.Combat;
using Imperium.MapObjects;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using Imperium.Rendering;
using UnityEngine;

[RequireComponent(typeof(MapObject))]
public class BulletController : MonoBehaviour, ISerializable<BulletControllerPersistance>, INonExplorable
{
    private Bullet bullet;
    private bool initialized = false;
    private GameObject source;

    public void Initiate(GameObject source, Bullet bullet)
    {
        this.bullet = bullet;
        this.source = source;
        gameObject.SetActive(true);
        initialized = true;
        Destroy(gameObject, 10f);
    }

    public BulletControllerPersistance Serialize()
    {
        return new BulletControllerPersistance(bullet, initialized, GetComponent<MapObject>().Serialize(), source.GetComponent<MapObject>().id);
    }

    public ISerializable<BulletControllerPersistance> SetObject(BulletControllerPersistance serializedObject)
    {
        this.bullet = serializedObject.bullet;
        this.initialized = serializedObject.initialized;
        this.source = MapObject.FindByID(serializedObject.sourceID).gameObject;
        return this;
    }

    private void FixedUpdate()
    {
        if (initialized)
        {
            transform.position += transform.forward * bullet.speed * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MapObject>() != null /*&& (other.gameObject.layer == (int)ObjectLayers.Ship || other.gameObject.layer == (int)ObjectLayers.Station) */ && !other.gameObject.Equals(source) && other.gameObject.GetComponent<IHittable>() != null)
        {
            if(other.gameObject.GetComponent<ShipSquadronController>() == null)
            {
                other.gameObject.GetComponent<IHittable>().TakeHit(bullet);
                Destroy(gameObject);
            }
            
            /*MapObjectCombatter mapObjectCombatter = other.gameObject.GetComponent<MapObjectCombatter>();
            mapObjectCombatter.TakeDamage(bullet.damage);*/
            
        }
    }
}