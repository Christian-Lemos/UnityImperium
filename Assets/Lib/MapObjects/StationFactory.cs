using Imperium.Combat;
using Imperium.MapObjects;
using Imperium.Misc;

namespace Imperium
{
    public class StationFactory : Singleton<StationFactory>
    {
        private StationFactory()
        {
        }

        public Station CreateStation(StationType type)
        {
            CombatStats stats;

            string stationName;

            switch (type)
            {
                case (StationType.TestStation):
                    stats = new CombatStats(500, 1, 500, 1, 5, 10f);
                    stationName = "Test Station";
                    break;

                default:
                    throw new System.Exception("Type of station not supported");
            }

            return new Station(stationName, stats, "station_icon");
        }
    }
}