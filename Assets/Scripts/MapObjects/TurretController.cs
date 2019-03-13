using Imperium.MapObjects;
using Imperium.Misc;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MapObject))]
[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class TurretController : MonoBehaviour, ISerializable<TurretControllerPersistance>
{
    public Turret turret;

    private GameObject @object;

    private AudioSource audioSource;

    private IEnumerator fireCoroutine;
    [SerializeField]
    private GameObject firePriority;

    [SerializeField]
    private Timer fireTimer;

    private bool isReloading = false;
    
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private TurretType turretType;

    public void Fire(GameObject target)
    {
        this.target = target;
    }

    public TurretControllerPersistance Serialize()
    {
        long targetId = target != null ? target.GetComponent<MapObject>().id : -1;
        long firePriorityId = firePriority != null ? firePriority.GetComponent<MapObject>().id : -1;

        return new TurretControllerPersistance(targetId, firePriorityId, isReloading, GetComponent<MapObject>().Serialize(), this.fireTimer, turret, turretType);
    }

    public void SetFirePriority(GameObject target)
    {
        firePriority = target;
    }

    public ISerializable<TurretControllerPersistance> SetObject(TurretControllerPersistance serializedObject)
    {
        throw new System.NotImplementedException();
    }

    private void FireBullet(GameObject target)
    {
        Quaternion desRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);

        GameObject bullet = Spawner.Instance.SpawnBullet(turret.bullet.prefab, transform.position, desRotation);
        turret.reloadTime = 0.0000001f;
        bullet.GetComponent<BulletController>().Initiate(@object, turret.bullet);
        audioSource.Play();
        isReloading = true;
        fireTimer.timerSet = true;
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

    private void ReloadControl()
    {
        isReloading = false;
        fireTimer.timerSet = false;
        fireTimer.ResetTimer();
    }

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        turret = TurretFactory.getInstance().CreateTurret(turretType);

        fireTimer = new Timer(turret.reloadTime, false, ReloadControl);
        @object = transform.parent.gameObject;
    }

    private void Update()
    {
        if (!isReloading)
        {
            if (firePriority != null && Vector3.Distance(transform.position, firePriority.transform.position) <= turret.range)
            {
                FireBullet(firePriority);
            }
            else if (target != null && Vector3.Distance(transform.position, target.transform.position) <= turret.range)
            {
                FireBullet(target);
            }
            
        }

        if (isReloading)
        {
            fireTimer.Execute();
        }
    }
}