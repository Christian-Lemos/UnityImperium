using Assets.Lib.Events;
using Assets.Lib.Persistance;
using Imperium.Persistence;
using System.Collections.Generic;
using UnityEngine;

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

        public abstract bool DoesStack { get; }
        public bool ExecuteEveryUpdate { get => executeEveryUpdate; protected set => executeEveryUpdate = value; }

        public HashSet<GameObject> Guardians
        {
            get
            {
                guardians.RemoveWhere((GameObject g) =>
                {
                    return g == null;
                });
                return guardians;
            }
            set => guardians = value;
        }

        public abstract string Icon { get; }
        public int Level { get => level; set { level = value;  } }
        public abstract string Name { get; }

        public abstract void Modify();

        public abstract void ReverseModify();

        #region Serialization

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

        #endregion Serialization

        #region UnityCallbacks

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

        private void OnDisable()
        {
            ReverseModify();
        }

        #endregion UnityCallbacks

        #region ModifierGuardians

        [SerializeField]
        private HashSet<GameObject> guardians = new HashSet<GameObject>();

        public void AddGuardian(GameObject gameObject)
        {
            Guardians.Add(gameObject);
        }

        public void RemoveGuardian(GameObject gameObject)
        {
            Guardians.Remove(gameObject);
        }

        public class ModifierGuardian
        {
            public GameObject guardian;
            public Modifier modifier;

            public ModifierGuardian(GameObject guardian, Modifier modifier)
            {
                this.guardian = guardian;
                this.modifier = modifier;
            }

            public void OnDestroy()
            {
            }
        }

        #endregion ModifierGuardians
    }
}