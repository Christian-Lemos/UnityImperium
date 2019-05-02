using Imperium;
using UnityEngine;

public class PrebuiltPlayerAdder : MonoBehaviour
{
    public Player addToPlayer;

    private PlayerDatabase playerDatabase;

    private void Start()
    {
        playerDatabase = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerDatabase>();
        playerDatabase.AddObjectToPlayer(gameObject, addToPlayer);
    }
}