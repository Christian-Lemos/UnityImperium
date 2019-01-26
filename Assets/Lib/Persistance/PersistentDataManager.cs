using System.IO;
using UnityEngine;

namespace Imperium.Persistence
{
    public class PersistantDataManager
    {
        private static PersistantDataManager instance = null;
        private readonly string gameDataDirectory;
        private readonly string gameDataPath = "/savegames/";

        private PersistantDataManager()
        {
            gameDataDirectory = Application.persistentDataPath + gameDataPath;
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
    }
}