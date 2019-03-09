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
    public Station station;
    public StationType stationType;

    private MapObjectCombatter mapObjectCombatter;

    public void AddConstructionProgress(int progress)
    {
        station.combatStats.HP += progress;
        float addedContructionProgress = (100 * (float)progress) / station.combatStats.maxHP;

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
        return new StationControllerPersistance(constructed, constructionProgress, GetComponent<MapObject>().Serialize(), station, stationType);
    }

    public void SetObject(StationControllerPersistance serializedObject)
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        station = StationFactory.getInstance().CreateStation(stationType);

        mapObjectCombatter = GetComponent<MapObjectCombatter>();
        mapObjectCombatter.combatStats = station.combatStats;

        station.combatStats.HP = (int)(station.combatStats.maxHP * constructionProgress) / 100;

        if (constructionProgress >= 100)
        {
            constructed = true;
            StartCoroutine(mapObjectCombatter.ShieldRegeneration());
        }
        else
        {
            station.combatStats.Shields = 0;
        }
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