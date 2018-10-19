using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Imperium
{
    public class Stats
    {
        public int MaxHP { get; set; }
        public int HP { get; set; }
        public int MaxShields { get; set; }
        public int Shields { get; set; }
        public int ShieldRegen { get; set; }
        public float FieldOfViewDistance { get; set; }


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
    }
}

