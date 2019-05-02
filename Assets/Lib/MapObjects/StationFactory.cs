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
            string stationIcon;
            switch (type)
            {
                case (StationType.TestStation):
                    stats = new CombatStats(500, 1, 500, 1, 5, 20f);
                    stationName = "Test Station";
                    stationIcon = "station_icon";
                    break;
                case (StationType.MiningStation):
                    stats = new CombatStats(700, 1, 300, 5, 2, 10f);
                    stationName = "Mining Outpost";
                    stationIcon = "mining_outpost";
                    break;

                default:
                    throw new System.Exception("Type of station not supported");
            }

            return new Station(stationName, stats, stationIcon);
        }
    }
}