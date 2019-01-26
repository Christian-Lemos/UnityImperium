namespace Imperium
{
    public class Stats
    {
        private int hp;
        private int shields;

        public Stats(int maxHP, int maxShields, int shieldRegen, float fieldOfViewDistance)
        {
            MaxHP = maxHP;
            MaxShields = maxShields;
            ShieldRegen = shieldRegen;
            HP = maxHP;
            Shields = MaxShields;
            FieldOfViewDistance = fieldOfViewDistance;
        }

        public Stats(int maxHP, int hP, int maxShields, int shields, int shieldRegen, float fieldOfViewDistanc)
        {
            MaxHP = maxHP;
            MaxShields = maxShields;
            ShieldRegen = shieldRegen;
            Shields = shields;
            ShieldRegen = shieldRegen;
            FieldOfViewDistance = fieldOfViewDistanc;
        }

        public float FieldOfViewDistance { get; set; }

        public int HP
        {
            get
            {
                return hp;
            }
            set
            {
                if (value > MaxHP)
                {
                    hp = MaxHP;
                }
                else
                {
                    hp = value;
                }
            }
        }

        public int MaxHP { get; set; }
        public int MaxShields { get; set; }
        public int ShieldRegen { get; set; }

        public int Shields
        {
            get
            {
                return shields;
            }
            set
            {
                if (value > MaxShields)
                {
                    shields = MaxShields;
                }
                else
                {
                    shields = value;
                }
            }
        }
    }
}