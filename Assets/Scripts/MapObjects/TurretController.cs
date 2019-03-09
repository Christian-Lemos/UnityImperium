using Imperium.MapObjects;
using System.Collections;
using UnityEngine;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;

[RequireComponent(typeof(MapObject))]
[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class TurretController : MonoBehaviour, ISerializable<TurretControllerPersistance>
{
    private GameObject @object;
    private AudioSource audioSource;
    private IEnumerator fireCoroutine;
    private GameObject firePriority;
    private bool isFiring = false;

    [SerializeField]
    private TurretType turretType;

    public Turret turret;
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
        else if (firePriority != null && Vector3.Distance(transform.position, firePriority.transform.position) <= turret.range)
        {
            isFiring = true;
            Quaternion desRotation = Quaternion.LookRotation(firePriority.transform.position - transform.position, Vector3.up);
            GameObject bullet = Instantiate(turret.bullet.prefab, transform.position, desRotation);

            bullet.GetComponent<BulletController>().Initiate(@object, turret.bullet);
            audioSource.Play();
        }
        else if (Vector3.Distance(transform.position, target.transform.position) <= turret.range)
        {
            isFiring = true;
            Quaternion desRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
            GameObject bullet = Instantiate(turret.bullet.prefab, transform.position, desRotation);

            bullet.GetComponent<BulletController>().Initiate(@object, turret.bullet);
            audioSource.Play();
        }
        yield return new WaitForSeconds(turret.fireRate);
        isFiring = false;
        StopCoroutine(fireCoroutine);
    }

    private void OnDrawGizmoS()
    {
        try
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, turret.range);
        }
        catch
        {
        }
    }

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        turret = TurretFactory.getInstance().CreateTurret(turretType);
        @object = transform.parent.gameObject;
    }

    public TurretControllerPersistance Serialize()
    {
        long firePriorityId = firePriority != null ? firePriority.GetComponent<MapObject>().id : -1;

        return new TurretControllerPersistance(firePriorityId, isFiring, GetComponent<MapObject>().Serialize(), turret, turretType);
    }

    public void SetObject(TurretControllerPersistance serializedObject)
    {
        throw new System.NotImplementedException();
    }
}