using Imperium.Persistance;
using System;
using UnityEngine;

namespace Imperium
{
    [Serializable]
    public class Player
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private int number;

        [SerializeField]
        private PlayerType playerType;

        [SerializeField]
        private SerializableColor serializableColor;

        public Player(string name, int number, PlayerType playerType, SerializableColor serializableColor)
        {
            Name = name;
            Number = number;
            PlayerType = playerType;
            SerializableColor = serializableColor;
        }

        public string Name { get => name; set => name = value; }
        public int Number { get => number; set => number = value; }
        public PlayerType PlayerType { get => playerType; private set => playerType = value; }
        public SerializableColor SerializableColor { get => serializableColor; set => serializableColor = value; }
    }
}