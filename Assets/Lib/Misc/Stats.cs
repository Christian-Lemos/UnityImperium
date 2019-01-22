using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Imperium
{
    public class Stats
    {
        public int MaxHP { get; set; }

        private int hp;
        public int HP
        {
            get
            {
                return hp;
            }
            set
            {
                if(value > MaxHP)
                {
                    hp = MaxHP;
                }
                else
                {
                    hp = value;
                }
            }
        }
        public int MaxShields { get; set; }

        private int shields;
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

