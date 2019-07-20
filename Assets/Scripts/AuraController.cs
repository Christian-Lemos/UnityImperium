using Assets.Lib.Civilization;
using Imperium;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AuraController : MonoBehaviour
{
    public HashSet<GameObject> collection = new HashSet<GameObject>();
    public int level;
    public ModifierType modifierType;

    [SerializeField]
    private float radius;

    private float radius_sqrt;
    
    private Type type;

    private Player player;
    public float Radius { get => radius; set { radius = value; radius_sqrt = radius * radius; } }


    private HashSet<GameObject> GetGameObjectsWithinRadius()
    {
        HashSet<GameObject> gameObjects = new HashSet<GameObject>();
        
        HashSet<GameObject> mapObjects = new HashSet<GameObject>(PlayerDatabase.Instance.GetObjects(player));

        mapObjects.RemoveWhere((GameObject g) =>
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
        player = PlayerDatabase.Instance.GetObjectPlayer(this.gameObject);
        type = ModifierFactory.getInstance().GetModifierType(modifierType);
        Radius = Radius;
      
        if (PlayerDatabase.Instance.GetObjectPlayer(this.gameObject).Number != 0)
        {
            Destroy(this.gameObject);
        }
        this.coroutine = UpdateEnumerator;
        StartCoroutine(this.coroutine);
    }



    private void UpdateAffectedObjects()
    {
        HashSet<GameObject> gameObjects = GetGameObjectsWithinRadius();

        foreach (GameObject go in gameObjects)
        {
            Modifier m = go.AddModifier(type, this.level, true);
            if (m != null)
            {
                m.AddGuardian(this.gameObject);
            }

        }

        foreach (GameObject go in collection)
        {
            if (!gameObjects.Contains(go))
            {
                Modifier modifier = (Modifier)go.GetComponent(type);
                if (modifier != null)
                {
                    modifier.RemoveGuardian(this.gameObject);
                    go.RemoveModifier(modifier);
                    //modifier.ReverseModify();
                    //Destroy(go.GetComponent(type));
                }
            }
        }
        collection = gameObjects;
    }

    private IEnumerator coroutine;

    private IEnumerator UpdateEnumerator
    {
        get
        {
            while (true)
            {
                UpdateAffectedObjects();
                yield return new WaitForSeconds(0.5f);
            }

        }
    }

    private void OnDestroy()
    {
        StopCoroutine(this.coroutine);
    }
}