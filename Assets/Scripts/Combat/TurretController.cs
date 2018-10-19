using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;
using Imperium.Combat.Turret;
public class TurretController : MonoBehaviour {

    private new Transform transform;

    [SerializeField]
    private TurretType turretType;
    public Turret Turret { get; private set; }

    private IEnumerator fireCoroutine;
    private AudioSource audioSource;
    private bool isFiring = false;
    private GameObject firePriority;

   

    void Start () {
        transform = this.gameObject.GetComponent<Transform>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        this.Turret = TurretFactory.getInstance().CreateTurret(turretType);

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

    public void setFirePriority(GameObject target)
    {
        this.firePriority = target;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, this.Turret.Range);
    }
    private IEnumerator FireSequence(GameObject target)
    {

        if(target == null)
        {
            isFiring = false;
        }
        else if(this.firePriority != null && Vector3.Distance(this.transform.position, firePriority.transform.position) <= Turret.Range)
        {
            isFiring = true;
            Quaternion desRotation = Quaternion.LookRotation(firePriority.transform.position - transform.position, Vector3.up);
            GameObject bullet = Instantiate(this.Turret.Bullet.Prefab, this.transform.position, desRotation);

            bullet.GetComponent<BulletController>().Initiate(this.Turret.Bullet);
            audioSource.Play();
        }
        else if (Vector3.Distance(this.transform.position, target.transform.position) <= Turret.Range)
        {
            isFiring = true;
            Quaternion desRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
            GameObject bullet = Instantiate(this.Turret.Bullet.Prefab, this.transform.position, desRotation);

            bullet.GetComponent<BulletController>().Initiate(this.Turret.Bullet);
            audioSource.Play();
        }
        yield return new WaitForSeconds(this.Turret.FireRate);
        isFiring = false;
    }
}
