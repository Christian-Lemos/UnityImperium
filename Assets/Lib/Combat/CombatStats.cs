using UnityEngine;

namespace Imperium.Combat
{
    [System.Serializable]
    public class CombatStats
    {
        [SerializeField]
        private float m_fieldOfView;

        [SerializeField]
        private int m_hp;

        [SerializeField]
        private int m_maxHP;

        [SerializeField]
        private int m_maxShields;

        [SerializeField]
        private int m_shieldRegen;

        [SerializeField]
        private int m_shields;

        public CombatStats(int maxHP, int maxShields, int shieldRegen, float fieldOfViewDistance)
        {
            this.m_maxHP = maxHP;
            this.m_maxShields = maxShields;
            this.m_shieldRegen = shieldRegen;
            this.m_hp = maxHP;
            this.m_shields = this.MaxShields;
            this.m_fieldOfView = fieldOfViewDistance;
        }

        public CombatStats(int maxHP, int hP, int maxShields, int shields, int shieldRegen, float fieldOfViewDistance)
        {
            this.m_maxHP = maxHP;
            this.m_maxShields = maxShields;
            this.m_shieldRegen = shieldRegen;
            this.m_shields = shields;
            this.m_shieldRegen = shieldRegen;
            this.m_fieldOfView = fieldOfViewDistance;
            m_hp = hP;
        }

        public CombatStats(CombatStats combatStats)
        {
            this.m_maxHP = combatStats.MaxHP;
            this.m_maxShields = combatStats.Shields;

            this.m_hp = combatStats.HP;
            this.m_shields = combatStats.Shields;

            this.m_shieldRegen = combatStats.ShieldRegen;
            this.FieldOfView = combatStats.FieldOfView;
        }

        public delegate void m_observer(CombatStats combatStats, int hp, int maxHP, int shields, int maxShields, int shieldRegen, float fieldOfView);

        private event m_observer ObserversEvent;

        public float FieldOfView
        {
            get => m_fieldOfView; set
            {
                float oldValue = m_fieldOfView;
                if (value < 0)
                {
                    value = 0;
                }
                m_fieldOfView = value;

                CallObservers(m_hp, m_maxHP, m_shields, m_maxShields, m_shieldRegen, m_fieldOfView - oldValue);
            }
        }

        public int HP
        {
            get
            {
                return m_hp;
            }
            set
            {
                int oldHp = m_hp;
                if (value > MaxHP)
                {
                    m_hp = MaxHP;
                }
                else
                {
                    m_hp = value;
                }

                CallObservers(m_hp - oldHp, m_maxHP, m_shields, m_maxShields, m_shieldRegen, m_fieldOfView);
            }
        }

        public int MaxHP
        {
            get => m_maxHP; set
            {
                int oldHp = m_hp;
                int oldMaxHp = m_maxShields;
                if (value < 0)
                {
                    value = 0;
                }

                m_maxHP = value;
                if (m_hp > m_maxHP)
                {
                    m_hp = m_maxHP;
                }
                CallObservers(m_hp - oldHp, m_maxHP - oldMaxHp, m_shields, m_maxShields, m_shieldRegen, m_fieldOfView);
            }
        }

        public int MaxShields
        {
            get => m_maxShields; set
            {
                int oldShields = m_shields;
                int oldMaxShields = m_maxShields;
                if (value < 0)
                {
                    value = 0;
                }

                m_maxShields = value;
                if (m_shields > m_maxShields)
                {
                    m_shields = m_maxShields;
                }
                CallObservers(m_hp, m_maxHP, m_shields - oldShields, m_maxShields - oldMaxShields, m_shieldRegen, m_fieldOfView);
            }
        }

        public int ShieldRegen
        {
            get => m_shieldRegen; set
            {
                float oldValue = m_shieldRegen;
                m_shieldRegen = value;
                CallObservers(m_hp, m_maxHP, m_maxShields, m_maxShields, m_shieldRegen, m_shieldRegen - oldValue);
            }
        }

        public int Shields
        {
            get
            {
                return m_shields;
            }
            set
            {
                int oldShields = m_shields;
                if (value > MaxShields)
                {
                    m_shields = MaxShields;
                }
                else
                {
                    m_shields = value;
                }

                CallObservers(m_hp, m_maxHP, m_shields - oldShields, m_maxShields, m_shieldRegen, m_fieldOfView);
            }
        }

        public void AddObserver(m_observer observer)
        {
            ObserversEvent += observer;
        }

        public void RemoveObserver(m_observer observer)
        {
            ObserversEvent -= observer;
        }

        private void CallObservers(int hp, int maxHP, int shields, int maxShields, int shieldRegen, float fieldOfView)
        {
            ObserversEvent?.Invoke(this, hp, maxHP, shields, maxShields, shieldRegen, fieldOfView);

        }
    }
}