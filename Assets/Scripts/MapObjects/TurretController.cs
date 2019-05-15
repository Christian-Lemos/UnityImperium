using Imperium;
using Imperium.Combat;
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

    private static System.Random random = new System.Random();
    private GameObject @object;

    [SerializeField]
    private FireStage _fireStage = FireStage.READY;

    private AudioSource audioSource;

    private IEnumerator fireCoroutine;

    [SerializeField]
    private GameObject firePriority = null;

    private FireStage fireStage;

    [SerializeField]
    private Timer reloadTimer = null;

    [SerializeField]
    private int salvoCount = 0;

    [SerializeField]
    private Timer salvoTimer = null;

    [SerializeField]
    private GameObject target = null;

    [SerializeField]
    private TurretType turretType;

    private FireStage FireStage
    {
        get
        {
            return _fireStage;
        }
        set
        {
            switch (value)
            {
                case FireStage.FIRING_SALVO:
                    salvoCount = 1;
                    salvoTimer.timerSet = true;
                    reloadTimer.timerSet = false;
                    break;

                case FireStage.RELOADING:
                    salvoTimer.timerSet = false;
                    reloadTimer.timerSet = true;
                    break;

                case FireStage.READY:
                    reloadTimer.ResetTimer();
                    salvoTimer.ResetTimer();
                    salvoCount = 0;
                    salvoTimer.timerSet = false;
                    reloadTimer.timerSet = false;
                    break;
            }
            _fireStage = value;
        }
    }

    public void Fire(GameObject target)
    {
        this.target = target;
    }

    public double GetRandomNumber(System.Random random, double minimum, double maximum)
    {
        return random.NextDouble() * (maximum - minimum) + minimum;
    }

    public bool IsInRange(GameObject target)
    {
        Player player = PlayerDatabase.Instance.GetObjectPlayer(transform.parent.gameObject);

        if (player.PlayerType == PlayerType.AI)
        {
            if (!StrategicAI.playerStrategicAI[player].scoutData.visibleObjetcs.Contains(target))
            {
                return false;
            }
        }
        else
        {
            if (!MapObjecsRenderingController.Instance.visibleObjects.Contains(target))
            {
                return false;
            }
        }

        float magnitude = (transform.parent.transform.position - target.transform.position).sqrMagnitude;

        return magnitude <= turret.range * turret.range;
    }

    public TurretControllerPersistance Serialize()
    {
        long targetId = target != null ? target.GetComponent<MapObject>().id : -1;
        long firePriorityId = firePriority != null ? firePriority.GetComponent<MapObject>().id : -1;

        return new TurretControllerPersistance(targetId, firePriorityId, _fireStage, GetTurretIndex(), reloadTimer, turret, turretType, salvoCount, salvoTimer);
    }

    public void SetFirePriority(GameObject target)
    {
        firePriority = target;
    }

    public ISerializable<TurretControllerPersistance> SetObject(TurretControllerPersistance serializedObject)
    {
        this.target = serializedObject.targetID != -1 ? MapObject.FindByID(serializedObject.targetID).gameObject : null;
        this.firePriority = serializedObject.firePriorityID != -1 ? MapObject.FindByID(serializedObject.firePriorityID).gameObject : null;
        //this.isReloading = serializedObject.isReloading;
        this.FireStage = serializedObject.fireStage;
        this.reloadTimer = serializedObject.reloadTimer;
        //this.reloadTimer.action = this.ReloadControl;
        this.salvoCount = serializedObject.salvoCount;
        this.salvoTimer = serializedObject.salvoTimer;
        //this.salvoTimer.action = this.ShotSalvo;
        this.turret = serializedObject.turret;
        this.turretType = serializedObject.turretType;
        return this;
    }

    private void FireBullet(GameObject target)
    {
        Quaternion desRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);

        GameObject bullet = Spawner.Instance.SpawnBullet(turret.bullet.prefab, transform.position, desRotation);

        bullet.transform.rotation = Quaternion.LookRotation((SetBulletDirection(target.transform.position) - bullet.transform.position));

        turret.salvoReloadTime = 0.0000001f;

        bullet.GetComponent<BulletController>().Initiate(@object, turret.bullet);
        audioSource.Play();
        //salvoCount++;
        if (this.FireStage == FireStage.READY)
        {
            if (turret.shotsPerSalvo == 1)
            {
                this.FireStage = FireStage.RELOADING;
            }
            else
            {
                FireStage = FireStage.FIRING_SALVO;
            }
        }
        else
        {
            salvoCount++;
        }

        //isReloading = true;
        //salvoTimer.timerSet = true;
    }

    private int GetTurretIndex()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).gameObject.Equals(this.gameObject))
            {
                return i;
            }
        }
        return -1;
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

    private Vector3 SetBulletDirection(Vector3 targetPosition)
    {
        float maximumOffset = 3;

        float offset = (maximumOffset / 100) * turret.accuracy;

        float xOffset = (float)GetRandomNumber(random, -offset, offset);
        float yOffset = (float)GetRandomNumber(random, -offset, offset);
        float zOffset = (float)GetRandomNumber(random, -offset, offset);

        Vector3 position = new Vector3(targetPosition.x + xOffset, targetPosition.y + yOffset, targetPosition.z + zOffset);

        Debug.Log(xOffset + " " + yOffset + " " + zOffset);

        return position;
    }

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        turret = TurretFactory.getInstance().CreateTurret(turretType);

        if (reloadTimer == null || reloadTimer.duration == 0)
        {
            reloadTimer = new Timer(turret.salvoReloadTime, false);
        }
        if (salvoTimer == null || salvoTimer.duration == 0)
        {
            salvoTimer = new Timer(turret.shotReloadTime, false);
        }

        @object = transform.parent.gameObject;
    }

    private void Update()
    {
        if (this.FireStage == FireStage.READY)
        {
            if ((firePriority != null && IsInRange(firePriority)) || (target != null && IsInRange(target)))
            {
                FireBullet(firePriority != null ? firePriority : target);
            }
        }
        else if (this.FireStage == FireStage.FIRING_SALVO)
        {
            salvoTimer.Execute();

            if (salvoTimer.IsFinished)
            {
                if (firePriority != null || target != null)
                {
                    FireBullet(firePriority != null ? firePriority : target);
                }

                if (salvoCount < turret.shotsPerSalvo)
                {
                    salvoTimer.ResetTimer();
                }
                else
                {
                    this.FireStage = FireStage.RELOADING;
                }
            }
        }
        else if (this.FireStage == FireStage.RELOADING)
        {
            reloadTimer.Execute();
            if (reloadTimer.IsFinished)
            {
                FireStage = FireStage.READY;
            }
        }
    }
}