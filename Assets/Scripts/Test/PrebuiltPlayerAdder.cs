using UnityEngine;

public class PrebuiltPlayerAdder : MonoBehaviour
{
    public int addToPlayer;

    private PlayerDatabase playerDatabase;

    private void Start()
    {
        playerDatabase = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerDatabase>();
        playerDatabase.AddToPlayer(gameObject, addToPlayer);
    }
}