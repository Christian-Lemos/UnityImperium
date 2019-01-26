using Imperium;
using Imperium.Enum;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class ObjectController : MonoBehaviour
{
    public float lowestTurretRange;
    public Stats stats;
    private readonly int fireLayer = 1 << (int)ObjectLayers.Ship | 1 << (int)ObjectLayers.Station;

    public void TakeDamage(int damage)
    {
        int shields = stats.Shields;
        if (shields <= damage)
        {
            int hpDamage = shields - damage;
            stats.Shields = 0;
            stats.HP -= -hpDamage;
            if (stats.HP <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            stats.Shields -= damage;
        }
    }

    protected void FireAtClosestTarget()
    {
        // Debug.Log(this.gameObject);
        //Debug.Log(this.gameObject.transform);
        //Debug.Log(statsfireLayer
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, stats.FieldOfViewDistance, fireLayer);
        GameObject closestTarget = null;
        float closestDistance = 0f;
        int thisPlayer = PlayerDatabase.Instance.GetObjectPlayer(gameObject);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<ObjectController>() != null && !PlayerDatabase.Instance.IsFromPlayer(collider.gameObject, thisPlayer) && !collider.gameObject.Equals(gameObject))
            {
                float distance = Vector3.Distance(collider.gameObject.transform.position, gameObject.transform.position);
                if (distance >= closestDistance && distance <= stats.FieldOfViewDistance)
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

    protected float GetLowestTurretRange()
    {
        float lowest = stats.FieldOfViewDistance;
        TurretController[] turretControllers = gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turretController in turretControllers)
        {
            if (turretController.Turret.Range > lowest)
            {
                lowest = turretController.Turret.Range;
            }
        }
        return lowest;
    }

    protected IEnumerator ShieldRegeneration()
    {
        while (true)
        {
            if (stats.Shields + stats.ShieldRegen > stats.MaxShields)
            {
                stats.Shields = stats.MaxShields;
            }
            else
            {
                stats.Shields += stats.ShieldRegen;
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

    private void OnDestroy()
    {
        try
        {
            int thisPlayer = PlayerDatabase.Instance.GetObjectPlayer(gameObject);
            PlayerDatabase.Instance.RemoveFromPlayer(gameObject, thisPlayer);
        }
        catch
        {
        }
    }
}