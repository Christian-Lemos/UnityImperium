using Imperium;
using Imperium.Enum;
using UnityEngine;
public class StationController : ObjectController
{
    public StationType stationType;
    public Station station;

    public bool constructed;
    public float constructionProgress;

    private void Start()
    {
        this.station = StationFactory.getInstance().CreateStation(stationType);
        this.stats = this.station.StationStats;

        lowestTurretRange = base.GetLowestTurretRange();

        this.station.StationStats.HP = (int)(this.station.StationStats.MaxHP * constructionProgress) / 100;

        if (constructionProgress >= 100)
        {
            constructed = true;
            StartCoroutine(ShieldRegeneration());
        }
        else
        {
            this.station.StationStats.Shields = 0;
        }
    }


    private void Update()
    {

        if (constructed)
        {
            FireAtClosestTarget();
        }
    }

    public void AddConstructionProgress(int progress)
    {
        this.station.StationStats.HP += progress;
        float addedContructionProgress = (100 * (float)progress) / this.station.StationStats.MaxHP;
  
        constructionProgress += addedContructionProgress;
        
        if (constructionProgress >= 100)
        {
            constructed = true;
            StartCoroutine(ShieldRegeneration());
        }
    }

    public override void AttackTarget(GameObject target)
    {
        if (!target.Equals(this.gameObject))
        {
            TurretController[] turrets = this.gameObject.GetComponentsInChildren<TurretController>(false);
            foreach (TurretController turret in turrets)
            {
                turret.SetFirePriority(target);
            }
        }
    }
}