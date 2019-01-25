using System.Collections;
using UnityEngine;
using Imperium;
using Imperium.Enum;

[DisallowMultipleComponent]
public abstract class ObjectController : MonoBehaviour {



    private readonly int fireLayer = 1 << (int)ObjectLayers.Ship | 1 << (int)ObjectLayers.Station;

    public Stats stats;
    public float lowestTurretRange;

    public void TakeDamage(int damage)
    {

        int shields = this.stats.Shields;
        if (shields <= damage)
        {
            int hpDamage = shields - damage;
            this.stats.Shields = 0;
            this.stats.HP -= -hpDamage;
            if (this.stats.HP <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            this.stats.Shields -= damage;
        }
    }

    protected void FireAtClosestTarget()
    {
        
        // Debug.Log(this.gameObject);
        //Debug.Log(this.gameObject.transform);
        //Debug.Log(statsfireLayer
        Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, stats.FieldOfViewDistance, fireLayer);
        GameObject closestTarget = null;
        float closestDistance = 0f;
        int thisPlayer = PlayerDatabase.Instance.GetObjectPlayer(this.gameObject);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<ObjectController>() != null && !PlayerDatabase.Instance.IsFromPlayer(collider.gameObject, thisPlayer) && !collider.gameObject.Equals(this.gameObject))
            {
                float distance = Vector3.Distance(collider.gameObject.transform.position, this.gameObject.transform.position);
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
        float lowest = this.stats.FieldOfViewDistance;
        TurretController[] turretControllers = this.gameObject.GetComponentsInChildren<TurretController>(false);
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
        TurretController[] turrets = this.gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turret in turrets)
        {
            turret.Fire(target);
        }
    }

    private void OnDestroy()
    {
        try
        {
            int thisPlayer = PlayerDatabase.Instance.GetObjectPlayer(this.gameObject);
            PlayerDatabase.Instance.RemoveFromPlayer(this.gameObject, thisPlayer);
        }
        catch
        {

        }
        
    }
} 
