using System;

namespace Imperium.Misc
{
    public abstract class Singleton<T> where T : class
    {
        private static readonly object INSTANCE_LOCK = new object();
        private static T INSTANCE = null;

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