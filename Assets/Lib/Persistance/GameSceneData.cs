using Imperium.Enum;
using Imperium.MapObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Imperium.Persistence
{
    [System.Serializable]
    public class GameSceneData
    {
        public List<AsteroidFieldPersistance> asteroidFields;
        public Vector2 MapSize;
        public string Name;
        public List<PlayerPersistance> players;
        public GameSceneData(string name, Vector2 mapSize, List<PlayerPersistance> players, List<AsteroidFieldPersistance> asteroidFields)
        {
            Name = name;
            MapSize = mapSize;
            this.asteroidFields = asteroidFields;
            this.players = players;
        }

        public static GameSceneData NewGameDefault()
        {
            ///////////////////////////////////Player 1///////////////////////////////////////////////////////
            List<ResourcePersistance> player1Resources = new List<ResourcePersistance>
            {
                new ResourcePersistance( ResourceType.Metal, 500),
                new ResourcePersistance(ResourceType.Crystal, 500),
                new ResourcePersistance(ResourceType.Energy, 100)
            };

            List<ShipPersistence> player1Ships = new List<ShipPersistence>()
            {
                new ShipPersistence(new Vector3(-15, 0, -20), new Vector3(), new Vector3(1, 1, 1), ShipType.MotherShip)
            };

            //////////////////////////////////////Player 2//////////////////////////////////////////////////////
            List<ResourcePersistance> player2Resources = new List<ResourcePersistance>
            {
                new ResourcePersistance( ResourceType.Metal, 500),
                new ResourcePersistance(ResourceType.Crystal, 500),
                new ResourcePersistance(ResourceType.Energy, 100)
            };

            List<ShipPersistence> player2Ships = new List<ShipPersistence>()
            {
                new ShipPersistence(new Vector3(15, 0, 20), new Vector3(), new Vector3(1, 1, 1), ShipType.MotherShip)
            };

            ///////////////////////////////////////////END///////////////////////////////////////////////////////

            List<PlayerPersistance> players = new List<PlayerPersistance>()
            {
                new PlayerPersistance(PlayerType.Real, 0, player1Ships, player1Resources),
                new PlayerPersistance(PlayerType.AI, 1, player2Ships, player2Resources)
            };

            AsteroidFieldAsteroidSettings asteroidFieldAsteroidSettings = AsteroidFieldAsteroidSettings.CreateDefaultSettings();

            AsteroidFieldPersistance middleAsteroidField = new AsteroidFieldPersistance(asteroidFieldAsteroidSettings.Serialize(), new List<AsteroidPersistance>(), new Vector3(0, 0, 0), new Vector3(15, 3, 15), false);

            return new GameSceneData("New Game", new Vector2(40, 50), players, new List<AsteroidFieldPersistance>() { middleAsteroidField });
        }
    }
}