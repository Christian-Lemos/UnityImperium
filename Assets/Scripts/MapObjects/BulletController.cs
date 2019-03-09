using Imperium;
using Imperium.MapObjects;
using UnityEngine;

public class BulletController : MonoBehaviour
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

    private void FixedUpdate()
    {
        if (initialized)
        {
            transform.position += transform.forward * bullet.speed * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == (int)ObjectLayers.Ship || other.gameObject.layer == (int)ObjectLayers.Station) && !other.gameObject.Equals(source))
        {
            MapObjectCombatter mapObjectCombatter = other.gameObject.GetComponent<MapObjectCombatter>();
            mapObjectCombatter.TakeDamage(bullet.damage);
            Destroy(gameObject);
        }
    }
}