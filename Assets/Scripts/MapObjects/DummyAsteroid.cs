using Imperium.Economy;
using UnityEngine;

public class DummyAsteroid : MonoBehaviour
{
    public float timeWhenCreated;
    public int resourceQuantity;
    public ResourceType resourceType;
    public float LastSeen
    {
        get
        {
            return GameTimeOptions.Instance.currentTime - timeWhenCreated;
        }
    }

    private void Start()
    {
        timeWhenCreated = GameTimeOptions.Instance.currentTime;

        Material material = GetComponentInChildren<Renderer>().material;
        material.color = AsteroidController.asteroidColors[resourceType];
    }
}