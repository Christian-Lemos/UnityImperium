using UnityEngine;
using UnityEditor;
using Imperium.MapObjects;
using System.Collections.Generic;
using Imperium;
using Assets.Lib.Civilization;
using Assets.Lib.Events;
public class FlagshipRegenAuraRB : LeveledResearchBehaviour
{
    
    protected override void Initiate()
    {
        HashSet<GameObject> gameObjects = PlayerDatabase.Instance.GetObjects(base.player);

        foreach(GameObject go in gameObjects)
        {
            if(go.layer == (int)ObjectLayers.Ship)
            {
                if(go.GetComponent<ShipController>().shipType == ShipType.MotherShip)
                {
                    AuraController auraController = go.AddComponent<AuraController>();
                    auraController.level = 1;
                    auraController.modifierType = ModifierType.ShipHPRegen;
                    auraController.Radius = 20f;
                    break;
                }
            }
        }
    }



    public override void UpdateLevel()
    {

    }
}