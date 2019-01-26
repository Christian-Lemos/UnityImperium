using UnityEngine;

public class PrebuiltPlayerAdder : MonoBehaviour
{
    [SerializeField]
    private int addToPlayer;

    private PlayerDatabase playerDatabase;

    private void Start()
    {
        playerDatabase = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerDatabase>();
        playerDatabase.AddToPlayer(gameObject, addToPlayer);
    }
}