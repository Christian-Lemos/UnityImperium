using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Imperium.Persistence
{
    public class PersistantDataManager
    {
        private readonly string gameDataPath = "/savegames/";

        private readonly string gameDataDirectory;
        private static PersistantDataManager instance = null;

        public static PersistantDataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PersistantDataManager();
                }
                return instance;
            }
        }


        private PersistantDataManager()
        {
            this.gameDataDirectory = Application.persistentDataPath + gameDataPath;
            if (!Directory.Exists(Application.persistentDataPath + this.gameDataPath))
            {
                Directory.CreateDirectory(this.gameDataDirectory);
            }
        }

        public void CreateGameSceneData(GameSceneData data)
        {
            string dataAsJson = JsonUtility.ToJson(data);
            string filePath = this.gameDataDirectory + data.Name + ".json";
            File.WriteAllText(filePath, dataAsJson);
        }

        public GameSceneData GetGameData(string name)
        {
            string filePath = this.gameDataDirectory + name + ".json";
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                return  JsonUtility.FromJson<GameSceneData>(dataAsJson);
            }
            else
            {
                throw new System.Exception("savegame not found!");
            }
        }


    }
}

