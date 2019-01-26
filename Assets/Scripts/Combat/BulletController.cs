using Imperium.Combat.Turret;
using Imperium.Enum;
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
            transform.position += transform.forward * bullet.Speed * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == (int)ObjectLayers.Ship || other.gameObject.layer == (int)ObjectLayers.Station) && !other.gameObject.Equals(source))
        {
            ObjectController objectController = other.gameObject.GetComponent<ObjectController>();
            objectController.TakeDamage(bullet.Damage);
            Destroy(gameObject);
        }
    }
}