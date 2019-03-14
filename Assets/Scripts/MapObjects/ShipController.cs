using Imperium;
using Imperium.MapObjects;
using Imperium.Navigation;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapObject))]
public class ShipController : MonoBehaviour, ISerializable<ShipControllerPersistance>
{
    public FleetCommandQueue fleetCommandQueue = new FleetCommandQueue();
    public Ship ship;
    public ShipType shipType;
    public StationConstructor stationConstructor;
    private MapObjectCombatter mapObjectCombatter;

    public void AttackTarget(GameObject target, bool resetCommands, bool loopCommands)
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

    public void BuildStation(GameObject station, bool resetCommands, bool loopCommands)
    {
        FleetCommand fleetCommand = new BuildCommand(gameObject.GetComponent<MapObject>(), station.GetComponent<MapObject>());
        AddCommand(resetCommands, fleetCommand);
        fleetCommandQueue.loopFleetCommands = loopCommands;
    }

    public void FireTurrets(GameObject target)
    {
        TurretController[] turrets = gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turret in turrets)
        {
            turret.Fire(target);
        }
    }

    public void MineAsteroid(GameObject asteroid, bool resetCommands)
    {
        FleetCommand fleetCommand = new MineCommand(gameObject.GetComponent<MapObject>(), asteroid.GetComponent<MapObject>());
        AddCommand(resetCommands, fleetCommand);
        fleetCommandQueue.loopFleetCommands = false;
    }

    public void MoveControl(Vector3 destination)
    {
        Quaternion desRotation = Quaternion.LookRotation(destination - transform.position, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desRotation, ship.angularSpeed * Time.deltaTime);
        transform.position += transform.forward * ship.speed * Time.deltaTime;
    }

    public void MoveToPosition(Vector3 destination, float destinationOffset, bool resetCommands, bool loopCommands)
    {
        FleetCommand fleetCommand = new MoveCommand(gameObject.GetComponent<MapObject>(), destination, destinationOffset);
        AddCommand(resetCommands, fleetCommand);
        fleetCommandQueue.loopFleetCommands = loopCommands;
    }

    public ShipControllerPersistance Serialize()
    {
        List<TurretControllerPersistance> turretControllerPersistances = new List<TurretControllerPersistance>();

        TurretController[] turretControllers = GetComponentsInChildren<TurretController>();

        foreach (TurretController turretController in turretControllers)
        {
            turretControllerPersistances.Add(turretController.Serialize());
        }

        ShipControllerPersistance shipControllerPersistance = new ShipControllerPersistance(ship, shipType, GetComponent<MapObject>().Serialize(), fleetCommandQueue.Serialize(), turretControllerPersistances);

        MineController mineController = GetComponent<MineController>();
        ResourceStorageController resourceStorageController = GetComponent<ResourceStorageController>();

        shipControllerPersistance.mineControllerPersistance = mineController != null ? mineController.Serialize() : null;
        shipControllerPersistance.resourceStoragePersistance = resourceStorageController != null ? resourceStorageController.Serialize() : null;

        return shipControllerPersistance;
    }

    public void SetIdle()
    {
        fleetCommandQueue.ResetCommands();
        fleetCommandQueue.loopFleetCommands = false;
    }

    public ISerializable<ShipControllerPersistance> SetObject(ShipControllerPersistance serializedObject)
    {
        throw new System.NotImplementedException();
    }

    private void AddCommand(bool resetCommands, FleetCommand fleetCommand)
    {
        Debug.Log("Added: " + fleetCommand);
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

    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(gameObject.transform.position, ship.combatStats.FieldOfViewDistance);
        }
        catch
        {
        }
    }

    private void Start()
    {
        ship = ShipFactory.getInstance().CreateShip(shipType);

        mapObjectCombatter = GetComponent<MapObjectCombatter>();
        mapObjectCombatter.combatStats = ship.combatStats;

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