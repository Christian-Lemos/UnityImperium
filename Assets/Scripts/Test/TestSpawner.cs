using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;
public class TestSpawner : MonoBehaviour {

	
	void Start () {
        GetComponent<Spawner>().SpawnShip(ShipType.Test, 0, new Vector3(0, 0, 0), Quaternion.identity);
	}
	
	
	void Update () {
		
	}
}
