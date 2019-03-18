using Imperium.Rendering;
using System.Collections.Generic;
using UnityEngine;

public class MapObjecsRenderingController : MonoBehaviour
{
    public int[] players;
    public ICollection<GameObject> visibleObjects = new HashSet<GameObject>();

    private void LateUpdate()
    {
        ICollection<GameObject> visibleNow = FogOfWarUtility.GetVisibleObjects(players);

        FogOfWarUtility.SetRendering(true, visibleNow);

        foreach (GameObject gameObject in visibleObjects)
        {
            if (gameObject != null && !visibleNow.Contains(gameObject) && gameObject.GetComponent<INonExplorable>() != null)
            {
                FogOfWarUtility.SetRendering(false, gameObject);
            }
        }
        visibleObjects = visibleNow;
    }
}