﻿using Imperium;
using Imperium.Combat;
using Imperium.MapObjects;
using Imperium.Navigation;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using Imperium.Rendering;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapObject))]
public class ShipSquadronUnitController : ShipController, ISerializable<ShipControllerPersistance>, INonExplorable
{

    public ShipSquadronUnitController shipSquadronUnitController;

    /*


    private void Start()
    {
        shipControllerType = ShipControllerType.SQUADRON_UNIT;
    }


    public override void AttackTarget(GameObject target, bool resetCommands, bool loopCommands)
    {
        shipSquadronUnitController.AttackTarget(target, resetCommands, loopCommands);
    }

    

    public override void BuildStation(GameObject station, bool resetCommands, bool loopCommands)
    {
        shipSquadronUnitController.BuildStation(station, resetCommands, loopCommands);

        foreach (ShipController shipController in SquadronUnits)
        {
            shipController.BuildStation(station, resetCommands, loopCommands);
        }
    }

    public override void FireTurrets(GameObject target)
    {
        shipSquadronUnitController.FireTurrets(target);
        foreach (ShipController shipController in SquadronUnits)
        {
            shipController.FireTurrets(target);
        }
    }


    public override void MineAsteroid(GameObject asteroid, bool resetCommands)
    {
        shipSquadronUnitController.MineAsteroid(asteroid, resetCommands);
        foreach (ShipController shipController in SquadronUnits)
        {
            shipController.MineAsteroid(asteroid, resetCommands);
        }
    }


    public override void MoveControl(Vector3 destination)
    {
        shipSquadronUnitController.MoveControl(destination);
        foreach (ShipController shipController in SquadronUnits)
        {
            //shipController.MoveControl(destination);
        }
    }


    public override void MoveToPosition(Vector3 destination, float destinationOffset, bool resetCommands, bool loopCommands)
    {
        shipSquadronUnitController.MoveToPosition(destination, destinationOffset, resetCommands, loopCommands);
        foreach (ShipController shipController in SquadronUnits)
        {
            shipController.MoveToPosition(this.transform.position, 0, resetCommands, loopCommands);
        }
    }

    public override ShipControllerPersistance Serialize()
    {
        ShipControllerPersistance shipControllerPersistance = base.Serialize();
        List<ShipControllerPersistance> squadronPersistance =new List<ShipControllerPersistance>();
        foreach (ShipController shipController in SquadronUnits)
        {
            squadronPersistance.Add(shipController.Serialize());
        }

        shipControllerPersistance.shipControllerType = ShipControllerType.SQUADRON;
        shipControllerPersistance.squadron = squadronPersistance;
        return shipControllerPersistance;
    }


    public override void SetIdle()
    {
        shipSquadronUnitController.SetIdle();
        foreach (ShipController shipController in SquadronUnits)
        {
            shipController.SetIdle();
        }
    }

    public override ISerializable<ShipControllerPersistance> SetObject(ShipControllerPersistance serializedObject)
    {
        shipSquadronUnitController.SetObject(serializedObject);
        int i = 0;
        foreach (ShipControllerPersistance shipControllerPersistance in serializedObject.squadron)
        {
            SquadronUnits[i].SetObject(shipControllerPersistance);
            i++;
        }

        return this;
    }

    public override void AddCommand(bool resetCommands, FleetCommand fleetCommand)
    {
        shipSquadronUnitController.AddCommand(resetCommands, fleetCommand);
        foreach (ShipController shipController in SquadronUnits)
        {
            shipController.AddCommand(resetCommands, fleetCommand);
        }
    }

    public override void TakeHit(Bullet bullet)
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

        shipSquadronUnitController.TakeHit(bullet);
    }*/
}