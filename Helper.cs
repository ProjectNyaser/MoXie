using System.Runtime.Serialization;
using System.Xml;

namespace MoXie
{
    internal class Helper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[]? Serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            using MemoryStream memoryStream = new();
            DataContractSerializer ser = new(typeof(object));
            ser.WriteObject(memoryStream, obj);
            byte[] data = memoryStream.ToArray();
            return data;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T? Deserialize<T>(byte[] data)
        {
            if (data == null)
            {
                return default;
            }

            using MemoryStream memoryStream = new(data);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(memoryStream, new XmlDictionaryReaderQuotas());
            DataContractSerializer ser = new(typeof(T));
            T? result = (T?)ser.ReadObject(reader, true);
            return result;
        }
    }
}
