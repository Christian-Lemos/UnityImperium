using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium;
using Imperium.Enum;
using Imperium.Movement;


public class ShipController : MonoBehaviour {
    public ShipType type;
    private Ship ship;

    private new Transform transform;

    private GameObject target;
    private ShipMovement shipMovement;
    private Vector3 moveDestination;
    private float moveOffset = 0f;
    [SerializeField]
    private ShipState shipState = ShipState.Idle;
    private GameObject gameController;
    private PlayerDatabase playerDatabase;

    private float lowestTurretRange;
    private void Start()
    {
        this.ship = ShipFactory.getInstance().CreateShip(type);
        this.transform = this.gameObject.GetComponent<Transform>();
        this.shipMovement = new ShipMovement(this.transform, 2f, 50f);
        gameController = GameObject.FindGameObjectWithTag("GameController");
        playerDatabase = gameController.GetComponent<PlayerDatabase>();

        lowestTurretRange = this.ship.stats.FieldOfViewDistance;
        TurretController[] turretControllers = this.gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turretController in turretControllers)
        {
            if(turretController.Turret.Range > lowestTurretRange)
            {
                lowestTurretRange = turretController.Turret.Range;
            }
        }
    }

    private void Update()
    {
        switch(shipState)
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
        }
        FireAtClosestTarget();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, this.ship.stats.FieldOfViewDistance);
    }

    private void MovingStateControl()
    {
        Vector3 moveDestinationDirection = moveDestination - this.gameObject.transform.position;
        float distance = Vector3.Distance(moveDestination, this.gameObject.transform.position);
        if (distance > moveOffset)
        {
            shipMovement.MoveToPosition(this.moveDestination);
        }
        else if(this.shipState == ShipState.Moving)
        {
            shipState = ShipState.Idle;
        }
    }
    private void AttackingStateControl()
    {
        if(Vector3.Distance(target.transform.position, transform.position) <= this.ship.stats.FieldOfViewDistance)
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
    private void FireAtClosestTarget()
    {
        int shipLayer = 1 << (int)ObjectLayers.Ship;

        Collider[] colliders = Physics.OverlapSphere(transform.position, this.ship.stats.FieldOfViewDistance, shipLayer);
        GameObject closestTarget = null;
        float closestDistance = 0f;
        int thisPlayer = playerDatabase.getObjectPlayer(this.gameObject);
        foreach (Collider collider in colliders)
        {
            if (true && !collider.gameObject.Equals(this.gameObject))
            {
                float distance = Vector3.Distance(collider.gameObject.transform.position, transform.position);
                if (distance >= closestDistance && distance <= this.ship.stats.FieldOfViewDistance)
                {
                    closestTarget = collider.gameObject;
                }
            }
        }
        if (closestTarget != null)
        {
            FireTurrets(closestTarget);
        }
    }

    public void MoveToPosition(Vector3 destination, float destinationOffset)
    {
        this.moveDestination = destination;
        this.moveOffset = destinationOffset;
        shipState = ShipState.Moving;

        TurretController[] turrets = this.gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turret in turrets)
        {
            turret.setFirePriority(null);
        }
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



    private void FireTurrets(GameObject target)
    {
        TurretController[] turrets = this.gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turret in turrets)
        {
            turret.Fire(target);
        }
    }
}