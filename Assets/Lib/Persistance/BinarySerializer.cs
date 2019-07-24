using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Lib.Persistance
{
    public static class BinarySerializer
    {
        private static readonly BinaryFormatter binaryFormatter = new BinaryFormatter();
        public static byte[] BinarySerialization(this object @object)
        {
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, @object);
            memoryStream.Close();
            return memoryStream.ToArray();
        }

        public static object BinaryDeserialization<T>(this byte[] byteArray)
        {
            MemoryStream memoryStream = new MemoryStream(byteArray);
            T @object = (T)binaryFormatter.Deserialize(memoryStream);
            memoryStream.Close();
            return @object;
        }
    }
}
