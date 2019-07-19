using UnityEngine;
using System.Collections;
using Imperium;

public abstract class ResearchBehaviour : MonoBehaviour
{
    public Player player;
   
    protected abstract void Initiate();

    private void Start()
    {
        Initiate();
    }
}
