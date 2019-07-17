using UnityEngine;
using System.Collections;
using Assets.Lib.Civilization;
using System;

[DisallowMultipleComponent]
public class ShipHPModifier : Modifier
{
    private ShipController shipController;
    [SerializeField]
    private int baseMaxHP;

    private int porcentagePerLevel = 5;
    private int rawPerLevel = 50;
    public int PorcentagePerLevel { get => porcentagePerLevel; private set => porcentagePerLevel = value; }
    public ShipController ShipController
    {
        get
        {
            if (shipController == null)
            {
                shipController = GetComponent<ShipController>();
            }
            return shipController;
        }
        set => shipController = value;
    }

    public override void Modify()
    {
        int porcentAddedHP = (int)(baseMaxHP * ((float)(porcentagePerLevel * base.Level) / 100));
        int rawAddedHP = base.Level * rawPerLevel;
        ShipController.Ship.combatStats.MaxHP += porcentAddedHP + rawAddedHP;
        base.active = true;
    }

    public override void ReverseModify()
    {
        this.ShipController.Ship.combatStats.MaxHP = baseMaxHP;
        base.active = false;
    }

    // Use this for initialization
    new void Start()
    {
        base.modifierType = ModifierType.ShipMaxHPBuffer;
        base.ExecuteEveryUpdate = false;
        
        baseMaxHP = this.ShipController.Ship.combatStats.MaxHP;
        Debug.Log("Original: " + baseMaxHP);
        base.Start();

    }
}
