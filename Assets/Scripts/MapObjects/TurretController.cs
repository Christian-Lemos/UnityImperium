using Imperium.MapObjects;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class TurretController : MonoBehaviour
{
    private GameObject @object;
    private AudioSource audioSource;
    private IEnumerator fireCoroutine;
    private GameObject firePriority;
    private bool isFiring = false;
    private new Transform transform;

    [SerializeField]
    private TurretType turretType;

    public Turret Turret { get; private set; }
    // Station or Ship

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
        firePriority = target;
    }

    private IEnumerator FireSequence(GameObject target)
    {
        if (target == null)
        {
            isFiring = false;
        }
        else if (firePriority != null && Vector3.Distance(transform.position, firePriority.transform.position) <= Turret.Range)
        {
            isFiring = true;
            Quaternion desRotation = Quaternion.LookRotation(firePriority.transform.position - transform.position, Vector3.up);
            GameObject bullet = Instantiate(Turret.Bullet.prefab, transform.position, desRotation);

            bullet.GetComponent<BulletController>().Initiate(@object, Turret.Bullet);
            audioSource.Play();
        }
        else if (Vector3.Distance(transform.position, target.transform.position) <= Turret.Range)
        {
            isFiring = true;
            Quaternion desRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
            GameObject bullet = Instantiate(Turret.Bullet.prefab, transform.position, desRotation);

            bullet.GetComponent<BulletController>().Initiate(@object, Turret.Bullet);
            audioSource.Play();
        }
        yield return new WaitForSeconds(Turret.FireRate);
        isFiring = false;
        StopCoroutine(fireCoroutine);
    }

    private void OnDrawGizmoS()
    {
        try
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Turret.Range);
        }
        catch
        {
        }
    }

    private void Start()
    {
        transform = gameObject.GetComponent<Transform>();
        audioSource = gameObject.GetComponent<AudioSource>();
        Turret = TurretFactory.getInstance().CreateTurret(turretType);
        @object = transform.parent.gameObject;
    }
}