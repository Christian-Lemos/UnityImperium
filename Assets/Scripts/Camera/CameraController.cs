using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Movement;
public class CameraController : MonoBehaviour {

    private CameraMovement cameraMovement;
	// Use this for initialization
	void Start () {
        cameraMovement = new CameraMovement((Transform) this.gameObject.GetComponent<Transform>());
	}
	
	// Update is called once per frame
	void Update () {
        float v_axis = Input.GetAxis("Vertical");
        float h_axis = Input.GetAxis("Horizontal");
        cameraMovement.Move(h_axis, v_axis); 
        
	}
}
