using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Lib.Events
{
    public static class MonoBehaviourEvents
    {
        private static readonly Dictionary<MonoBehaviour, MBEvents> mbs = new Dictionary<MonoBehaviour, MBEvents>();

        public delegate void OnDestroyDelegate();

        /*public static Thread cleaner = new Thread(CleanerExecution); 

        public

        private static void CleanerExecution()
        {
            while(true)
            {
                Thread.Sleep(100);
            }
            
        }*/

        public static void CallDestroyedObservers(this MonoBehaviour monoBehaviour)
        {
            lock (mbs)
            {
                if (mbs.ContainsKey(monoBehaviour))
                {
                    MBEvents mBEvents = mbs[monoBehaviour];
                    foreach (OnDestroyDelegate onDestroy in mBEvents.OnDestroys)
                    {
                        onDestroy.Invoke();
                    }

                    mBEvents.Clear();

                    mbs.Remove(monoBehaviour);
                }
            }

        }

        public static void OnDestroyEvent(this MonoBehaviour target, OnDestroyDelegate @delegate)
        {
            lock (mbs)
            {
                if (!mbs.ContainsKey(target))
                {
                    mbs.Add(target, new MBEvents(target));
                }

                mbs[target].OnDestroys.Add(@delegate);
            }
        }
        public static Cleaner cleaner = new Cleaner();
        public class Cleaner
        {
            private readonly object _lock = new object();
            private bool _start = false;
            private Thread thread;
            private bool Start{
                get
                {
                    lock(_lock)
                    {
                        return _start;
                    }
                }
            }
            public void StartThread()
            {
                lock(_lock)
                {
                    if(_start)
                    {
                        return;
                    }

                    if(this.thread == null)
                    {
                        this.thread = new Thread(this.CleanerExecution);
                        this._start = true;
                        this.thread.Start();
                    }
                    else if(this.thread.IsAlive)
                    {
                        _start = true;   
                    }                    
                }
            }

            public void StopThread()
            {
                lock(_lock)
                {
                    _start = false;
                }
            }

            private void CleanerExecution()
            {
                while(Start)
                {
                    lock(mbs)
                    {
                        HashSet<MonoBehaviour> removed = new HashSet<MonoBehaviour>();
                        foreach(KeyValuePair<MonoBehaviour, MBEvents> keyValuePair in mbs)
                        {
                            if(keyValuePair.Key == null)
                            {
                                Debug.Log("Caling Destroyed");
                                removed.Add(keyValuePair.Key);
                                /*foreach (OnDestroyDelegate onDestroy in keyValuePair.Value.OnDestroys)
                                {
                                    onDestroy.Invoke();
                                }*/

                                keyValuePair.Value.Clear();
                            }
                        }
                        foreach(MonoBehaviour key in removed)
                        {
                            mbs.Remove(key);
                        }
                    }
                    Thread.Sleep(5000);
                }
            }


        }


        private class MBEvents
        {
            private List<OnDestroyDelegate> onDestroys = new List<OnDestroyDelegate>();
            public string name;
            public MBEvents(MonoBehaviour monoBehaviour)
            {
                this.name = monoBehaviour.name;
            }

            public List<OnDestroyDelegate> OnDestroys { get => onDestroys; set => onDestroys = value; }
            public void Clear()
            {
                this.OnDestroys.Clear();
            }
        }
    }
}

