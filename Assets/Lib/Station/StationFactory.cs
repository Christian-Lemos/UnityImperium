using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Misc;
using Imperium.Enum;
namespace Imperium
{

    public class StationFactory : Singleton<StationFactory>
    {


        private StationFactory()
        {

        }

        public Station CreateStation(StationType type)
        {
            Stats stats;

            string stationName;

            switch (type)
            {
                case (StationType.TestStation):
                    stats = new Stats(500, 1, 500, 1, 5, 10f);
                    stationName = "Test Station";
                    break;
                default:
                    throw new System.Exception("Type of station not supported");
            }

            return new Station(stationName, stats, "station_icon");
        }

    }
}

