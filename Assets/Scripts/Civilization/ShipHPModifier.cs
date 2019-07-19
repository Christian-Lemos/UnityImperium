using UnityEngine;
using System.Collections;
using Assets.Lib.Civilization;
using System;
using Imperium;

[DisallowMultipleComponent]
public class ShipHPModifier : Modifier
{
    private ShipController shipController;
    [SerializeField]
    private int baseMaxHP;

    private int porcentagePerLevel = 5;
    private int rawPerLevel = 50;
    public int PorcentagePerLevel { get => porcentagePerLevel; private set => porcentagePerLevel = value; }


    public bool heal = false;

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
        GetHpAdders(out int porcentage, out int raw);
        ShipController.Ship.combatStats.MaxHP += porcentage + raw;
        if(heal)
        {
            ShipController.Ship.combatStats.HP = ShipController.Ship.combatStats.MaxHP;
            heal = false;
        }
        base.active = true;
    }

    public override void ReverseModify()
    {
        GetHpAdders(out int porcentAddedHP, out int rawAddedHP); 
        this.ShipController.Ship.combatStats.MaxHP -= porcentAddedHP + rawAddedHP;
        base.active = false;
    }

    // Use this for initialization
    new void Start()
    {
        base.modifierType = ModifierType.ShipMaxHPBuffer;
        base.ExecuteEveryUpdate = false;
        
        //baseMaxHP = this.ShipController.Ship.combatStats.MaxHP;
  
        baseMaxHP = ShipFactory.getInstance().CreateShip(this.ShipController.shipType).combatStats.MaxHP;
     
        base.Start();

    }

    public void GetHpAdders(out int porcentage, out int raw)
    {
        porcentage = (int)(baseMaxHP * ((float)(porcentagePerLevel * base.Level) / 100));
        raw = base.Level * rawPerLevel;
    }

}
