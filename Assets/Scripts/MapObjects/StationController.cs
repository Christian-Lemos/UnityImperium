using Assets.Lib;
using Imperium;
using Imperium.Combat;
using Imperium.MapObjects;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour, ISerializable<StationControllerPersistance>, IHittable, ICombatable
{
    public float constructionProgress;

    public StationType stationType;
    private bool _constructed;
    private ShieldRegenerator shieldRegenerator;
    private TurretManager turretManager;
    public CombatStats CombatStats => Station.combatStats;

    public bool Constructed
    {
        get
        {
            return _constructed;
        }
        set
        {
            _constructed = value;
            if (value == false)
            {
                try
                {
                    Station.combatStats.FieldOfView = 0f;
                }
                catch
                {
                }
            }
            else
            {
                Station.combatStats.FieldOfView = StationFactory.getInstance().CreateStation(stationType).combatStats.FieldOfView;
            }
            EnableShieldRegeneration(value);
            EnableTurrets(value);
        }
    }

    public Station Station { get; set; }

    public void AddConstructionProgress(int progress)
    {
        Station.combatStats.HP += progress;
        float addedContructionProgress = (100 * (float)progress) / Station.combatStats.MaxHP;

        constructionProgress += addedContructionProgress;

        if (constructionProgress >= 100)
        {
            Constructed = true;
        }
    }

    public void AttackTarget(GameObject target)
    {
        if (!target.Equals(gameObject))
        {
            TurretController[] turrets = gameObject.GetComponentsInChildren<TurretController>(false);
            foreach (TurretController turret in turrets)
            {
                turret.SetFirePriority(target);
            }
        }
    }

    public StationControllerPersistance Serialize()
    {
        List<TurretControllerPersistance> turretControllerPersistances = new List<TurretControllerPersistance>();

        TurretController[] turretControllers = GetComponentsInChildren<TurretController>();

        foreach (TurretController turretController in turretControllers)
        {
            turretControllerPersistances.Add(turretController.Serialize());
        }

        return new StationControllerPersistance(Constructed, constructionProgress, GetComponent<MapObject>().Serialize(), Station, stationType, turretControllerPersistances, true);
    }

    public ISerializable<StationControllerPersistance> SetObject(StationControllerPersistance serializedObject)
    {
        this.Constructed = serializedObject.constructed;
        this.constructionProgress = serializedObject.constructionProgress;
        this.Station = serializedObject.station;
        this.stationType = serializedObject.stationType;

        if (serializedObject.initialized)
        {
            foreach (TurretControllerPersistance turretControllerPersistance in serializedObject.turretControllerPersistances)
            {
                transform.GetChild(turretControllerPersistance.turretIndex).GetComponent<TurretController>().SetObject(turretControllerPersistance);
            }
        }

        return this;
    }

    public void TakeHit(Bullet bullet)
    {
        int damage = bullet.damage;
        int shields = Station.combatStats.Shields;

        if (shields <= damage)
        {
            int hpDamage = shields - damage;
            Station.combatStats.Shields = 0;
            Station.combatStats.HP -= -hpDamage;
            if (Station.combatStats.HP <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Station.combatStats.Shields -= damage;
        }
    }

    private void EnableShieldRegeneration(bool value)
    {
        if (shieldRegenerator != null)
        {
            shieldRegenerator.enabled = value;
        }
    }

    private void EnableTurrets(bool value)
    {
        if (turretManager != null)
        {
            turretManager.enabled = value;
        }
    }

    private void Start()
    {
        if (Station == null)
        {
            Station = StationFactory.getInstance().CreateStation(stationType);
        }

        shieldRegenerator = GetComponent<ShieldRegenerator>();
        turretManager = GetComponent<TurretManager>();
        Station.combatStats.HP = (int)(Station.combatStats.MaxHP * constructionProgress) / 100;

        if (constructionProgress >= 100)
        {
            Constructed = true;
            EnableShieldRegeneration(true);
        }
        else
        {
            Constructed = false;
            Station.combatStats.Shields = 0;
        }
    }
}