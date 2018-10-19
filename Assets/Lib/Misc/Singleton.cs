using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Imperium.Misc
{
    public abstract class Singleton<T> where T : class
    {
        private static T INSTANCE = null;
        private static readonly object INSTANCE_LOCK = new object();

        public static T getInstance()
        {
            lock (INSTANCE_LOCK)
            {
                if (INSTANCE == null)
                {
                    INSTANCE = (T)Activator.CreateInstance(typeof(T), true);
                }
                return INSTANCE;
            }
        }
    }
}

