using Imperium.MapObjects;
using UnityEngine;

public class DummyStation : MonoBehaviour
{
    public float constructionProgress;
    public float timeWhenCreated;
    public Station station;
    public StationType stationType;

    public bool Constructed
    {
        get
        {
            return constructionProgress >= 100;
        }
    }

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
    }
}