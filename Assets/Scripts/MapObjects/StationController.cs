using Imperium;
using Imperium.MapObjects;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapObject))]
public class StationController : MonoBehaviour, ISerializable<StationControllerPersistance>
{
    
    public float constructionProgress;

    public Station Station { get; set;}
    public StationType stationType;

    private MapObjectCombatter mapObjectCombatter;


    private bool _constructed;
    public bool Constructed
    {
        get
        {
            return _constructed;
        }
        set
        {
            _constructed = value;
            if(value == false)
            {
                try
                {
                    Station.combatStats.fieldOfViewDistance = 0f;
                }
                catch
                {

                }
                
            }
            else
            {
                Station.combatStats.fieldOfViewDistance = StationFactory.getInstance().CreateStation(stationType).combatStats.fieldOfViewDistance;
            }
        }
    }

    public void AddConstructionProgress(int progress)
    {
        Station.combatStats.HP += progress;
        float addedContructionProgress = (100 * (float)progress) / Station.combatStats.maxHP;

        constructionProgress += addedContructionProgress;

        if (constructionProgress >= 100)
        {
            Constructed = true;
            StartCoroutine(mapObjectCombatter.ShieldRegeneration());
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

        return new StationControllerPersistance(Constructed, constructionProgress, GetComponent<MapObject>().Serialize(), Station, stationType,turretControllerPersistances, true);
    }

    public ISerializable<StationControllerPersistance> SetObject(StationControllerPersistance serializedObject)
    {
        this.Constructed = serializedObject.constructed;
        this.constructionProgress = serializedObject.constructionProgress;
        this.Station = serializedObject.station;
        this.stationType = serializedObject.stationType;

        if(serializedObject.initialized)
        {
            foreach(TurretControllerPersistance turretControllerPersistance in serializedObject.turretControllerPersistances)
            {
                transform.GetChild(turretControllerPersistance.turretIndex).GetComponent<TurretController>().SetObject(turretControllerPersistance);
            }
        }
        

        return this;
    }

    

    private void Start()
    {
        if(Station == null)
        {
             Station = StationFactory.getInstance().CreateStation(stationType);
        }
       
        
        mapObjectCombatter = GetComponent<MapObjectCombatter>();
        mapObjectCombatter.combatStats = Station.combatStats;

        Station.combatStats.HP = (int)(Station.combatStats.maxHP * constructionProgress) / 100;

        if (constructionProgress >= 100)
        {
            Constructed = true;
            StartCoroutine(mapObjectCombatter.ShieldRegeneration());
        }
        else
        {
            Station.combatStats.Shields = 0;
        }

        mapObjectCombatter = GetComponent<MapObjectCombatter>();
        mapObjectCombatter.combatStats = Station.combatStats;
    }

    private void Update()
    {
        //Debug.Log(mapObjectCombatter.combatStats.hp);
        if (Constructed)
        {
            mapObjectCombatter.FireAtClosestTarget();
        }
    }
}