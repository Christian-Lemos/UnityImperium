using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium;
using Imperium.Enum;
using Imperium.Movement;


public class ShipController : MonoBehaviour {

    
    public ShipType type;
    private ShipState shipState = ShipState.Idle;
    public Ship Ship { get; private set; }
    private ShipMovement shipMovement;

    private new Transform transform;

    private GameObject target;
    private Vector3 moveDestination;
    private float moveOffset = 0f;

    private GameObject gameController;
    private PlayerDatabase playerDatabase;

    private float lowestTurretRange;

   // public GameObject explosion;

    private void Start()
    {
        this.Ship = ShipFactory.getInstance().CreateShip(type);
        this.transform = this.gameObject.GetComponent<Transform>();
        this.shipMovement = new ShipMovement(this.transform, 2f, 50f);

        gameController = GameObject.FindGameObjectWithTag("GameController");
        playerDatabase = gameController.GetComponent<PlayerDatabase>();

        lowestTurretRange = this.Ship.shipStats.FieldOfViewDistance;

        TurretController[] turretControllers = this.gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turretController in turretControllers)
        {
            if (turretController.Turret.Range > lowestTurretRange)
            {
                lowestTurretRange = turretController.Turret.Range;
            }
        }
        StartCoroutine(ShieldRegeneration());
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
        try
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(gameObject.transform.position, this.Ship.shipStats.FieldOfViewDistance);
        }
        catch
        {

        }
       
    }

    private void MovingStateControl()
    {
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
        if(Vector3.Distance(target.transform.position, transform.position) <= this.Ship.shipStats.FieldOfViewDistance)
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

        Collider[] colliders = Physics.OverlapSphere(transform.position, this.Ship.shipStats.FieldOfViewDistance, shipLayer);
        GameObject closestTarget = null;
        float closestDistance = 0f;
        int thisPlayer = playerDatabase.GetObjectPlayer(this.gameObject);
        foreach (Collider collider in colliders)
        {
            if (!playerDatabase.IsFromPlayer(collider.gameObject, thisPlayer) && !collider.gameObject.Equals(this.gameObject))
            {
                float distance = Vector3.Distance(collider.gameObject.transform.position, transform.position);
                if (distance >= closestDistance && distance <= this.Ship.shipStats.FieldOfViewDistance)
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

    public void TakeDamage(int damage)
    {
       
        int shields = this.Ship.shipStats.Shields;
        if (shields <= damage)
        {
            int hpDamage = shields - damage;
            this.Ship.shipStats.Shields = 0;
            this.Ship.shipStats.HP -= -hpDamage;
            if(this.Ship.shipStats.HP <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            this.Ship.shipStats.Shields -= damage;
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

    /*private void OnDestroy()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }*/

    private IEnumerator ShieldRegeneration()
    {
        while(true)
        {
            if (Ship.shipStats.Shields + this.Ship.shipStats.ShieldRegen > Ship.shipStats.MaxShields)
            {
                Ship.shipStats.Shields = Ship.shipStats.MaxShields;
            }
            else
            {
                Ship.shipStats.Shields += this.Ship.shipStats.ShieldRegen;
            }
            yield return new WaitForSeconds(1f);
        }
        
    }

    private void OnDestroy()
    {
        int thisPlayer = PlayerDatabase.INSTANCE.GetObjectPlayer(this.gameObject);
        PlayerDatabase.INSTANCE.RemoveFromPlayer(this.gameObject, thisPlayer);
    }
}