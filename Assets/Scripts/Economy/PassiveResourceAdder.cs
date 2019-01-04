using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;
public class PassiveResourceAdder : MonoBehaviour {

    [System.Serializable]
    public struct Assosiation
    {
        public ResourceType shipType;
        public int ResourcesPerSecound;
    }
    public Assosiation[] assosiations;

    public Dictionary<ResourceType, int> true_associations = new Dictionary<ResourceType, int>();
    private PlayerDatabase playerDatabase;

    private void Start()
    {
        playerDatabase = GetComponent<PlayerDatabase>();
        for (int i = 0; i < assosiations.Length; i++)
        {
            true_associations.Add(assosiations[i].shipType, assosiations[i].ResourcesPerSecound);
        }
    }
}
