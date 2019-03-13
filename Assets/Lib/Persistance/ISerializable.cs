namespace Imperium.Persistence
{
    public interface ISerializable<T> where T : class
    {
        T Serialize();

        ISerializable<T> SetObject(T serializedObject);
    }
}