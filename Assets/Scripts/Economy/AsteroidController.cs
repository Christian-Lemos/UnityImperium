using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;
public class AsteroidController : MonoBehaviour {

    public ResourceType resourceType;

    [SerializeField]
    private int resourceQuantity;
    public int ResourceQuantity
    {
        set
        {
            if(value <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                this.resourceQuantity = value;
            }
        }
        get
        {
            return this.resourceQuantity;
        }
    }
}
