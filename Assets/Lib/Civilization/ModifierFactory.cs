using Imperium.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Lib.Civilization
{
    public class ModifierFactory : Singleton<ModifierFactory>
    {

        private Dictionary<ModifierType, Type> keyValuePairs = new Dictionary<ModifierType, Type>() 
        {
            {ModifierType.ShipHPRegen, typeof(ShipHPRegen)},
            {ModifierType.ShipMaxHPBuffer, typeof(ShipHPModifier)}
        };


        private ModifierFactory()
        {

        }

        public Type GetModifierType(ModifierType modifierType)
        {
            if(keyValuePairs.ContainsKey(modifierType))
            {
                return keyValuePairs[modifierType];
            }
            return null;
        }

        public Modifier AddModifierToGameObject(GameObject gameObject, ModifierType modifierType, int level, bool enabled)
        {
            Type type = GetModifierType(modifierType);
            Modifier existing = (Modifier) gameObject.GetComponent(type);

            if (existing == null)
            {
                Modifier modifier = (Modifier) gameObject.AddComponent(type);
                modifier.Level = level;
                modifier.enabled = enabled;
                return modifier;
            }
            existing.Level = level;
            existing.enabled = enabled;
            return existing;
        }
    }
}
