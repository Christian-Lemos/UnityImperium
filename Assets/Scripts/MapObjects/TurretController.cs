using Imperium.MapObjects;
using Imperium.Misc;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using System.Collections;
using UnityEngine;

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
    private Timer fireTimer = null;

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

        return new TurretControllerPersistance(targetId, firePriorityId, isReloading, GetTurretIndex(), fireTimer, turret, turretType);
    }

    private int GetTurretIndex()
    {
        for(int i = 0; i < transform.parent.childCount; i++)
        {
            if(transform.parent.GetChild(i).gameObject.Equals(this.gameObject))
            {
                return i;
            }
        }
        return -1;
    }

    public void SetFirePriority(GameObject target)
    {
        firePriority = target;
    }

    public ISerializable<TurretControllerPersistance> SetObject(TurretControllerPersistance serializedObject)
    {
        this.target = serializedObject.targetID != -1 ? MapObject.FindByID(serializedObject.targetID).gameObject : null; 
        this.firePriority = serializedObject.firePriorityID != -1 ? MapObject.FindByID(serializedObject.firePriorityID).gameObject : null;
        this.isReloading = serializedObject.isReloading;
        this.fireTimer = serializedObject.timer;
        this.fireTimer.action = ReloadControl;
        this.turret = serializedObject.turret;
        this.turretType = serializedObject.turretType;
        return this;

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
        
        if(fireTimer == null || fireTimer.duration == 0)
        {
            fireTimer = new Timer(turret.reloadTime, false, ReloadControl);
        }
        
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