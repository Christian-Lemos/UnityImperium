using Imperium.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ShipMovementController : MonoBehaviour
{
    private ShipMovement shipMovement;
    private new Transform transform;

    private Vector3 moveDestination;
    private float moveOffset = 0f;
  

    private bool isMoving = false;

    private void Start()
    {
        this.transform = this.gameObject.GetComponent<Transform>();
        this.shipMovement = new ShipMovement(this.transform, 2f, 50f);
    }



    private void Update()
    {
        if(this.isMoving == true)
        {
            float distance = Vector3.Distance(transform.position, this.moveDestination);
            if(distance > moveOffset)
            {
                shipMovement.MoveToPosition(this.moveDestination);
            }
            else
            {
                this.isMoving = false;
            }
        }
    }

    public void MoveToPosition(Vector3 destination, float destinationOffset)
    {
        this.moveDestination = destination;
        this.moveOffset = destinationOffset;
        this.isMoving = true;
    }

}

