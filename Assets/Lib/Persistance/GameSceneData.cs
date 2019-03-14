using Imperium.Economy;
using Imperium.MapObjects;
using Imperium.Persistence.MapObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Imperium.Persistence
{
    [System.Serializable]
    public class GameSceneData
    {
        public List<AsteroidFieldControllerPersistance> asteroidFields;
        public List<BulletControllerPersistance> bulletControllerPersistances;
        public Vector2 MapSize;
        public string Name;
        public List<PlayerPersistance> players;
        public ShipConstructionManagerPersistance shipConstructionManagerPersistance;

        public GameSceneData(string name, Vector2 mapSize, List<PlayerPersistance> players, List<AsteroidFieldControllerPersistance> asteroidFields)
        {
            Name = name;
            MapSize = mapSize;
            this.asteroidFields = asteroidFields;
            this.players = players;
        }

        public GameSceneData(string name, Vector2 mapSize, List<PlayerPersistance> players, List<AsteroidFieldControllerPersistance> asteroidFields, List<BulletControllerPersistance> bulletControllerPersistances)
        {
            Name = name;
            MapSize = mapSize;
            this.asteroidFields = asteroidFields;
            this.players = players;
            this.bulletControllerPersistances = bulletControllerPersistances;
        }

        public static GameSceneData NewGameDefault()
        {
            ///////////////////////////////////Player 1///////////////////////////////////////////////////////
            List<ResourcePersistance> player1Resources = new List<ResourcePersistance>
            {
                new ResourcePersistance(ResourceType.Metal, 500),
                new ResourcePersistance(ResourceType.Crystal, 500),
                new ResourcePersistance(ResourceType.Energy, 100)
            };

            List<ShipControllerPersistance> player1Ships = new List<ShipControllerPersistance>()
            {
                new ShipControllerPersistance(ShipFactory.getInstance().CreateShip(ShipType.MotherShip), ShipType.MotherShip, new MapObjectPersitance(1, new Vector3(-15, 0, -20), new Vector3(1, 1, 1), Quaternion.identity), new Navigation.FleetCommandQueue().Serialize())
            };

            //////////////////////////////////////Player 2//////////////////////////////////////////////////////
            List<ResourcePersistance> player2Resources = new List<ResourcePersistance>
            {
                new ResourcePersistance(ResourceType.Metal, 500),
                new ResourcePersistance(ResourceType.Crystal, 500),
                new ResourcePersistance(ResourceType.Energy, 100)
            };

            List<ShipControllerPersistance> player2Ships = new List<ShipControllerPersistance>()
            {
                new ShipControllerPersistance(ShipFactory.getInstance().CreateShip(ShipType.MotherShip), ShipType.MotherShip, new MapObjectPersitance(2, new Vector3(15, 0, 20), new Vector3(1, 1, 1), Quaternion.identity), new Navigation.FleetCommandQueue().Serialize())
            };

            ///////////////////////////////////////////END///////////////////////////////////////////////////////

            List<PlayerPersistance> players = new List<PlayerPersistance>()
            {
                new PlayerPersistance(0, PlayerType.Real, player1Resources, player1Ships, new List<StationControllerPersistance>()),
                new PlayerPersistance(1, PlayerType.AI, player2Resources, player2Ships, new List<StationControllerPersistance>())
            };

            AsteroidFieldAsteroidSettings asteroidFieldAsteroidSettings = AsteroidFieldAsteroidSettings.CreateDefaultSettings();

            AsteroidFieldControllerPersistance middleAsteroidField = new AsteroidFieldControllerPersistance(asteroidFieldAsteroidSettings.Serialize(), new List<AsteroidControllerPersistance>(), false, new MapObjectPersitance(3, new Vector3(0, 0, 0), new Vector3(1, 1, 1), Quaternion.identity), new Vector3(15, 3, 15));

            return new GameSceneData("New Game", new Vector2(40, 50), players, new List<AsteroidFieldControllerPersistance>() { middleAsteroidField });
        }
    }
}