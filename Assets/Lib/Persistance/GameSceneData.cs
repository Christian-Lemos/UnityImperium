using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Imperium.Persistence
{
    [System.Serializable]
    public class GameSceneData
    {
        public string Name;
        public int PlayerCount;

        public GameSceneData(string name, int playerCount)
        {
            Name = name;
            PlayerCount = playerCount;
        }

    }
}

