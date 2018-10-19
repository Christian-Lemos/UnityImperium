using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Movement;

[RequireComponent(typeof(ShipMovementController))]
public class ShipMovementTester : MonoBehaviour {

    public Vector3 destination;
    public float offset;

    private ShipMovementController controller;
    private void Start()
    {
        controller = this.GetComponent<ShipMovementController>();
        controller.MoveToPosition(destination, offset);
    }

    private void Update()
    {
       
    }

}
