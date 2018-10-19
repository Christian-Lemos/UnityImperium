using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium;
using Imperium.Enum;
using Imperium.Movement;
[RequireComponent(typeof(ShipMovementController))]
public class ShipController : MonoBehaviour {
    public ShipType type;
    private Ship ship;

    private new Transform transform;

    private GameObject target;
    private ShipMovement shipMovement;
    private Vector3 moveDestination;
    private float moveOffset = 0f;
    private ShipState shipState = ShipState.Idle;
    private GameObject gameController;
    private PlayerDatabase playerDatabase;
    
    private void Start()
    {
        this.ship = ShipFactory.getInstance().CreateShip(type);
        this.transform = this.gameObject.GetComponent<Transform>();
        this.shipMovement = new ShipMovement(this.transform, 2f, 50f);
        gameController = GameObject.FindGameObjectWithTag("GameController");
        playerDatabase = gameController.GetComponent<PlayerDatabase>();
    }

    private void Update()
    {
        switch(shipState)
        {
            case ShipState.Moving:
                MovingStateControl();
                break;
            case ShipState.Idle:
                IdleStateControl();
                break;
        }
    }

    private void MovingStateControl()
    {
        Vector3 moveDestinationDirection = moveDestination - this.transform.position;
        float distance = moveDestination.sqrMagnitude;

        if (distance > moveOffset)
        {
            shipMovement.MoveToPosition(this.moveDestination);
        }
        else
        {
            shipState = ShipState.Idle;
        }
    }
    private void IdleStateControl()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, ship.stats.FieldOfViewDistance, (int)ObjectLayers.Ship, QueryTriggerInteraction.Ignore);
        GameObject closestTarget = null;
        float closestDistance = 0f;
        int thisPlayer = playerDatabase.getObjectPlayer(this.gameObject);
        foreach(Collider collider in colliders)
        {
            if (!playerDatabase.IsFromPlayer(collider.gameObject, thisPlayer))
            {
                Vector3 targetDirection = collider.gameObject.transform.position - transform.position;
                if (targetDirection.sqrMagnitude > closestDistance)
                {
                    closestTarget = collider.gameObject;
                }
            }
        }
        if(closestTarget != null)
        {
            FireTurrets(closestTarget);
        }
    }



    public void MoveToPosition(Vector3 destination, float destinationOffset)
    {
        this.moveDestination = destination;
        this.moveOffset = destinationOffset;
        shipState = ShipState.Moving;
    }

    public void AttackTarget(GameObject target)
    {
        
    }



    private void FireTurrets(GameObject target)
    {
        TurretController[] turrets = target.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turret in turrets)
        {
            turret.Fire(target);
        }
    }
}