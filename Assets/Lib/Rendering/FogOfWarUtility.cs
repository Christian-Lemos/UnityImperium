using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imperium.Rendering
{
    [System.Serializable]
    public class FogOfWarUtility
    {
        private FogOfWarUtility() { }

        public static ICollection<GameObject> GetVisibleObjects(params int[] players)
        {
            ICollection<GameObject> playersGOs = new HashSet<GameObject>();
            ICollection<GameObject> visible = new HashSet<GameObject>();
            for (int i = 0; i < players.Length; i++)
            {
                HashSet<GameObject> playerGOs = PlayerDatabase.Instance.GetObjects(players[i]);
                foreach (GameObject @object in playerGOs)
                {
                    playersGOs.Add(@object);
                }
            }

            foreach (GameObject gameObject in playersGOs)
            {
                FOWAgent agent = gameObject.GetComponent<FOWAgent>();
                if (agent != null)
                {
                    foreach (GameObject @object in agent.visibleObjects)
                    {
                        if (@object != null && !visible.Contains(@object))
                        {
                            visible.Add(@object);
                        }
                    }
                }
            }

            return visible;
        }

        public static bool IsGameObjectVisible(GameObject gameObject, params int[] players)
        {
            return GetVisibleObjects(players).Contains(gameObject);
        }

        public static void SetRendering(bool value, params GameObject[] gameObjects)
        {
            for(int i = 0; i < gameObjects.Length; i++)
            {
                Render(gameObjects[i], value);
            }
        }

        public static void SetRendering(bool value, ICollection<GameObject> gameObjects)
        {
            foreach(GameObject gameObject in gameObjects)
            {
                Render(gameObject, value);
            }
        }

        private static void Render(GameObject gameObject, bool value)
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
    }
}