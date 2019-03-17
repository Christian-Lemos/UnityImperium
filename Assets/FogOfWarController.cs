using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarController : MonoBehaviour
{
    private Color[] meshColors;
    public int[] players;

    private IEnumerator enumerator;

    [SerializeField]
    private GameObject fogOfWarPlane;

    private List<FogOfWarUtility> fogOfWarUtilities = new List<FogOfWarUtility>();
    private Mesh mesh;

    private enum FogOfWarState
    {
        Unexplored, Explored, Visible
    }

    private HashSet<GameObject> GetObjects()
    {
        HashSet<GameObject> gameObjects = new HashSet<GameObject>();
        for (int i = 0; i < players.Length; i++)
        {
            HashSet<GameObject> playerGOs = PlayerDatabase.Instance.GetObjects(players[i]);
            foreach (GameObject @object in playerGOs)
            {
                //if (@object.GetComponent<MapObjectCombatter>() != null)
                //{
                gameObjects.Add(@object);
                // }
            }
        }

        return gameObjects;
    }

    private void Initialize()
    {
        mesh = fogOfWarPlane.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        meshColors = new Color[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            FogOfWarUtility fogOfWarUtility = new FogOfWarUtility(fogOfWarPlane, vertices[i], i, FogOfWarState.Unexplored);
            fogOfWarUtilities.Add(fogOfWarUtility);
            meshColors[i] = Color.black;
        }
    }

    private void Start()
    {
        Initialize();
        enumerator = Updater();
        StartCoroutine(enumerator);
    }

    private void UpdateFogOfWar()
    {
        HashSet<GameObject> gameObjects = GetObjects();

        foreach (GameObject @object in gameObjects)
        {
            float fogRadius;
            try
            {
                fogRadius = @object.GetComponent<MapObjectCombatter>().combatStats.fieldOfViewDistance;
            }
            catch
            {
                continue;
            }

            Vector3 fogPoint = new Vector3(@object.transform.position.x, fogOfWarPlane.transform.position.y, @object.transform.position.z);
            for (int i = 0; i < fogOfWarUtilities.Count; i++)
            {
                FogOfWarUtility fowu = fogOfWarUtilities[i];
                if (fowu.higherState != FogOfWarState.Visible)
                {
                    float dist = (fowu.verticeInWorldSpace - fogPoint).sqrMagnitude;
                    bool inRange = dist < fogRadius * fogRadius;

                    switch (fowu.fogOfWarState)
                    {
                        case FogOfWarState.Unexplored:
                            fowu.fogOfWarState = inRange ? FogOfWarState.Visible : FogOfWarState.Unexplored;
                            break;

                        case FogOfWarState.Visible:
                            fowu.fogOfWarState = inRange ? FogOfWarState.Visible : FogOfWarState.Explored;
                            break;

                        case FogOfWarState.Explored:
                            fowu.fogOfWarState = inRange ? FogOfWarState.Visible : FogOfWarState.Explored;
                            break;
                    }

                    if ((int)fowu.fogOfWarState > (int)fowu.higherState)
                    {
                        fowu.higherState = fowu.fogOfWarState;
                    }
                }
            }
        }

        for (int i = 0; i < fogOfWarUtilities.Count; i++)
        {
            FogOfWarUtility fowu = fogOfWarUtilities[i];
            switch (fowu.higherState)
            {
                case FogOfWarState.Unexplored:
                    meshColors[fowu.colorIndex].a = 0.6f;
                    break;

                case FogOfWarState.Explored:
                    meshColors[fowu.colorIndex].a = 0.3f;
                    break;

                case FogOfWarState.Visible:
                    meshColors[fowu.colorIndex].a = 0f;
                    break;
            }
            fowu.higherState = FogOfWarState.Unexplored;
        }

        mesh.colors = meshColors;
    }

    private IEnumerator Updater()
    {
        while (true)
        {
            UpdateFogOfWar();
            yield return null;
        }
    }

    private class FogOfWarUtility
    {
        public int colorIndex;
        public FogOfWarState fogOfWarState;
        public FogOfWarState higherState;
        public Vector3 vertice;
        public Vector3 verticeInWorldSpace;

        public FogOfWarUtility(GameObject fogOfWarPlane, Vector3 vertice, int colorIndex, FogOfWarState fogOfWarState)
        {
            this.vertice = vertice;
            this.colorIndex = colorIndex;
            this.fogOfWarState = fogOfWarState;
            verticeInWorldSpace = fogOfWarPlane.transform.TransformPoint(vertice);
            higherState = fogOfWarState;
        }
    }
}