using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium;
using Imperium.Enum;
using Imperium.Misc;
using Imperium.Combat;
public abstract class ObjectController : MonoBehaviour {



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
        int shipLayer = 1 << (int)ObjectLayers.Ship;
       // Debug.Log(this.gameObject);
        //Debug.Log(this.gameObject.transform);
        //Debug.Log(stats);

        Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, stats.FieldOfViewDistance, shipLayer);
        GameObject closestTarget = null;
        float closestDistance = 0f;
        int thisPlayer = PlayerDatabase.Instance.GetObjectPlayer(this.gameObject);
        foreach (Collider collider in colliders)
        {
            if (!PlayerDatabase.Instance.IsFromPlayer(collider.gameObject, thisPlayer) && !collider.gameObject.Equals(this.gameObject))
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
