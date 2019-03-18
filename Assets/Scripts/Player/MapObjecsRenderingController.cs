using Imperium.Rendering;
using System.Collections.Generic;
using UnityEngine;

public class MapObjecsRenderingController : MonoBehaviour
{
    public int[] players;
    public List<GameObject> visibleObjects = new List<GameObject>();

    private void LateUpdate()
    {
        HashSet<GameObject> gameObjects = new HashSet<GameObject>();

        List<GameObject> visibleNow = new List<GameObject>();
        for (int i = 0; i < players.Length; i++)
        {
            HashSet<GameObject> playerGOs = PlayerDatabase.Instance.GetObjects(players[i]);
            foreach (GameObject @object in playerGOs)
            {
                gameObjects.Add(@object);
            }
        }

        foreach (GameObject gameObject in gameObjects)
        {
            FOWAgent agent = gameObject.GetComponent<FOWAgent>();
            if (agent != null)
            {
                foreach (GameObject @object in agent.visibleObjects)
                {
                    if (@object != null && !visibleNow.Contains(@object))
                    {
                        visibleNow.Add(@object);
                    }
                }
            }
        }

        foreach (GameObject gameObject in visibleNow)
        {
            MeshRenderer[] meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].enabled = true;
            }

            TrailRenderer[] trailRenderers = gameObject.GetComponentsInChildren<TrailRenderer>();
            for (int i = 0; i < trailRenderers.Length; i++)
            {
                trailRenderers[i].enabled = true;
            }
        }

        foreach (GameObject gameObject in visibleObjects)
        {

            if (gameObject != null && !visibleNow.Contains(gameObject) && gameObject.GetComponent<INonExplorable>() != null)
            {
                MeshRenderer[] meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
                for (int i = 0; i < meshRenderers.Length; i++)
                {
                    meshRenderers[i].enabled = false;
                }

                TrailRenderer[] trailRenderers = gameObject.GetComponentsInChildren<TrailRenderer>();
                for (int i = 0; i < trailRenderers.Length; i++)
                {
                    trailRenderers[i].enabled = false;
                }
            }
           
        }

      

        visibleObjects = visibleNow;
    }
}