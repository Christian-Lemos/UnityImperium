using Assets.Lib;
using Assets.Lib.Navigation;
using Imperium;
using Imperium.Combat;
using Imperium.MapObjects;
using Imperium.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipGroup : MonoBehaviour, ICombatable, ISelectable, INavigationAgent
{
    public bool initialized = false;
    public List<ShipController> members;
    public ShipType membersShipType;
    public Player player;
    public int totalMembers;
    private CombatStats m_combatStats;
    private FleetCommandQueue m_fleetCommandQueue = new FleetCommandQueue();
    private Dictionary<CombatStats, ShipController> m_memberStats = new Dictionary<CombatStats, ShipController>();
    public CombatStats CombatStats => m_combatStats;
    FleetCommandQueue INavigationAgent.FleetCommandQueue => m_fleetCommandQueue;

    private float m_speed;
    private float m_angularSpeed;

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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desRotation, m_angularSpeed * Time.deltaTime);
        transform.position += transform.forward * m_speed * Time.deltaTime;
    }

    public void MoveToPosition(Vector3 destination, float destinationOffset, bool resetCommands, bool loopCommands)
    {
        FleetCommand fleetCommand = new MoveCommand(gameObject.GetComponent<MapObject>(), destination, destinationOffset);
        AddCommand(resetCommands, fleetCommand);
        m_fleetCommandQueue.loopFleetCommands = loopCommands;
    }

    public GameObject Select()
    {
        return this.gameObject;
    }

    private bool AreAllMembersDestoyed()
    {
        foreach (KeyValuePair<CombatStats, ShipController> item in m_memberStats)
        {
            if (item.Key.HP > 0)
            {
                return false;
            }
        }
        return true;
    }

    private CombatStats CreateStats()
    {
        int length = members.Count;
        CombatStats combatStats = new CombatStats(0, 0, 0, 0);
        for (int i = 0; i < length; i++)
        {
            CombatStats memberCombatStats = members[i].Ship.combatStats;

            combatStats.MaxHP += memberCombatStats.MaxHP;
            combatStats.MaxShields += memberCombatStats.MaxShields;
            combatStats.ShieldRegen += memberCombatStats.ShieldRegen;
            combatStats.FieldOfView = memberCombatStats.FieldOfView > combatStats.FieldOfView ? memberCombatStats.FieldOfView : combatStats.FieldOfView;
        }

        for (int i = 0; i < length; i++)
        {
            CombatStats memberCombatStats = members[i].Ship.combatStats;

            combatStats.HP += memberCombatStats.HP;
            combatStats.Shields += memberCombatStats.Shields;
        }

        return combatStats;
    }

    private void SetObservers()
    {
        int length = members.Count;
        for (int i = 0; i < length; i++)
        {
            CombatStats memberCombatStats = members[i].Ship.combatStats;
            m_memberStats.Add(memberCombatStats, members[i]);
            memberCombatStats.AddObserver(StatsUpdate);
        }
    }

    // Use this for initialization
    private void Start()
    {
        player = PlayerDatabase.Instance.GetObjectPlayer(this.gameObject);

        if (!initialized)
        {
            for (int i = 0; i < totalMembers; i++)
            {
                GameObject member = Spawner.Instance.SpawnShip(membersShipType, player, this.transform.position, Quaternion.identity, false);
                members.Add(member.GetComponent<ShipController>());
            }
        }

        totalMembers = members.Count;
        m_combatStats = CreateStats();
        SetObservers();
        SetSpeeds();
    }
    void SetSpeeds()
    {
        float lowestSpeed = 99999999999;
        float lowestAngularSpeed = 999999999999;
        for (int i = 0; i < members.Count; i++)
        {
            if(members[i].Ship.speed < lowestSpeed)
            {
                lowestSpeed = members[i].Ship.speed;
            }
            if (members[i].Ship.angularSpeed < lowestAngularSpeed)
            {
                lowestAngularSpeed = members[i].Ship.angularSpeed;
            }
        }
        m_speed = lowestSpeed;
        m_angularSpeed = lowestAngularSpeed;
    }

    private void StatsUpdate(CombatStats combatStats, int hp, int maxHP, int shields, int maxShields, int shieldRegen, float fieldOfView)
    {
        m_combatStats.MaxHP += maxHP;
        m_combatStats.MaxShields += maxShields;
        m_combatStats.HP += hp;
        m_combatStats.Shields += shields;

        m_combatStats.ShieldRegen += shieldRegen;
        m_combatStats.FieldOfView += fieldOfView;

        if (combatStats.HP > 0)
        {
            ShipController member = m_memberStats[combatStats];

            member.gameObject.SetActive(false);
        }

        if (AreAllMembersDestoyed())
        {
            Destroy(this.gameObject);
        }
    }
}