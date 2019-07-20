using Assets.Lib.Civilization;
using Imperium;
using UnityEngine;

[DisallowMultipleComponent]
public class ShipHPModifier : Modifier
{
    public bool heal = false;

    [SerializeField]
    private int baseMaxHP;

    private int porcentagePerLevel = 5;
    private int rawPerLevel = 50;
    private ShipController shipController;

    public override string Description
    {
        get
        {
            GetHpAdders(out int porcentage, out int raw);
            return "Ship HP increased by " + (porcentage + raw);
        }
    }

    public override string Icon => "ship_armor";
    public override string Name => "Ship Armor " + this.Level;
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

    public override bool DoesStack => false;

    public void GetHpAdders(out int porcentage, out int raw)
    {
        porcentage = (int)(baseMaxHP * ((float)(porcentagePerLevel * base.Level) / 100));
        raw = base.Level * rawPerLevel;
    }

    public override void Modify()
    {
        GetHpAdders(out int porcentage, out int raw);
        ShipController.Ship.combatStats.MaxHP += porcentage + raw;
        if (heal)
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
    private new void Start()
    {
        base.modifierType = ModifierType.ShipMaxHPBuffer;
        base.ExecuteEveryUpdate = false;

        if (this.ShipController == null)
        {
            Destroy(this);
        }

        //baseMaxHP = this.ShipController.Ship.combatStats.MaxHP;

        baseMaxHP = ShipFactory.getInstance().CreateShip(this.ShipController.shipType).combatStats.MaxHP;

        base.Start();
    }
}