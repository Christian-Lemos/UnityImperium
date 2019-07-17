using UnityEngine;
using System.Collections;
using Assets.Lib.Civilization;

[DisallowMultipleComponent]
public class ShipHPModifier : Modifier
{
    private ShipController shipController;
    private int baseMaxHP;

    private int porcentagePerLevel = 5;
    private int rawPerLevel = 50;
    public int PorcentagePerLevel { get => porcentagePerLevel; private set => porcentagePerLevel = value; }

    public override void Modify()
    {
        int porcentAddedHP = (int) (baseMaxHP * ((float) (porcentagePerLevel * base.Level) / 100));
        int rawAddedHP = base.Level * rawPerLevel;
        
        shipController.Ship.combatStats.MaxHP += porcentAddedHP + rawAddedHP;
        base.active = true;
    }

    public override void ReverseModify()
    {
        this.shipController.Ship.combatStats.MaxHP = baseMaxHP;
        base.active = false;
    }

    // Use this for initialization
    void Start()
    {
        base.modifierType = ModifierType.ShipMaxHPBuffer;
        base.ExecuteEveryUpdate = false;
        this.shipController = GetComponent<ShipController>();
        baseMaxHP = this.shipController.Ship.combatStats.MaxHP;
        if(!base.active)
        {
            Modify();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(base.active && base.ExecuteEveryUpdate)
        {
            Modify();
        }
    }
}
