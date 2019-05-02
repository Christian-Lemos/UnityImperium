using UnityEngine;

namespace Imperium.AI
{
    public abstract class UnitLevelAI : MonoBehaviour
    {
        public Player player;
        public StrategicAI strategicAI;

        public abstract void Execute();

        protected void Start()
        {
            player = PlayerDatabase.Instance.GetObjectPlayer(gameObject);
            strategicAI = StrategicAI.playerStrategicAI[player];
        }

        private void Update()
        {
            Execute();
        }
    }
}