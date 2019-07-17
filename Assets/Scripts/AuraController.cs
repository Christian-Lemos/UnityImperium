using Assets.Lib.Civilization;
using Imperium;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class AuraController : MonoBehaviour
{
    public HashSet<GameObject> collection = new HashSet<GameObject>();
    public List<GameObject> test;
    public int level;
    public ModifierType modifierType;

    [SerializeField]
    private float radius;

    private float radius_sqrt;
    private int selectLayer = (1 << (int)ObjectLayers.Ship) | (1 << (int)ObjectLayers.Station);
    private Type type;
    public float Radius { get => radius; set { radius = value; radius_sqrt = radius * radius; } }

    private void FilterGameObjects(ref HashSet<GameObject> gameObjects)
    {
        gameObjects.RemoveWhere((GameObject g) =>
        {
            return !PlayerDatabase.Instance.AreFromSamePlayer(this.gameObject, g);
        });
    }

    private HashSet<GameObject> GetGameObjectsWithinRadius()
    {
        HashSet<GameObject> gameObjects = new HashSet<GameObject>();
        
        HashSet<MapObject> mapObjects = MapObject.GetMapObjects();
        Debug.Log(mapObjects.Count);

        mapObjects.RemoveWhere((MapObject g) =>
        {
            if(g.gameObject.layer != (int)ObjectLayers.Ship && g.gameObject.layer != (int)ObjectLayers.Station)
            {
                return true;
            }
            else
            {
                float sqrMag = (g.transform.position - this.gameObject.transform.position).sqrMagnitude;
                bool removed = sqrMag > this.radius_sqrt;
                
                if (!removed)
                {
                    gameObjects.Add(g.gameObject);
                }
                return removed;
            }
        });

        return gameObjects;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, Radius);
    }

    // Use this for initialization
    private void Start()
    {
        type = ModifierFactory.getInstance().GetModifierType(modifierType);
        Radius = Radius;
      
        if (PlayerDatabase.Instance.GetObjectPlayer(this.gameObject).Number != 0)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        HashSet<GameObject> gameObjects = GetGameObjectsWithinRadius();
        
        FilterGameObjects(ref gameObjects);
        

        foreach(GameObject go in gameObjects)
        {
   
            Debug.Log("Creating modifier in:" + go.name);
            ModifierFactory.getInstance().AddModifierToGameObject(go, modifierType, this.level, true);
            
        }

        foreach(GameObject go in collection)
        {
            if(!gameObjects.Contains(go))
            {
                Debug.Log("Removing modifier in:" + go.name);
                Modifier modifier = (Modifier)go.GetComponent(type);
                if (modifier != null)
                {
                    modifier.ReverseModify();
                    Destroy(go.GetComponent(type));
                }
            }
        }

        collection = gameObjects;
 
    }
}