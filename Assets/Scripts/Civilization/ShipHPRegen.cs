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
    public override void Modify()
    {
        Debug.Log("Executing Timer");
        this.timer.Execute();
        if(this.timer.IsFinished)
        {
            Debug.Log("timer is finished");
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
    void Start()
    {
        base.modifierType = ModifierType.ShipMaxHPBuffer;
        base.ExecuteEveryUpdate = true;
        this.shipController = GetComponent<ShipController>();

        this.timer = new Timer(1f, true, AddHP);
        base.Level = 1;
        if(!base.active)
        {
            Modify();
        }

    }

    private void AddHP()
    {
       int addedHP = base.Level * this.hpPerLevel;

       shipController.Ship.combatStats.HP += addedHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(base.active && base.ExecuteEveryUpdate)
        {
            Debug.Log("Modifing");
            Modify();
        }
    }
}
