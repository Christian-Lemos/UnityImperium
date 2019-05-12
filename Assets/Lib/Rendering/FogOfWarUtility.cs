using Imperium.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imperium.Rendering
{
    [System.Serializable]
    public class FogOfWarUtility
    {
        private FogOfWarUtility() { }

        public static HashSet<GameObject> GetVisibleObjects(params Player[] players)
        {
            HashSet<GameObjectMOC> gameObjectMOCs = new HashSet<GameObjectMOC>();
            HashSet<GameObject> visible = new HashSet<GameObject>();
            HashSet<GameObject> playersGOs = new HashSet<GameObject>();
            for (int i = 0; i < players.Length; i++)
            {
                HashSet<GameObject> playerGOs = PlayerDatabase.Instance.GetObjects(players[i]);
                foreach (GameObject @object in playerGOs)
                {
                    gameObjectMOCs.Add(new GameObjectMOC(@object));
                    playersGOs.Add(@object);
                }
            }


           /* HashSet<MapObject> mapObjects = MapObject.GetMapObjects();
            foreach(MapObject mapObject in mapObjects)
            {
                if(playersGOs.Contains(mapObject.gameObject))
                {
                    visible.Add(mapObject.gameObject);
                }
            }

            foreach(MapObject mapObject in mapObjects)
            {
                if(!visible.Contains(mapObject.gameObject))
                {
                    foreach(GameObjectMOC gomoc in gameObjectMOCs)
                    {
                        float dist = (gomoc.gameObject.transform.position - mapObject.gameObject.transform.position).sqrMagnitude;
                        if (dist < gomoc.fovSqr)
                        {
                            visible.Add(mapObject.gameObject);
                            break;
                        }
                    }
                }
            }*/

            MapObject[] mapObjects = GameObject.FindObjectsOfType<MapObject>();
            for(int i = 0; i < mapObjects.Length; i++)
            {
                if(playersGOs.Contains(mapObjects[i].gameObject))
                {
                    visible.Add(mapObjects[i].gameObject);
                }
            }

            for (int i = 0; i < mapObjects.Length; i++)
            {
                MapObject mapObject = mapObjects[i];

                if(!visible.Contains(mapObject.gameObject))
                {
                    foreach(GameObjectMOC gomoc in gameObjectMOCs)
                    {
                        float dist = (gomoc.gameObject.transform.position - mapObjects[i].gameObject.transform.position).sqrMagnitude;
                        if (dist < gomoc.fovSqr)
                        {
                            visible.Add(mapObject.gameObject);
                            break;
                        }
                    }
                }
            }

            return visible;
        }

        public static bool IsGameObjectVisible(GameObject gameObject, params Player[] players)
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
                meshRenderers[i].enabled = value;
            }

            TrailRenderer[] trailRenderers = gameObject.GetComponentsInChildren<TrailRenderer>();
            for (int i = 0; i < trailRenderers.Length; i++)
            {
                trailRenderers[i].enabled = value;
            }
        }

        private class GameObjectMOC
        {
            public GameObject gameObject;
            public ICombatable combatable;
            public float fovSqr;
            public GameObjectMOC(GameObject gameObject)
            {
                this.gameObject = gameObject;
                this.combatable = gameObject.GetComponent<ICombatable>();
                try
                {
                    fovSqr = combatable.CombatStats.FieldOfView * combatable.CombatStats.FieldOfView;
                }
                catch
                {
                   // Debug.Log(gameObject.name);
                }
                
            }
        }
    }
}