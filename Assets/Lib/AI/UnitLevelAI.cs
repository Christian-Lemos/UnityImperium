using UnityEngine;

namespace Imperium.AI
{
    public abstract class UnitLevelAI : MonoBehaviour
    {
        public int player = 1;
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