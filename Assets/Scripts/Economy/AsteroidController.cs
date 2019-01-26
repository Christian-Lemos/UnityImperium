using Imperium.Enum;
using UnityEngine;

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
}