using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Lib.Civilization
{
    public static class ModifierHelper
    {
        public static HashSet<Modifier> GetModifiers(this GameObject gameObject)
        {
            Modifier[] modifiers = gameObject.GetComponents<Modifier>();
            return new HashSet<Modifier>(modifiers);
        }


        public static Modifier AddModifier(this GameObject gameObject, Type type, int level, bool enabled)
        {
            Debug.Log("Adding: " + type);
            Modifier existingModifier = (Modifier) gameObject.GetComponent(type);
            if (existingModifier == null)
            {
                Modifier modifier = (Modifier)gameObject.AddComponent(type);
                modifier.Level = level;
                modifier.enabled = enabled;
                return modifier;
            }
            else
            {
                if (existingModifier.DoesStack)
                {
                    Modifier sameLevel = gameObject.GetModifierWithLevel(type, level);
                    if(sameLevel == null)
                    {
                        Modifier modifier = (Modifier)gameObject.AddComponent(type);
                        modifier.Level = level;
                        modifier.enabled = enabled;
                        return modifier;
                    }
                    return null;
                }
                else
                {

                    Modifier highest = gameObject.GetModifierWithHighestLevel(type);
                    if (highest != null && highest.Level < level)
                    {
                        Modifier modifier = (Modifier)gameObject.AddComponent(type);
                        modifier.Level = level;
                        modifier.enabled = enabled;

                        highest.enabled = false;
                        highest.ReverseModify();

                        return modifier;
                    }
                    return null;
                }
            }
        }
        public static Modifier AddModifier<T>(this GameObject gameObject, int level, bool enabled) where T : Modifier
        {
            return AddModifier(gameObject, typeof(T), level, enabled);
        }

        public static HashSet<Modifier> GetAllModifiersOfType<T>(this GameObject gameObject) where T : Modifier
        {
            return gameObject.GetAllModifiersOfType(typeof(T));
        }

        public static HashSet<Modifier> GetAllModifiersOfType(this GameObject gameObject, Type type)
        {
            Component[] components =  gameObject.GetComponents(type);
            HashSet<Modifier> set = new HashSet<Modifier>();
            foreach(Component c in components)
            {
                set.Add((Modifier) c);
            }
            return set;
        }

        public static Modifier GetModifierWithHighestLevel<T>(this GameObject gameObject) where T : Modifier
        {
            return gameObject.GetModifierWithHighestLevel(typeof(T));
        }

        public static Modifier GetModifierWithHighestLevel(this GameObject gameObject, Type type)
        {
            HashSet<Modifier> modifiers = gameObject.GetAllModifiersOfType(type);
            Modifier modifier = null;
            foreach (Modifier m in modifiers)
            {
                if (modifier == null || modifier.Level < m.Level)
                {
                    modifier = m;
                }
            }
            return modifier;
        }

        public static bool RemoveModifier(this GameObject gameObject, Modifier modifier)
        { 
            if(modifier.Guardians.Count == 0)
            {
                modifier.ReverseModify();
                UnityEngine.Object.Destroy(modifier);

                Modifier highest = gameObject.GetModifierWithHighestLevel(modifier.GetType());
                if(highest != null)
                {
                    highest.active = true;
                    highest.enabled = true;
                    highest.Modify();
                }
                return true;
            }
            return false;
        }


        public static Modifier GetModifierWithLevel(this GameObject go, Type type, int level)
        {
            HashSet<Modifier> modifiers = go.GetAllModifiersOfType(type);
            foreach(Modifier modifier in modifiers)
            {
                if(modifier.Level == level)
                {
                    return modifier;
                }
            }
            return null;
        }
    }
}
