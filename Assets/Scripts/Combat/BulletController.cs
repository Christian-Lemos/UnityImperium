using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Combat.Turret;

public class BulletController : MonoBehaviour {

    private Bullet bullet;
    private bool initialized = false;

    public void Initiate(Bullet bullet)
    {
        this.bullet = bullet;
        this.gameObject.SetActive(true);
        initialized = true;
        Destroy(this.gameObject, 10f);
    }

	// Update is called once per frame
	void FixedUpdate () {
		if(initialized)
        {
            transform.position += transform.forward * bullet.Speed * Time.fixedDeltaTime;
        }
	}
}
