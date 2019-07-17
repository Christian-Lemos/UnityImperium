using Assets.Lib.Persistance;
using Imperium.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Lib.Civilization
{
    public abstract class Modifier : MonoBehaviour, Imperium.Persistence.ISerializable<ModifierPersistence>
    {
        public ModifierType modifierType;

        [SerializeField]
        private int level;

        public bool active;

        [SerializeField]
        private bool executeEveryUpdate;

        public int Level { get => level; set { level = value; Modify(); } }

        public bool ExecuteEveryUpdate { get => executeEveryUpdate; protected set => executeEveryUpdate = value; }

        public abstract void Modify();
        public abstract void ReverseModify();

        ModifierPersistence ISerializable<ModifierPersistence>.Serialize()
        {
            ModifierPersistence modifierPersistence = new ModifierPersistence(this.active, this.executeEveryUpdate, this.Level, this.modifierType);
            return modifierPersistence;
        }

        ISerializable<ModifierPersistence> ISerializable<ModifierPersistence>.SetObject(ModifierPersistence serializedObject)
        {
            this.active = serializedObject.Active;
            this.modifierType = serializedObject.ModifierType;
            this.Level = serializedObject.Level;

            return this;
        }


    }
}
