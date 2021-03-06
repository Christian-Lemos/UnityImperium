﻿using UnityEngine;

namespace Imperium.Rendering
{
    [System.Serializable]
    public class FogOfWarMeshVertice
    {
        public int colorIndex;
        public FogOfWarState fogOfWarState;
        public FogOfWarState higherState;
        public Vector3 vertice;
        public Vector3 verticeInWorldSpace;

        public FogOfWarMeshVertice(GameObject fogOfWarPlane, Vector3 vertice, int colorIndex, FogOfWarState fogOfWarState)
        {
            this.vertice = vertice;
            this.colorIndex = colorIndex;
            this.fogOfWarState = fogOfWarState;
            verticeInWorldSpace = fogOfWarPlane.transform.TransformPoint(vertice);
            higherState = fogOfWarState;
        }
    }
}