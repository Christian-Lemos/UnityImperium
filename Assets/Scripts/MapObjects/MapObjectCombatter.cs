using Imperium;
using Imperium.Combat;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MapObject))]
public class MapObjectCombatter : MonoBehaviour
{
    public float lowestTurretRange;
    public CombatStats combatStats;
    private readonly int fireLayer = 1 << (int)ObjectLayers.Ship | 1 << (int)ObjectLayers.Station;

    

    public void FireAtClosestTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, combatStats.FieldOfView, fireLayer);
        GameObject closestTarget = null;
        float smallerSqrMagnitude = 0f;
        Player thisPlayer = PlayerDatabase.Instance.GetObjectPlayer(gameObject);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<MapObject>() != null && !PlayerDatabase.Instance.IsFromPlayer(collider.gameObject, thisPlayer) && !collider.gameObject.Equals(gameObject))
            {
                float sqrMagnitude = (collider.gameObject.transform.position - gameObject.transform.position).sqrMagnitude;
                if (sqrMagnitude >= smallerSqrMagnitude && sqrMagnitude <= combatStats.FieldOfView * combatStats.FieldOfView)
                {
                    closestTarget = collider.gameObject;
                }
            }
        }
        if (closestTarget != null)
        {
            FireTurrets(closestTarget);
        }
    }

    public float GetLowestTurretRange()
    {
        float lowest = combatStats.FieldOfView;
        TurretController[] turretControllers = gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turretController in turretControllers)
        {
            if (turretController.turret.range > lowest)
            {
                lowest = turretController.turret.range;
            }
        }
        return lowest;
    }

    public IEnumerator ShieldRegeneration()
    {
        while (true)
        {
            if (combatStats.Shields + combatStats.ShieldRegen > combatStats.MaxShields)
            {
                combatStats.Shields = combatStats.MaxShields;
            }
            else
            {
                combatStats.Shields += combatStats.ShieldRegen;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void FireTurrets(GameObject target)
    {
        TurretController[] turrets = gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turret in turrets)
        {
            turret.Fire(target);
        }
    }

    private void Start()
    {
        lowestTurretRange = GetLowestTurretRange();
        if(GetComponent<CombatStatsCanvasController>() != null)
            GetComponent<CombatStatsCanvasController>().enabled = true;
    }
}