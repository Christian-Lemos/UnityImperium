using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Combat.Turret;
using Imperium.Enum;
public class BulletController : MonoBehaviour {

    private Bullet bullet;
    private bool initialized = false;
    private GameObject source;

    public void Initiate(GameObject source, Bullet bullet)
    {
        this.bullet = bullet;
        this.source = source;
        this.gameObject.SetActive(true);
        initialized = true;
        Destroy(this.gameObject, 10f);
    }


	void FixedUpdate () {
		if(initialized)
        {
            transform.position += transform.forward * bullet.Speed * Time.fixedDeltaTime;
        }
	}
    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.layer == (int)ObjectLayers.Ship || other.gameObject.layer == (int)ObjectLayers.Station) && !other.gameObject.Equals(this.source))
        {
            ObjectController objectController = other.gameObject.GetComponent<ObjectController>();
            objectController.TakeDamage(this.bullet.Damage);
            Destroy(this.gameObject);
        }
    }
}
