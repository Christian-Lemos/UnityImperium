using Imperium;
using Imperium.MapObjects;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using UnityEngine;

[RequireComponent(typeof(MapObject))]
public class StationController : MonoBehaviour, ISerializable<StationControllerPersistance>
{
    public bool constructed;
    public float constructionProgress;

    public Station Station { get; set;}
    public StationType stationType;

    private MapObjectCombatter mapObjectCombatter;

    public void AddConstructionProgress(int progress)
    {
        Station.combatStats.HP += progress;
        float addedContructionProgress = (100 * (float)progress) / Station.combatStats.maxHP;

        constructionProgress += addedContructionProgress;

        if (constructionProgress >= 100)
        {
            constructed = true;
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
        return new StationControllerPersistance(constructed, constructionProgress, GetComponent<MapObject>().Serialize(), Station, stationType);
    }

    public ISerializable<StationControllerPersistance> SetObject(StationControllerPersistance serializedObject)
    {
        this.constructed = serializedObject.constructed;
        this.constructionProgress = serializedObject.constructionProgress;
        this.Station = serializedObject.station;
        this.stationType = serializedObject.stationType;
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
            constructed = true;
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
        if (constructed)
        {
            mapObjectCombatter.FireAtClosestTarget();
        }
    }
}