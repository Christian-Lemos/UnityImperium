using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium;
using Imperium.Enum;
using Imperium.Navigation;


public class ShipController : ObjectController {


    public ShipType type;

    public Ship Ship { get; private set; }
    private ShipMovement shipMovement;

    public StationConstructor stationConstructor;

    public List<FleetCommand> fleetCommands = new List<FleetCommand>();

    private void Start()
    {
        this.Ship = ShipFactory.getInstance().CreateShip(type);
        this.stats = this.Ship.ShipStats;

        this.shipMovement = new ShipMovement(this.gameObject.transform, 2f, 50f);

        lowestTurretRange = base.GetLowestTurretRange();

        stationConstructor = GetComponent<StationConstructor>();

        StartCoroutine(ShieldRegeneration());
    }



    private void Update()
    {
        if(fleetCommands.Count > 0)
        {
            FleetCommand fleetCommand = this.fleetCommands[0];

            if (fleetCommand == null)
            {
                SetIdle();
            }
            else if (!fleetCommand.IsFinished())
            {
                
                fleetCommand.ExecuteCommand();
            }
            else
            {
                this.fleetCommands.RemoveAt(0);
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(gameObject.transform.position, this.Ship.ShipStats.FieldOfViewDistance);
        }
        catch
        {

        }

    }
   

    public void AttackTarget(GameObject target, bool resetCommands)
    {
        if(!target.Equals(this.gameObject))
        {
            FleetCommand fleetCommand = new AttackCommand(this.gameObject, target, this.shipMovement);
            
            AddCommand(resetCommands, fleetCommand);

            TurretController[] turrets = this.gameObject.GetComponentsInChildren<TurretController>(false);
            foreach (TurretController turret in turrets)
            {
                turret.SetFirePriority(target);
            }
        }
    }

    
    public void MoveToPosition(Vector3 destination, float destinationOffset, bool resetCommands)
    {
        FleetCommand fleetCommand = new MoveCommand(this.gameObject, destination, destinationOffset, this.shipMovement);
        AddCommand(resetCommands, fleetCommand);
    }


    public void BuildStation(GameObject station, bool resetCommands)
    {
        FleetCommand fleetCommand = new BuildCommand(this.gameObject, station, this.shipMovement);
        AddCommand(resetCommands, fleetCommand);
    }

    
    public void FireTurrets(GameObject target)
    {
        TurretController[] turrets = this.gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turret in turrets)
        {
            turret.Fire(target);
        }
    }

    private void AddCommand(bool resetCommands, FleetCommand fleetCommand)
    {
        
        if (resetCommands)
        {
            this.fleetCommands.Clear();
            this.fleetCommands.Add(fleetCommand);
        }
        else
        {
            
            this.fleetCommands.Add(fleetCommand);
        }
    }

    public void SetIdle()
    {
        this.fleetCommands.Clear();
    }
    /*private void OnDestroy()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }*/


}