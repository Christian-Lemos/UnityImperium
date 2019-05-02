using Imperium.Economy;
using Imperium.MapObjects;
using Imperium.Persistence.MapObjects;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Persistance;

namespace Imperium.Persistence
{
    [System.Serializable]
    public class GameSceneData
    {
        public List<AsteroidFieldControllerPersistance> asteroidFields = new List<AsteroidFieldControllerPersistance>();
        public List<BulletControllerPersistance> bulletControllerPersistances = new List<BulletControllerPersistance>();
        public Vector2 MapSize;
        public string Name;
        public List<PlayerPersistance> players = new List<PlayerPersistance>();
        public ShipConstructionManagerPersistance shipConstructionManagerPersistance;
        public long nextMapObjectId;

        public GameSceneData(string name, Vector2 mapSize, List<PlayerPersistance> players, List<AsteroidFieldControllerPersistance> asteroidFields, long nextMapObjectId)
        {
            Name = name;
            MapSize = mapSize;
            this.asteroidFields = asteroidFields;
            this.players = players;
            this.nextMapObjectId = nextMapObjectId;
        }

        public GameSceneData(string name, Vector2 mapSize, List<PlayerPersistance> players, List<AsteroidFieldControllerPersistance> asteroidFields, List<BulletControllerPersistance> bulletControllerPersistances, long nextMapObjectId)
        {
            Name = name;
            MapSize = mapSize;
            this.asteroidFields = asteroidFields;
            this.players = players;
            this.bulletControllerPersistances = bulletControllerPersistances;
            this.nextMapObjectId = nextMapObjectId;
        }

        public static GameSceneData NewGameDefault()
        {

            long id = 0;

            #region Player 1
            ///////////////////////////////////Player 1///////////////////////////////////////////////////////
            List<ResourcePersistance> player1Resources = new List<ResourcePersistance>
            {
                new ResourcePersistance(ResourceType.Metal, 500),
                new ResourcePersistance(ResourceType.Crystal, 500),
                new ResourcePersistance(ResourceType.BlackMatter, 100)
            };

            List<ShipControllerPersistance> player1Ships = new List<ShipControllerPersistance>()
            {
                new ShipControllerPersistance(ShipFactory.getInstance().CreateShip(ShipType.MotherShip), ShipType.MotherShip, new MapObjectPersitance(id++, new Vector3(-15, 0, -20), new Vector3(1, 1, 1), Quaternion.identity), new Navigation.FleetCommandQueue().Serialize(), false)
            };

            #endregion

            #region Player 2
            List<ResourcePersistance> player2Resources = new List<ResourcePersistance>
            {
                new ResourcePersistance(ResourceType.Metal, 500),
                new ResourcePersistance(ResourceType.Crystal, 500),
                new ResourcePersistance(ResourceType.BlackMatter, 100)
            };

            List<ShipControllerPersistance> player2Ships = new List<ShipControllerPersistance>()
            {
                new ShipControllerPersistance(ShipFactory.getInstance().CreateShip(ShipType.MotherShip), ShipType.MotherShip, new MapObjectPersitance(id++, new Vector3(15, 0, 20), new Vector3(1, 1, 1), Quaternion.identity), new Navigation.FleetCommandQueue().Serialize(), false)
            };

            #endregion

           

            List<PlayerPersistance> players = new List<PlayerPersistance>()
            {
                new PlayerPersistance(new Player("player 1", 0, PlayerType.Real, new SerializableColor(Color.blue)), player1Resources, player1Ships, new List<StationControllerPersistance>()),
                new PlayerPersistance(new Player("player 2", 1, PlayerType.AI, new SerializableColor(Color.red)), player2Resources, player2Ships, new List<StationControllerPersistance>())
            };

            #region asteroids

            AsteroidFieldAsteroidSettings asteroidFieldAsteroidSettings = AsteroidFieldAsteroidSettings.CreateDefaultSettings();

            AsteroidFieldControllerPersistance middleAsteroidField = new AsteroidFieldControllerPersistance(asteroidFieldAsteroidSettings.Serialize(), new List<AsteroidControllerPersistance>(), false, new MapObjectPersitance(id++, new Vector3(0, 0, 0), new Vector3(1, 1, 1), Quaternion.identity), new Vector3(15, 3, 15));

            #endregion

            return new GameSceneData("New Game", new Vector2(40, 50), players, new List<AsteroidFieldControllerPersistance>() { middleAsteroidField }, id++);
        }
    }
}