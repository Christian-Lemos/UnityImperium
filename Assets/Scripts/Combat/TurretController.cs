using Imperium.Combat.Turret;
using Imperium.Enum;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class TurretController : MonoBehaviour
{
    private new Transform transform;

    [SerializeField]
    private TurretType turretType;

    public Turret Turret { get; private set; }

    private IEnumerator fireCoroutine;
    private AudioSource audioSource;
    private bool isFiring = false;
    private GameObject firePriority;

    private GameObject @object; // Station or Ship

    private void Start()
    {
        transform = this.gameObject.GetComponent<Transform>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        this.Turret = TurretFactory.getInstance().CreateTurret(turretType);
        this.@object = transform.parent.gameObject;
    }

    private void OnDrawGizmoS()
    {
        try
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, this.Turret.Range);
        }
        catch
        {
        }
    }

    public void Fire(GameObject target)
    {
        if (!isFiring)
        {
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
            }

            fireCoroutine = FireSequence(target);
            StartCoroutine(fireCoroutine);
        }
    }

    public void SetFirePriority(GameObject target)
    {
        this.firePriority = target;
    }

    private IEnumerator FireSequence(GameObject target)
    {
        if (target == null)
        {
            isFiring = false;
        }
        else if (this.firePriority != null && Vector3.Distance(this.transform.position, firePriority.transform.position) <= Turret.Range)
        {
            isFiring = true;
            Quaternion desRotation = Quaternion.LookRotation(firePriority.transform.position - transform.position, Vector3.up);
            GameObject bullet = Instantiate(this.Turret.Bullet.Prefab, this.transform.position, desRotation);

            bullet.GetComponent<BulletController>().Initiate(this.@object, this.Turret.Bullet);
            audioSource.Play();
        }
        else if (Vector3.Distance(this.transform.position, target.transform.position) <= Turret.Range)
        {
            isFiring = true;
            Quaternion desRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
            GameObject bullet = Instantiate(this.Turret.Bullet.Prefab, this.transform.position, desRotation);

            bullet.GetComponent<BulletController>().Initiate(this.@object, this.Turret.Bullet);
            audioSource.Play();
        }
        yield return new WaitForSeconds(this.Turret.FireRate);
        isFiring = false;
        StopCoroutine(fireCoroutine);
    }
}