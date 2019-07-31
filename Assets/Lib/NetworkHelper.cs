using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGamesHashtable = ExitGames.Client.Photon.Hashtable;

namespace Assets.Lib
{
    public static class NetworkHelper
    {
        public static class Constants
        {
            public const string PLAYER_READY = "playerReady";
            public const string GAME_SCENE_DATA = "playerSceneData";
        }


        public static ExitGamesHashtable ToExitGamesHashtable(this Hashtable hashtable)
        {
            ExitGamesHashtable exitGamesHashtable = new ExitGamesHashtable();
            foreach(KeyValuePair<object, object> keyValuePair in hashtable)
            {
                exitGamesHashtable.Add(keyValuePair.Key, keyValuePair.Value);
            }
            return exitGamesHashtable;
        }

        public static Hashtable ToSystemHashtable(this ExitGamesHashtable exitGamesHashtable)
        {
            Hashtable hashtable = new Hashtable();
            foreach(KeyValuePair<object, object> keyValuePair in exitGamesHashtable.ToArray())
            {
                hashtable.Add(keyValuePair.Key, keyValuePair.Value);
            }
            return hashtable;
        }
    }
}
