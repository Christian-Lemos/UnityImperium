using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;
using Imperium.Combat.Turret;
public class TurretController : MonoBehaviour {

    private new Transform transform;

    [SerializeField]
    private TurretType turretType;
    private Turret turret;

    private IEnumerator fireCoroutine;
    private AudioSource audioSource;
    private bool isFiring = false;
	void Start () {
        transform = this.gameObject.GetComponent<Transform>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        this.turret = TurretFactory.getInstance().CreateTurret(turretType);

	}
	
	public void Fire(GameObject target)
    {
        if(!isFiring)
        {
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
            }

            fireCoroutine = FireSequence(target);
            StartCoroutine(fireCoroutine);
        }
       
    }

    private IEnumerator FireSequence(GameObject target)
    {
        while(true)
        {
            if(target == null)
            {
                isFiring = false;
                break;
            }
            else if (Vector3.Distance(this.transform.position, target.transform.position) <= turret.Range)
            {
                isFiring = true;
                Quaternion desRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
                GameObject bullet = Instantiate(this.turret.Bullet.Prefab, this.transform.position, desRotation);

                bullet.GetComponent<BulletController>().Initiate(this.turret.Bullet);
                audioSource.Play();
            }
            yield return new WaitForSeconds(this.turret.FireRate);
        }
    }
}
