using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrebuiltPlayerAdder : MonoBehaviour {
    [SerializeField]
    private int addToPlayer;
    private PlayerDatabase playerDatabase;

    void Start ()
    {
        playerDatabase = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerDatabase>();
        playerDatabase.AddToPlayer(this.gameObject, addToPlayer);
    }
	
	
	
}
