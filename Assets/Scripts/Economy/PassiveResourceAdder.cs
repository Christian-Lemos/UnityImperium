using Imperium.Enum;
using System.Collections.Generic;
using UnityEngine;

public class PassiveResourceAdder : MonoBehaviour
{
    public Assosiation[] assosiations;

    public Dictionary<ResourceType, int> true_associations = new Dictionary<ResourceType, int>();

    private void Start()
    {
        for (int i = 0; i < assosiations.Length; i++)
        {
            true_associations.Add(assosiations[i].shipType, assosiations[i].ResourcesPerSecound);
        }
    }

    [System.Serializable]
    public struct Assosiation
    {
        public int ResourcesPerSecound;
        public ResourceType shipType;
    }
}