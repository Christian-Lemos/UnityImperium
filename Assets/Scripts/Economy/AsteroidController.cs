﻿using Imperium.Enum;
using UnityEngine;
using System.Collections.Generic;
public class AsteroidController : MonoBehaviour
{
    public ResourceType resourceType;

    [SerializeField]
    private int resourceQuantity;

    public int ResourceQuantity
    {
        set
        {
            if (value <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                resourceQuantity = value;
            }
        }
        get
        {
            return resourceQuantity;
        }
    }

    public static readonly Dictionary<ResourceType, Color> asteroidColors = new Dictionary<ResourceType, Color>()
    {
          {ResourceType.Metal, Color.black},
          {ResourceType.Crystal, Color.magenta},
          {ResourceType.Energy, Color.yellow},
    };

    private void Start()
    {
        Material material = GetComponentInChildren<Renderer>().material;
        material.color = asteroidColors[resourceType];
    }
}