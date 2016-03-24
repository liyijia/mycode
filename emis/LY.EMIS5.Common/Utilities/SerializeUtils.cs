using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace LY.EMIS5.Common.Utilities
{
    /// <summary>
    /// 序列化辅助类
    /// </summary>
    public static class SerializeUtils
    {
        #region 二进制序列化

        /// <summary>
        /// 将泛型对象序列化为二进制数组
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>对象的二进制数组</returns>
        public static byte[] BinarySerialize<T>(T obj)
        {
            Contract.Requires(obj != null);

            BinaryFormatter binary = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                binary.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 将二进制数组返序列化为泛型对象
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="objects">二进制数组</param>
        /// <returns>泛型对象</returns>
        public static T BinaryDeserialize<T>(byte[] objects)
        {
            Contract.Requires(objects != null);

            BinaryFormatter binary = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(objects))
            {
                return (T)binary.Deserialize(stream);
            }
        }

        /// <summary>
        /// 将泛型对象序列化为二进制后保存在文件中
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="obj">泛型对象</param>
        /// <param name="filePath">文件路径</param>
        public static void SaveAsBinary<T>(T obj, string filePath)
        {
            Contract.Requires(obj != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(filePath));

            BinaryFormatter binary = new BinaryFormatter();
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                binary.Serialize(stream, obj);
            }
        }

        #endregion

        #region Xml序列化

        /// <summary>
        /// 将泛型对象序列化为xml
        /// 备注:使用的XmlSerializer
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="obj">泛型对象</param>
        /// <returns>xml格式字符串</returns>
        public static string XmlSerialize<T>(T obj)
        {
            Contract.Requires(obj != null);

            XmlSerializer xs = new XmlSerializer(typeof(T));

            using (TextWriter writer = new StringWriter(System.Globalization.CultureInfo.CurrentCulture))
            {
                xs.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        /// <summary>
        /// 将泛型对象序列化为指定字符编码的字符串
        /// 备注:使用的XmlSerializer
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="obj">泛型对象</param>
        /// <param name="encoding">字符编码 建议值为:Encoding.UTF8</param>
        /// <returns>xml格式字符串</returns>
        public static string XmlSerialize<T>(T obj, Encoding encoding)
        {
            Contract.Requires(obj != null);
            Contract.Requires(encoding != null);

            XmlSerializer xs = new XmlSerializer(typeof(T));

            using (MemoryStream stream = new MemoryStream())
            {
                xs.Serialize(stream, obj);
                return encoding.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// 将xml字符串反序列化为泛型对象
        /// 备注:使用的XmlSerializer
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="xml">xml字符串</param>
        /// <returns>泛型对象</returns>
        public static T XmlDeserialize<T>(string xml)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(xml));

            XmlSerializer xs = new XmlSerializer(typeof(T));

            using (TextReader reader = new StringReader(xml))
            {
                return (T)xs.Deserialize(reader);
            }
        }

        /// <summary>
        /// 将泛型对象序列化为xml文件
        /// 备注:使用的XmlSerializer
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="obj">泛型对象</param>
        /// <param name="filePath">要保存的xml文件路径</param>
        public static void SaveAsXml<T>(T obj, string filePath)
        {
            Contract.Requires(obj != null);
            Contract.Requires(filePath != null);

            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                xs.Serialize(stream, obj);
            }
        }

        /// <summary>
        /// 将泛型对象序列化为xml
        /// 备注:使用的DataContractSerializer
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="obj">泛型对象</param>
        /// <returns>xml格式字符串</returns>
        public static string XmlSerialize2<T>(T obj)
        {
            Contract.Requires(obj != null);

            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(stream, obj);
                stream.Position = 0;

                StreamReader reader = new StreamReader(stream);

                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 将泛型对象序列化为xml格式(有缩进)的字符串
        /// 备注:使用的DataContractSerializer
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="obj">泛型对象</param>
        /// <param name="encoding">字符编码</param>
        /// <returns>xml格式字符串</returns>
        public static string XmlSerialize2<T>(T obj, Encoding encoding)
        {
            Contract.Requires(obj != null);
            Contract.Requires(encoding != null);

            XmlWriterSettings setings = new XmlWriterSettings() { Indent = true, Encoding = encoding };
            using (MemoryStream stream = new MemoryStream())
            {
                XmlWriter writer = XmlWriter.Create(stream, setings);
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(writer, obj);
                writer.Flush();

                return encoding.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// 将xml字符串返序列化为泛型对象
        /// 备注:使用的DataContractSerializer
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="xml">xml字符串</param>
        /// <returns>泛型对象</returns>
        public static T XmlDeserialize2<T>(string xml)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(xml));

            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (TextReader txtReader = new StringReader(xml))
            {
                XmlReader xmlReader = XmlReader.Create(txtReader);
                return (T)serializer.ReadObject(xmlReader);
            }
        }

        /// <summary>
        /// 将泛型对象序列化为xml文件
        /// 备注:使用的DataContractSerializer
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="obj">泛型对象</param>
        /// <param name="filePath">要保存的xml文件路径</param>
        public static void SaveAsXml2<T>(T obj, string filePath)
        {
            Contract.Requires(obj != null);
            Contract.Requires(filePath != null);

            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));

                serializer.WriteObject(stream, obj);
            }
        }

        /// <summary>
        /// 将泛型对象序列化为xml文件(会对生成的xml进行格式进行缩进)
        /// 备注:使用的DataContractSerializer
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="obj">泛型对象</param>
        /// <param name="filePath">要保存的xml文件路径</param>
        /// <param name="encoding">字符编码</param>
        public static void SaveAsXml2<T>(T obj, string filePath, Encoding encoding)
        {
            Contract.Requires(obj != null);
            Contract.Requires(filePath != null);
            Contract.Requires(encoding != null);

            XmlWriterSettings setings = new XmlWriterSettings() { Indent = true, Encoding = encoding };
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                XmlWriter writer = XmlWriter.Create(stream, setings);
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(writer, obj);
                writer.Flush();
            }
        }

        #endregion

        #region Json序列化

        /// <summary>
        /// 将json字符串序列化为
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns>泛型对象</returns>
        public static T JsonDeserialize<T>(string json)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(json));

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 将泛型对象序列化为json字符串
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="obj">泛型对象</param>
        /// <returns>json字符串</returns>
        public static string JsonSerialize<T>(T obj)
        {
            Contract.Requires(obj != null);

            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将泛型对象序列化为json后保存到文件中
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="obj">泛型对象</param>
        /// <param name="filePath">文件路径</param>
        public static void SaveAsJson<T>(T obj, string filePath)
        {
            Contract.Requires(obj != null);
            Contract.Requires(filePath != null);

            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);

                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;

                JsonWriter jsonWriter = new JsonTextWriter(streamWriter);
                serializer.Serialize(jsonWriter, obj);
            }
        }

        #endregion
    }
}