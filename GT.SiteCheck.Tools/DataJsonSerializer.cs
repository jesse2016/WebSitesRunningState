using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace QuartzDemo.GT.SiteCheck.Tools
{
    /// <summary>
    /// json与实体转换类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataJsonSerializer<T>
    {
        #region json转实体
        /// <summary>
        /// json转实体
        /// </summary>
        /// <param name="jsonString">json字符串</param>
        /// <returns></returns>
        public static T JsonToEntity(string jsonString)
        {
            try
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                T jsonObject = (T)ser.ReadObject(ms);
                ms.Close();
                return jsonObject;  
            }
            catch (Exception)
            {
                return default(T);
            }
        }
        #endregion

        #region 实体转json
        /// <summary>
        /// 实体转json
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static string EntityToJson(T obj)
        {
            string rtnStr = string.Empty;
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                MemoryStream stream = new MemoryStream();
                serializer.WriteObject(stream, obj);
                byte[] dataBytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(dataBytes, 0, (int)stream.Length);
                rtnStr = System.Text.Encoding.UTF8.GetString(dataBytes);
            }
            catch(Exception)
            {
            }
            return rtnStr;
        }
        #endregion
    }
}