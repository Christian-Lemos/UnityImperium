using UnityEngine;
using System.Collections;
using Assets.Lib.Civilization;
using Imperium.Misc;

[DisallowMultipleComponent]
public class ShipHPRegen : Modifier
{
    private ShipController shipController;

    private int hpPerLevel = 5;

    private Timer timer;

    public override string Description => "Regenerating " + hpPerLevel + " HP per second";

    public override string Name => "Ship HP regeneration";

    public override string Icon => "ship_regen";

    public override void Modify()
    {
        this.timer.Execute();
        if(this.timer.IsFinished)
        {
            this.timer.ResetTimer();
        }
        base.active = true;
    }

    public override void ReverseModify()
    {
        this.timer.timerSet = false;
        this.timer.ResetTimer();
        base.active = false;
    }

    // Use this for initialization
    public new void Start()
    {
        base.modifierType = ModifierType.ShipHPRegen;
        base.ExecuteEveryUpdate = true;
        this.shipController = GetComponent<ShipController>();

        this.timer = new Timer(1f, true, AddHP);
        base.Level = 1;
        
        base.Start();

    }

    private void AddHP()
    {
       int addedHP = base.Level * this.hpPerLevel;

       shipController.Ship.combatStats.HP += addedHP;
    }

}
