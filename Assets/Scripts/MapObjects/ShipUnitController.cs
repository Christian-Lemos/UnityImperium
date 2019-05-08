using UnityEngine;
using System.Collections;
using Imperium;
using Imperium.Combat;
using Assets.Lib.Navigation;
using Imperium.Rendering;
using Assets.Lib;
using Imperium.MapObjects;
using System;
using Imperium.Navigation;

[RequireComponent(typeof(TurretManager))]
public class ShipUnitController : MonoBehaviour, ISelectable, IHittable, INavigationAgent, INonExplorable, ICombatable
{

    public ShipType shipType;

    private TurretManager turretManager;

    [NonSerialized]
    public Ship ship;


    public CombatStats CombatStats => ship.combatStats;


    private FleetCommandQueue m_fleetCommandQueue = new FleetCommandQueue();
    public FleetCommandQueue FleetCommandQueue => m_fleetCommandQueue;

    public GameObject Select()
    {
        GameObject group = ShipGroup.GetGroup(this.gameObject);
        if(group == null)
        {
            return this.gameObject;
        }
        else
        {
            return group;
        }
    }


    public void TakeHit(Bullet bullet)
    {
        int damage = bullet.damage;
        int shields = CombatStats.Shields;

        if (shields <= damage)
        {
            int hpDamage = shields - damage;
            CombatStats.Shields = 0;
            CombatStats.HP -= -hpDamage;
            if (CombatStats.HP <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            CombatStats.Shields -= damage;
        }
    }
    public void AddCommand(bool resetCommands, FleetCommand fleetCommand)
    {
        if (resetCommands)
        {
            m_fleetCommandQueue.ResetCommands();
            m_fleetCommandQueue.fleetCommands.Add(fleetCommand);
        }
        else
        {
            m_fleetCommandQueue.fleetCommands.Add(fleetCommand);
        }

        if (m_fleetCommandQueue.CurrentFleetCommand == null)
        {
            m_fleetCommandQueue.CurrentFleetCommand = fleetCommand;
        }
    }

    public void AttackTarget(GameObject target, bool resetCommands, bool loopCommands)
    {
        if (!target.Equals(gameObject))
        {
            FleetCommand fleetCommand = new AttackCommand(gameObject.GetComponent<MapObject>(), target.GetComponent<MapObject>());

            AddCommand(resetCommands, fleetCommand);

            m_fleetCommandQueue.loopFleetCommands = loopCommands;

            turretManager.SetFirePriority(target);
        }
    }

    public void BuildStation(GameObject station, bool resetCommands, bool loopCommands)
    {
        FleetCommand fleetCommand = new BuildCommand(gameObject.GetComponent<MapObject>(), station.GetComponent<MapObject>());
        AddCommand(resetCommands, fleetCommand);
        m_fleetCommandQueue.loopFleetCommands = loopCommands;
    }

    public void MineAsteroid(GameObject asteroid, bool resetCommands)
    {
        FleetCommand fleetCommand = new MineCommand(gameObject.GetComponent<MapObject>(), asteroid.GetComponent<MapObject>());
        AddCommand(resetCommands, fleetCommand);
        m_fleetCommandQueue.loopFleetCommands = false;
    }

    public void MoveControl(Vector3 destination)
    {
        destination.y = 0;
        Quaternion desRotation = Quaternion.LookRotation(destination - transform.position, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desRotation, ship.angularSpeed * Time.deltaTime);
        transform.position += transform.forward * ship.speed * Time.deltaTime;
    }

    public void MoveToPosition(Vector3 destination, float destinationOffset, bool resetCommands, bool loopCommands)
    {
        FleetCommand fleetCommand = new MoveCommand(gameObject.GetComponent<MapObject>(), destination, destinationOffset);
        AddCommand(resetCommands, fleetCommand);
        m_fleetCommandQueue.loopFleetCommands = loopCommands;
    }

    public void SetIdle()
    {
        m_fleetCommandQueue.ResetCommands();
        m_fleetCommandQueue.loopFleetCommands = false;
    }

    // Use this for initialization
    void Start()
    {
        if(ship == null)
        {
            ship = ShipFactory.getInstance().CreateShip(shipType);
        }

        turretManager = GetComponent<TurretManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_fleetCommandQueue.fleetCommands.Count > 0)
        {
            FleetCommand fleetCommand = m_fleetCommandQueue.CurrentFleetCommand;

            if (!fleetCommand.IsFinished())
            {
                fleetCommand.ExecuteCommand();
            }
            else
            {
                FleetCommand next = m_fleetCommandQueue.SetNextFleetCommand();
                if (next == null)
                {
                    SetIdle();
                }
            }
        }
    }

}
