using UnityEngine;
using System.Collections;
namespace Imperium.Persistence
{
    public interface ISerializable<T>
    {
        T Serialize();
        void SetObject(T serializedObject);

    }
}

