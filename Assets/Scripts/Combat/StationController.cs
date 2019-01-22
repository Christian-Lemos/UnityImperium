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

        int stationHP = (int)(this.station.StationStats.MaxHP * constructionProgress) / 100;

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
}