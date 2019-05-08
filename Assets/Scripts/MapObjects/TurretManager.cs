using UnityEngine;
using System.Collections;
using Assets.Lib;
using Imperium;

[RequireComponent(typeof(ICombatable))]
public class TurretManager : MonoBehaviour
{

    private ICombatable combatable;
    // Use this for initialization
    void Start()
    {
        combatable = GetComponent<ICombatable>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private readonly int fireLayer = 1 << (int)ObjectLayers.Ship | 1 << (int)ObjectLayers.Station;



    public void FireAtClosestTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, combatable.CombatStats.FieldOfView, fireLayer);
        GameObject closestTarget = null;
        float smallerSqrMagnitude = 0f;
        Player thisPlayer = PlayerDatabase.Instance.GetObjectPlayer(gameObject);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<MapObject>() != null && !PlayerDatabase.Instance.IsFromPlayer(collider.gameObject, thisPlayer) && !collider.gameObject.Equals(gameObject))
            {
                float sqrMagnitude = (collider.gameObject.transform.position - gameObject.transform.position).sqrMagnitude;
                if (sqrMagnitude >= smallerSqrMagnitude && sqrMagnitude <= combatable.CombatStats.FieldOfView * combatable.CombatStats.FieldOfView)
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

    private void FireTurrets(GameObject target)
    {
        TurretController[] turrets = gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turret in turrets)
        {
            turret.Fire(target);
        }
    }

    public void SetFirePriority(GameObject target)
    {
        TurretController[] turrets = gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turret in turrets)
        {
            turret.SetFirePriority(target);
        }
    }
}
