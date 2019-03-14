namespace Imperium.Combat
{
    [System.Serializable]
    public class CombatStats
    {
        //Use proprieties.This is public due serialization simplification
        public int hp;

        public int maxHP;

        public int maxShields;
        public int shieldRegen;
        public int shields;

        public CombatStats(int maxHP, int maxShields, int shieldRegen, float fieldOfViewDistance)
        {
            this.maxHP = maxHP;
            this.maxShields = maxShields;
            this.shieldRegen = shieldRegen;
            HP = maxHP;
            Shields = this.maxShields;
            FieldOfViewDistance = fieldOfViewDistance;
        }

        public CombatStats(int maxHP, int hP, int maxShields, int shields, int shieldRegen, float fieldOfViewDistanc)
        {
            this.maxHP = maxHP;
            this.maxShields = maxShields;
            this.shieldRegen = shieldRegen;
            Shields = shields;
            this.shieldRegen = shieldRegen;
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
                if (value > maxHP)
                {
                    hp = maxHP;
                }
                else
                {
                    hp = value;
                }
            }
        }

        public int Shields
        {
            get
            {
                return shields;
            }
            set
            {
                if (value > maxShields)
                {
                    shields = maxShields;
                }
                else
                {
                    shields = value;
                }
            }
        }
    }
}