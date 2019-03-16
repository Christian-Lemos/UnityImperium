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

    public void TakeDamage(int damage)
    {
        int shields = combatStats.Shields;
        if (shields <= damage)
        {
            int hpDamage = shields - damage;
            combatStats.Shields = 0;
            combatStats.HP -= -hpDamage;
            if (combatStats.HP <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            combatStats.Shields -= damage;
        }
    }

    public void FireAtClosestTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, combatStats.fieldOfViewDistance, fireLayer);
        GameObject closestTarget = null;
        float closestDistance = 0f;
        int thisPlayer = PlayerDatabase.Instance.GetObjectPlayer(gameObject);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<MapObject>() != null && !PlayerDatabase.Instance.IsFromPlayer(collider.gameObject, thisPlayer) && !collider.gameObject.Equals(gameObject))
            {
                float distance = Vector3.Distance(collider.gameObject.transform.position, gameObject.transform.position);
                if (distance >= closestDistance && distance <= combatStats.fieldOfViewDistance)
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
        float lowest = combatStats.fieldOfViewDistance;
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
            if (combatStats.Shields + combatStats.shieldRegen > combatStats.maxShields)
            {
                combatStats.Shields = combatStats.maxShields;
            }
            else
            {
                combatStats.Shields += combatStats.shieldRegen;
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
        GetComponent<CombatStatsCanvasController>().enabled = true;
    }
}