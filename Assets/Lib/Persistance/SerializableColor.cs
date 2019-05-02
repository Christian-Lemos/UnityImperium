using System;
using UnityEngine;

namespace Imperium.Persistance
{
    [Serializable]
    public class SerializableColor
    {
        [SerializeField]
        private float a;

        [SerializeField]
        private float b;

        private Color color;

        [SerializeField]
        private float g;

        [SerializeField]
        private float r;

        public SerializableColor(Color color)
        {
            R = color.r;
            G = color.g;
            B = color.b;
            A = color.a;
        }

        public SerializableColor(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
            Color = new Color(R, G, B, A);
        }

        public float A { get => a; private set => a = value; }
        public float B { get => b; private set => b = value; }
        public Color Color { get => color; private set => color = value; }
        public float G { get => g; private set => g = value; }
        public float R { get => r; private set => r = value; }
    }
}