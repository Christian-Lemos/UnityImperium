using Imperium;
using Imperium.Enum;
using UnityEngine;

public class StationController : ObjectController
{
    public bool constructed;
    public float constructionProgress;
    public Station station;
    public StationType stationType;

    public void AddConstructionProgress(int progress)
    {
        station.stats.HP += progress;
        float addedContructionProgress = (100 * (float)progress) / station.stats.maxHP;

        constructionProgress += addedContructionProgress;

        if (constructionProgress >= 100)
        {
            constructed = true;
            StartCoroutine(ShieldRegeneration());
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

    private void Start()
    {
        station = StationFactory.getInstance().CreateStation(stationType);
        stats = station.stats;

        lowestTurretRange = base.GetLowestTurretRange();

        station.stats.HP = (int)(station.stats.maxHP * constructionProgress) / 100;

        if (constructionProgress >= 100)
        {
            constructed = true;
            StartCoroutine(ShieldRegeneration());
        }
        else
        {
            station.stats.Shields = 0;
        }
    }

    private void Update()
    {
        if (constructed)
        {
            FireAtClosestTarget();
        }
    }
}