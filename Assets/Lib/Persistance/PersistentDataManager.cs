using System.IO;
using UnityEngine;

namespace Imperium.Persistence
{
    public class PersistantDataManager
    {
        private static readonly string gameDataPath = "savegames";
        public static readonly string gameDataDirectory = Application.persistentDataPath + "/" + gameDataPath + "/";
        private static PersistantDataManager instance = null;

        private PersistantDataManager()
        {
            if (!Directory.Exists(Application.persistentDataPath + gameDataPath))
            {
                Directory.CreateDirectory(gameDataDirectory);
            }
        }

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

        public void CreateGameSceneData(GameSceneData data)
        {
            string dataAsJson = JsonUtility.ToJson(data);
            string filePath = gameDataDirectory + data.Name + ".json";
            File.WriteAllText(filePath, dataAsJson);
        }

        public GameSceneData GetGameData(string name)
        {
            string filePath = gameDataDirectory + name + ".json";
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                return JsonUtility.FromJson<GameSceneData>(dataAsJson);
            }
            else
            {
                throw new System.Exception("savegame not found!");
            }
        }

        public void SaveGame(GameSceneData gameSceneData)
        {
            string dataAsJson = JsonUtility.ToJson(gameSceneData);
            string filePath = gameDataDirectory + gameSceneData.Name + ".json";
            File.WriteAllText(filePath, dataAsJson);
        }
    }
}