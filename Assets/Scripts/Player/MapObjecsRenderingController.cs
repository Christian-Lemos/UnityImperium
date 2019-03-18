using Imperium.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjecsRenderingController : MonoBehaviour
{
    public int[] players;
    public ICollection<GameObject> visibleObjects = new HashSet<GameObject>();

    private void Start()
    {
        StartCoroutine(RenderEnumerator());
    }

    private IEnumerator RenderEnumerator()
    {
        while(true)
        {
            ICollection<GameObject> visibleNow = FogOfWarUtility.GetVisibleObjects(players);

            //FogOfWarUtility.SetRendering(true, visibleNow);

            foreach (GameObject gameObject in visibleObjects)
            {
                if (gameObject != null && !visibleNow.Contains(gameObject) && gameObject.GetComponent<INonExplorable>() != null)
                {
                    FogOfWarUtility.SetRendering(false, gameObject);
                }
            }

            foreach(GameObject gameObject in visibleNow)
            {
                if(!visibleObjects.Contains(gameObject))
                {
                    FogOfWarUtility.SetRendering(true, gameObject);
                }
            }

            visibleObjects = visibleNow;
            yield return null;
            yield return null;
            yield return null;
        }
    }

}