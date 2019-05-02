using Imperium;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDropPoint : MonoBehaviour
{
    private static HashSet<GameObject> _instances;
    private static HashSet<GameObject> _Instances
    {
        get
        {
            if(_instances == null)
            {
                _instances = new HashSet<GameObject>();
            }
            return _instances;
        }
    }
    
    private void Start()
    {
        _Instances.Add(this.gameObject);
    }

    public static GameObject FindClosest(GameObject target)
    {   
        Player targetPlayer = PlayerDatabase.Instance.GetObjectPlayer(target);
        GameObject closest = null;
        float smallestMagnitude = 999999999999999999999999999999999f;
        foreach(GameObject gameObject in _Instances)
        {
            if(targetPlayer != PlayerDatabase.Instance.GetObjectPlayer(gameObject))
            {
                continue;
            }
            else
            {
                StationController stationController = gameObject.GetComponent<StationController>();
                if(stationController != null && !stationController.Constructed)
                {
                    continue;
                }
            }

            float magnitude = (target.transform.position - gameObject.transform.position).sqrMagnitude;
            if(magnitude < smallestMagnitude)
            {
                closest = gameObject;
                smallestMagnitude = magnitude;
            }
        }

        return closest;
    }

    private void OnDestroy()
    {
        _instances.Remove(this.gameObject);
    }
}
