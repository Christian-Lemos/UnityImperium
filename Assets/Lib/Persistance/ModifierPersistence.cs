using Assets.Lib.Civilization;
using System;
using UnityEngine;

namespace Assets.Lib.Persistance
{
    [Serializable]
    public class ModifierPersistence
    {
        [SerializeField]
        private bool active;

        [SerializeField]
        private bool executeEveryUpdate;

        [SerializeField]
        private int level;

        [SerializeField]
        private ModifierType modifierType;

        public ModifierPersistence(bool active, bool executeEveryUpdate, int level, ModifierType modifierType)
        {
            Active = active;
            ExecuteEveryUpdate = executeEveryUpdate;
            Level = level;
            ModifierType = modifierType;
        }

        public bool Active { get => active; set => active = value; }
        public bool ExecuteEveryUpdate { get => executeEveryUpdate; set => executeEveryUpdate = value; }
        public int Level { get => level; set => level = value; }
        public ModifierType ModifierType { get => modifierType; set => modifierType = value; }
    }
}