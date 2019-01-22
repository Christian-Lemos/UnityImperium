using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium;
using Imperium.Enum;
using Imperium.Movement;


public class ShipController : ObjectController {


    public ShipType type;
    private ShipState shipState = ShipState.Idle;
    public Ship Ship { get; private set; }
    private ShipMovement shipMovement;

    private GameObject target;
    private Vector3 moveDestination;
    private float moveOffset = 0f;


    // public GameObject explosion;

    private void Start()
    {
        this.Ship = ShipFactory.getInstance().CreateShip(type);
        this.stats = this.Ship.ShipStats;

        this.shipMovement = new ShipMovement(this.gameObject.transform, 2f, 50f);

        lowestTurretRange = base.GetLowestTurretRange();



        StartCoroutine(ShieldRegeneration());

        StationConstructor stationConstructor = GetComponent<StationConstructor>();

        /*if(stationConstructor != null)
        {
            stationConstructor.BuildStation(StationType.TestStation);   
        }*/
    }

    private void Update()
    {
        switch (shipState)
        {
            case ShipState.Moving:
                MovingStateControl();
                break;
            case ShipState.Attacking:
                AttackingStateControl();
                break;
            case ShipState.Idle:
                IdleStateControl();
                break;
            case ShipState.Building:
                ConstructionStateControl();
                break;
        }
        FireAtClosestTarget();
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

    private void MovingStateControl()
    {
       
        float distance = Vector3.Distance(this.moveDestination, this.gameObject.transform.position);
        if (distance > moveOffset)
        {
            shipMovement.MoveToPosition(this.moveDestination);
        }
        else if (this.shipState == ShipState.Moving)
        {
            shipState = ShipState.Idle;
        }
    }

    private void ConstructionStateControl()
    {
        this.moveDestination = this.target.transform.position;
        
        float distance = Vector3.Distance(this.moveDestination, this.gameObject.transform.position);
        if (distance > moveOffset)
        {
            shipMovement.MoveToPosition(this.moveDestination);
        }
        else if(target.GetComponent<StationController>().constructed == true)
        {
            this.target = null;
            this.shipState = ShipState.Idle;
            this.GetComponent<StationConstructor>().StopBuilding();
            
        }
        else
        {
            StationConstructor stationConstructor = this.GetComponent<StationConstructor>();
            if (stationConstructor.Building == false)
            {
                stationConstructor.StartBuilding(this.target);
            }
        }
    }

    private void AttackingStateControl()
    {
        if(Vector3.Distance(target.transform.position, transform.position) <= this.Ship.ShipStats.FieldOfViewDistance)
        {
            FireTurrets(target);
        }

        //MoveToPosition(target.transform.position, lowestTurretRange / 2);
        this.moveDestination = target.transform.position;
        this.moveOffset = lowestTurretRange / 2;
        MovingStateControl();
    }
    private void IdleStateControl()
    {
    }

    public void MoveToPosition(Vector3 destination, float destinationOffset)
    {
        this.moveDestination = destination;
        this.moveOffset = destinationOffset;
        shipState = ShipState.Moving;

        /*TurretController[] turrets = this.gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turret in turrets)
        {
            turret.setFirePriority(null);
        }*/
    }

    public void AttackTarget(GameObject target)
    {
        if(!target.Equals(this.gameObject))
        {
            this.target = target;
            this.shipState = ShipState.Attacking;

            TurretController[] turrets = this.gameObject.GetComponentsInChildren<TurretController>(false);
            foreach (TurretController turret in turrets)
            {
                turret.setFirePriority(target);
            }
        }
    }

    public void BuildStation(GameObject station)
    {
        this.moveOffset = 2f;
        this.target = station;
        this.shipState = ShipState.Building;
    }



    private void FireTurrets(GameObject target)
    {
        TurretController[] turrets = this.gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turret in turrets)
        {
            turret.Fire(target);
        }
    }

    /*private void OnDestroy()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }*/

   
}