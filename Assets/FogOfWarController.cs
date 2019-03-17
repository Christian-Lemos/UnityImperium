using System.Collections.Generic;
using UnityEngine;
using System;
public class FogOfWarController : MonoBehaviour
{
    public Color[] meshColors;
    public int[] players;

    [SerializeField]
    private LayerMask fogOfWarLayer;

    [SerializeField]
    private GameObject fogOfWarPlane;

    private List<FogOfWarUtility> fogOfWarUtilities = new List<FogOfWarUtility>();
    private Mesh mesh;
    private Dictionary<FogOfWarUtility, FogOfWarState> states = new Dictionary<FogOfWarUtility, FogOfWarState>();
    

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
                if (@object.GetComponent<MapObjectCombatter>() != null)
                {
                    gameObjects.Add(@object);
                }
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
            FogOfWarUtility fogOfWarUtility =  new FogOfWarUtility(vertices[i], i, FogOfWarState.Unexplored);
            fogOfWarUtilities.Add(fogOfWarUtility);
            states.Add(fogOfWarUtility, FogOfWarState.Unexplored);
            meshColors[i] = Color.black;
        }
    }

    private void Start()
    {
        Initialize();
       
    }

    private void Update()
    {
        List<FogOfWarUtility> keys = new List<FogOfWarUtility>(states.Keys);
        foreach(FogOfWarUtility key in keys)
        {
            states[key] = FogOfWarState.Unexplored;
        }
        UpdateFogOfWar();
    }

    private void UpdateFogOfWar()
    {
        HashSet<GameObject> gameObjects = GetObjects();

        foreach (GameObject @object in gameObjects)
        {
            Vector3 fogPoint = new Vector3(@object.transform.position.x, fogOfWarPlane.transform.position.y, @object.transform.position.z);
            float fogRadius = @object.GetComponent<MapObjectCombatter>().combatStats.fieldOfViewDistance;

            foreach (FogOfWarUtility fowu in fogOfWarUtilities)
            {
                FogOfWarState higherState = states[fowu];

                if(higherState != FogOfWarState.Visible)
                {
                    Vector3 v = fogOfWarPlane.transform.TransformPoint(fowu.vertice);

                    float dist = (v - fogPoint).sqrMagnitude;

                    bool inRange = dist < fogRadius * fogRadius;
                
                    FogOfWarState fogOfWarState = FogOfWarState.Unexplored;

                    switch (fowu.fogOfWarState)
                    {
                        case FogOfWarState.Unexplored:
                            fogOfWarState = inRange ? FogOfWarState.Visible : FogOfWarState.Unexplored;

                            break;

                        case FogOfWarState.Visible:
                            fogOfWarState = inRange ? FogOfWarState.Visible : FogOfWarState.Explored;
                            break;

                        case FogOfWarState.Explored:
                            fogOfWarState = inRange ? FogOfWarState.Visible : FogOfWarState.Explored;
                            break;
                    }

                    if((int) fogOfWarState > (int) higherState)
                    {
                        states[fowu] = fogOfWarState;
                    }
                }
            }
        }

        foreach(KeyValuePair<FogOfWarUtility, FogOfWarState> keyValuePair in states)
        {
            keyValuePair.Key.fogOfWarState = keyValuePair.Value;

            switch(keyValuePair.Value)
            {
                case FogOfWarState.Unexplored:
                    meshColors[keyValuePair.Key.colorIndex].a = 0.6f;
                    break;
                case FogOfWarState.Explored:
                    meshColors[keyValuePair.Key.colorIndex].a = 0.3f;
                    break;
                case FogOfWarState.Visible:
                    meshColors[keyValuePair.Key.colorIndex].a = 0f;
                    break;
            }
        }
        
        
        mesh.colors = meshColors;
    }


    private class FogOfWarUtility
    {
        public int colorIndex;
        public FogOfWarState fogOfWarState;
        public Vector3 vertice;

        public FogOfWarUtility(Vector3 vertice, int colorIndex, FogOfWarState fogOfWarState)
        {
            this.vertice = vertice;
            this.colorIndex = colorIndex;
            this.fogOfWarState = fogOfWarState;
        }
    }

 
}