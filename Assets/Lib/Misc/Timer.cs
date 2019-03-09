using System;
using UnityEngine;
namespace Imperium.Misc
{
    [Serializable]
    public class Timer
    {
        public Action action;
        public float duration;
        public float remainingDuration;
        public bool timerSet = false;

        public Timer(float duration, bool timerSet, Action action)
        {
            this.duration = duration;
            remainingDuration = duration;
            this.timerSet = timerSet;
            this.action = action;
        }

        public void Execute()
        {
            if(timerSet)
            {
                remainingDuration -= Time.deltaTime;

                if(IsFinished)
                {
                    action.Invoke();
                }
            }
        }
        public void ResetTimer()
        {
            remainingDuration = duration;
        }

        public bool IsFinished
        {
            get
            {
                return remainingDuration <= 0;
            }
        }
    }
}