using UnityEngine;
using UnityEditor;
using Imperium.MapObjects;
using System.Collections.Generic;
using Imperium;

public class ShipArmorRB : ResearchBehaviour
{
    
    protected override void Initiate()
    {
        Spawner.Instance.AddShipSpawnMiddleware(this.MiddleWare);
        HashSet<GameObject> gameObjects = PlayerDatabase.Instance.GetObjects(base.player);

        foreach(GameObject go in gameObjects)
        {
            if(go.layer == (int)ObjectLayers.Ship)
            {
                AddModifier(go, false);
            }
        }
    }

    private void MiddleWare(ref GameObject instance)
    {
        if (instance == null)
        {
            throw new System.ArgumentNullException(nameof(instance));
        }

        AddModifier(instance, true);
    }

    private void AddModifier(GameObject go, bool heal)
    {
        ShipHPModifier shipHPModifier = go.AddComponent<ShipHPModifier>();
        shipHPModifier.Level = 1;
        shipHPModifier.heal = heal;
    }

    private void OnDestroy()
    {
        Spawner.Instance.RemoveShipSpawnMiddleware(this.MiddleWare);
    }

}