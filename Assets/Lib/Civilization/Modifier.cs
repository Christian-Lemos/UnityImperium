using Assets.Lib.Persistance;
using Imperium.Persistence;
using UnityEngine;
using Assets.Lib.Events;

namespace Assets.Lib.Civilization
{
    public abstract class Modifier : MonoBehaviour, Imperium.Persistence.ISerializable<ModifierPersistence>
    {
        public bool active;
        public ModifierType modifierType;

        [SerializeField]
        private bool executeEveryUpdate;

        [SerializeField]
        private int level;

        public abstract string Description { get; }

        public abstract string Icon { get; }
        public bool ExecuteEveryUpdate { get => executeEveryUpdate; protected set => executeEveryUpdate = value; }
        public int Level { get => level; set { if (value != level) { level = value; } } }
        public abstract string Name { get; }

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

        protected void Start()
        {
            if (!active)
            {
                Modify();
            }
        }

        protected void Update()
        {
            if (active && ExecuteEveryUpdate)
            {
                Modify();
            }
        }

        private void OnDestroy()
        {
            ReverseModify();
            this.CallDestroyedObservers();
        }
    }
}