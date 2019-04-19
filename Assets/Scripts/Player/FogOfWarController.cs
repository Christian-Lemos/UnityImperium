using Imperium.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarController : MonoBehaviour
{

    public static FogOfWarController Instance;

    public int[] playersVision;
    private IEnumerator enumerator;

    public GameObject fogOfWarPlane;

    private List<FogOfWarMeshVertice> fogOfWarUtilities = new List<FogOfWarMeshVertice>();
    private Mesh mesh;
    private Color[] meshColors;



    public Dictionary<FogOfWarState, HashSet<GameObject>> GetObjectsFOWState(bool includeSources, params int[] players)
    {
        Dictionary<FogOfWarState, HashSet<GameObject>> objectFOWStates = new Dictionary<FogOfWarState, HashSet<GameObject>>()
        {
            {FogOfWarState.Unexplored, new HashSet<GameObject>()},
            {FogOfWarState.Explored, new HashSet<GameObject>()},
            {FogOfWarState.Visible, new HashSet<GameObject>()}
        };

        
        HashSet<MapObject> mapObjects = MapObject.GetMapObjects();
        List<FogOfWarMeshVertice> fogOfWarUtilities = GetFogOfWarUtilities(players);

        foreach(MapObject mapObject in mapObjects)
        {
            objectFOWStates[GetObjectFOWState(mapObject.gameObject, fogOfWarUtilities)].Add(mapObject.gameObject);
        }

        if(includeSources)
        {
            HashSet<GameObject> playerObjects = GetFOWSourceObjects(players);
            foreach(GameObject gameObject in playerObjects)
            {
                objectFOWStates[FogOfWarState.Visible].Add(gameObject);
            }
        }

        return objectFOWStates;
    }
    
    public FogOfWarState GetObjectFOWState(GameObject @object, params int[] players)
    {
        List<FogOfWarMeshVertice> fogOfWarUtilities = GetFogOfWarUtilities(players);
        
        return GetObjectFOWState(@object, fogOfWarUtilities);
    }

    public FogOfWarState GetObjectFOWState(GameObject @object, List<FogOfWarMeshVertice> fogOfWarUtilities)
    {
        Vector3 fogPoint = new Vector3(@object.transform.position.x, fogOfWarPlane.transform.position.y, @object.transform.position.z);

        FogOfWarMeshVertice objectFOWU = null;

        float lowestMag = 99999999f;
        for(int i = 0; i < fogOfWarUtilities.Count; i++)
        {
            float dist = (fogOfWarUtilities[i].verticeInWorldSpace - fogPoint).sqrMagnitude;
            if(dist < lowestMag)
            {
                lowestMag = dist;
                objectFOWU = fogOfWarUtilities[i];
            }
        }

        if(objectFOWU.fogOfWarState == FogOfWarState.Explored && @object.GetComponent<INonExplorable>() != null)
        {
            return FogOfWarState.Unexplored;
        }

        return objectFOWU.fogOfWarState;
    }


    public HashSet<GameObject> GetFOWSourceObjects(params int[] players)
    {
        HashSet<GameObject> gameObjects = new HashSet<GameObject>();
        for (int i = 0; i < players.Length; i++)
        {
            HashSet<GameObject> playerGOs = PlayerDatabase.Instance.GetObjects(players[i]);
            foreach (GameObject @object in playerGOs)
            {
                gameObjects.Add(@object);
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
            FogOfWarMeshVertice fogOfWarUtility = new FogOfWarMeshVertice(fogOfWarPlane, vertices[i], i, FogOfWarState.Unexplored);
            fogOfWarUtilities.Add(fogOfWarUtility);
            meshColors[i] = Color.black;
        }
    }

    private void Start()
    {
        Initialize();
        enumerator = Updater();
        StartCoroutine(enumerator);
        Instance = this;
    }

    public List<FogOfWarMeshVertice> GetFogOfWarUtilities(params int[] players)
    {
        Vector3[] vertices = mesh.vertices;
        List<FogOfWarMeshVertice> fogOfWarUtilities = new List<FogOfWarMeshVertice>(vertices.Length);

        for (int i = 0; i < vertices.Length; i++)
        {
            FogOfWarMeshVertice fogOfWarUtility = new FogOfWarMeshVertice(fogOfWarPlane, vertices[i], i, FogOfWarState.Unexplored);
            fogOfWarUtilities.Add(fogOfWarUtility);
        }

        HashSet<GameObject> gameObjects = GetFOWSourceObjects(playersVision);

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
                FogOfWarMeshVertice fowu = fogOfWarUtilities[i];
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
        return fogOfWarUtilities;
    }

    private void UpdateFogOfWar()
    {
        HashSet<GameObject> gameObjects = GetFOWSourceObjects(playersVision);

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
                FogOfWarMeshVertice fowu = fogOfWarUtilities[i];
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
            FogOfWarMeshVertice fowu = fogOfWarUtilities[i];
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
}