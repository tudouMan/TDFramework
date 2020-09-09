using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TDFramework.Net
{
    [MonoSingletonPath("[Net]/[TCP]")]
    public class NetTcp : MonoSingleton<NetTcp>
    {
        private Socket mSocket;
        private string mIpAddress="192.168.2.188";
        private int mPort=8899;
        private void Awake()
        {
            OnInitTcp();
        }

        private void OnInitTcp()
        {
            IPAddress ip = IPAddress.Parse(mIpAddress);
            IPEndPoint ip_end_point = new IPEndPoint(ip, mPort);
            mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mSocket.Connect(ip_end_point);
            Thread thread = new Thread(Receive);
            thread.Start();
            
        }


      
        public void Send(byte[]msg)
        {
            mSocket.Send(msg);
        }



        public void Receive()
        {
            while (true)
            {
                byte[] buffer = new byte[1024*3];
                int length = mSocket.Receive(buffer);
                string str = Encoding.UTF8.GetString(buffer, 0, length);
                UnityEngine.Debug.Log("客户端打印：" + mSocket.RemoteEndPoint + ":" + str);
            }
          
        }
    }
}
