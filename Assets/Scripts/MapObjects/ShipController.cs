using Imperium;
using Imperium.Combat;
using Imperium.MapObjects;
using Imperium.Navigation;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using Imperium.Rendering;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapObject))]
public class ShipController : MonoBehaviour, ISerializable<ShipControllerPersistance>, INonExplorable, IHittable, ISelectable
{
    public FleetCommandQueue fleetCommandQueue = new FleetCommandQueue();

    public bool initialized = true;
    public ShipControllerType shipControllerType;
    public ShipType shipType;
    public StationConstructor stationConstructor;
    protected Ship _ship;
    protected MapObjectCombatter mapObjectCombatter;

    public Ship Ship
    {
        get
        {
            return _ship;
        }
        set
        {
            _ship = value;
        }
    }

    public virtual void AddCommand(bool resetCommands, FleetCommand fleetCommand)
    {
        //Debug.Log("Added: " + fleetCommand);
        if (resetCommands)
        {
            fleetCommandQueue.ResetCommands();
            fleetCommandQueue.fleetCommands.Add(fleetCommand);
        }
        else
        {
            fleetCommandQueue.fleetCommands.Add(fleetCommand);
        }

        if (fleetCommandQueue.CurrentFleetCommand == null)
        {
            fleetCommandQueue.CurrentFleetCommand = fleetCommand;
        }
    }

    public virtual void AttackTarget(GameObject target, bool resetCommands, bool loopCommands)
    {
        if (!target.Equals(gameObject))
        {
            FleetCommand fleetCommand = new AttackCommand(gameObject.GetComponent<MapObject>(), target.GetComponent<MapObject>());

            AddCommand(resetCommands, fleetCommand);

            fleetCommandQueue.loopFleetCommands = loopCommands;

            TurretController[] turrets = gameObject.GetComponentsInChildren<TurretController>(false);
            foreach (TurretController turret in turrets)
            {
                turret.SetFirePriority(target);
            }
        }
    }

    public virtual void BuildStation(GameObject station, bool resetCommands, bool loopCommands)
    {
        FleetCommand fleetCommand = new BuildCommand(gameObject.GetComponent<MapObject>(), station.GetComponent<MapObject>());
        AddCommand(resetCommands, fleetCommand);
        fleetCommandQueue.loopFleetCommands = loopCommands;
    }

    public virtual void FireTurrets(GameObject target)
    {
        TurretController[] turrets = gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turret in turrets)
        {
            turret.Fire(target);
        }
    }

    public virtual void MineAsteroid(GameObject asteroid, bool resetCommands)
    {
        FleetCommand fleetCommand = new MineCommand(gameObject.GetComponent<MapObject>(), asteroid.GetComponent<MapObject>());
        AddCommand(resetCommands, fleetCommand);
        fleetCommandQueue.loopFleetCommands = false;
    }

    public virtual void MoveControl(Vector3 destination)
    {
        destination.y = 0;
        Quaternion desRotation = Quaternion.LookRotation(destination - transform.position, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desRotation, Ship.angularSpeed * Time.deltaTime);
        transform.position += transform.forward * Ship.speed * Time.deltaTime;
    }

    public virtual void MoveToPosition(Vector3 destination, float destinationOffset, bool resetCommands, bool loopCommands)
    {
        FleetCommand fleetCommand = new MoveCommand(gameObject.GetComponent<MapObject>(), destination, destinationOffset);
        AddCommand(resetCommands, fleetCommand);
        fleetCommandQueue.loopFleetCommands = loopCommands;
    }

    public virtual ShipControllerPersistance Serialize()
    {
        List<TurretControllerPersistance> turretControllerPersistances = new List<TurretControllerPersistance>();

        TurretController[] turretControllers = GetComponentsInChildren<TurretController>();

        foreach (TurretController turretController in turretControllers)
        {
            turretControllerPersistances.Add(turretController.Serialize());
        }

        ShipControllerPersistance shipControllerPersistance = new ShipControllerPersistance(Ship, shipType, GetComponent<MapObject>().Serialize(), fleetCommandQueue.Serialize(), turretControllerPersistances, true, null, shipControllerType);

        MineController mineController = GetComponent<MineController>();
        ResourceStorageController resourceStorageController = GetComponent<ResourceStorageController>();

        shipControllerPersistance.mineControllerPersistance = mineController != null ? mineController.Serialize() : null;
        shipControllerPersistance.resourceStoragePersistance = resourceStorageController != null ? resourceStorageController.Serialize() : null;

        return shipControllerPersistance;
    }

    public virtual void SetIdle()
    {
        fleetCommandQueue.ResetCommands();
        fleetCommandQueue.loopFleetCommands = false;
    }

    public virtual ISerializable<ShipControllerPersistance> SetObject(ShipControllerPersistance serializedObject)
    {
        this.fleetCommandQueue = new FleetCommandQueue();
        this.fleetCommandQueue.SetObject(serializedObject.fleetCommandQueuePersistance);

        MineController mineController = GetComponent<MineController>();
        if (mineController != null)
        {
            mineController.SetObject(serializedObject.mineControllerPersistance);
        }

        ResourceStorageController resourceStorageController = GetComponent<ResourceStorageController>();
        if (resourceStorageController != null)
        {
            resourceStorageController.SetObject(serializedObject.resourceStoragePersistance);
        }

        foreach (TurretControllerPersistance turretControllerPersistance in serializedObject.turretControllerPersistances)
        {
            transform.GetChild(turretControllerPersistance.turretIndex).GetComponent<TurretController>().SetObject(turretControllerPersistance);
        }

        this._ship = serializedObject.ship;
        this.shipType = serializedObject.shipType;

        return this;
    }

    public virtual void TakeHit(Bullet bullet)
    {
        if (shipControllerType != ShipControllerType.SQUADRON_UNIT)
        {
            CombatStats combatStats = Ship.combatStats;
            int damage = bullet.damage;
            int shields = combatStats.Shields;

            if (shields <= damage)
            {
                int hpDamage = shields - damage;
                combatStats.Shields = 0;
                combatStats.HP -= -hpDamage;
                if (combatStats.HP <= 0)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                combatStats.Shields -= damage;
            }
        }
        else 
        {
            transform.parent.gameObject.GetComponent<ShipController>().TakeHit(bullet);
        }
    }

    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(gameObject.transform.position, Ship.combatStats.FieldOfView);
        }
        catch
        {
        }
    }

    public virtual GameObject Select()
    {
        return this.gameObject;
    }

    private void Start()
    {
        if (_ship == null)
        {
            Ship = ShipFactory.getInstance().CreateShip(shipType);
        }

        mapObjectCombatter = GetComponent<MapObjectCombatter>();
        mapObjectCombatter.combatStats = Ship.combatStats;
        StartCoroutine(mapObjectCombatter.ShieldRegeneration());

        stationConstructor = GetComponent<StationConstructor>();
    }

    private void Update()
    {
        if (fleetCommandQueue.fleetCommands.Count > 0)
        {
            FleetCommand fleetCommand = fleetCommandQueue.CurrentFleetCommand;

            if (!fleetCommand.IsFinished())
            {
                fleetCommand.ExecuteCommand();
            }
            else
            {
                FleetCommand next = fleetCommandQueue.SetNextFleetCommand();
                if (next == null)
                {
                    SetIdle();
                }
            }
        }

        mapObjectCombatter.FireAtClosestTarget();
    }
}