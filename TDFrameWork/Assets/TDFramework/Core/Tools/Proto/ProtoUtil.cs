using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;

namespace TDFramework
{
    public static class ProtoUtil
    {
        /// <summary>
        ///  将消息序列化为二进制的方法
        /// </summary>
        /// <param name="meg">要序列化的对象</param>
        /// <returns></returns>
        public static byte[] Serizlize<T>(T obj) where T : IMessage
        {
            return obj.ToByteArray();
        }


        /// <summary>
        /// 将收到的消息反序列化成对象
        /// </summary>
        /// <param name="msg">收到的消息</param>
        /// <returns></returns>
        public static T DeSerizlize<T>(byte[] msg) where T : IMessage, new()
        {
            IMessage message = new T();
            return (T)message.Descriptor.Parser.ParseFrom(msg);
        }
    }
}
