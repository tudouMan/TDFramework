using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Collections.Generic;
using System.Text;
using TDFramework;


public class NetWorkSocket : MonoSingleton<NetWorkSocket>
{


    //private byte[] buffer = new byte[10240];

    #region 发送消息所需变量
    /// <summary>
    /// 发送消息队列
    /// </summary>
    private Queue<byte[]> mSendQueue = new Queue<byte[]>();

    /// <summary>
    /// 检查队列的委托
    /// </summary>
    private Action mCheckSendQueue;

    //压缩标志位
    private const int mCompressLen = 200;
    #endregion

    #region 接受消息所需变量
    private byte[] m_ReceiveBuffer = new byte[10240];//接收数据包的字节数组缓冲区
    private ByteMemoryStream m_ReceiveMS = new ByteMemoryStream();//接收数据包的缓冲数据流
    private Queue<byte[]> m_ReceiveQueue = new Queue<byte[]>();//接收消息的队列
    private int m_ReceiveCount = 0;
    #endregion

    /// <summary>
    /// 客户端socket
    /// </summary>
    private Socket m_Client;

    /// <summary>
    /// 连接成功委托
    /// </summary>
    public Action OnConnectOk;


    //==================接收消息=================================
    #region 从连接的 Socket 中异步接收数据
    /// <summary>
    /// 接收数据
    /// </summary>
    private void ReceiveMsg()
    {
        //开始从连接的 Socket 中异步接收数据
        m_Client.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, m_Client);

        //buffer Byte 类型的数组，它是存储接收到的数据的位置。 
        //offset buffer 参数中存储所接收数据的位置，该位置从零开始计数。 
        //size 要接收的字节数。 
        //socketFlags SocketFlags 值的按位组合。 
        //callback 一个 AsyncCallback 委托，它引用操作完成时要调用的方法。 
        //state 一个用户定义对象，其中包含接收操作的相关信息。当操作完成时，此对象会被传递给 EndReceive 委托。

        //异步 BeginReceive 操作必须通过调用 EndReceive 方法来完成。通常，该方法由 callback 委托调用。
    }
    #endregion

    #region  接收数据回调  ReceiveCallBack
    //接收数据回调
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            int len = m_Client.EndReceive(ar);

            if (len > 0)//已经接收到数据
            {

                //把接收到的数据写入缓冲数据流的尾部
                m_ReceiveMS.Position = m_ReceiveMS.Length;

                //把长度的字节写入数据流
                m_ReceiveMS.Write(m_ReceiveBuffer, 0, len);

                //如果缓存数据流的长度大于2说明至少有个不完整的包传递过来,因为包头为ushort长度为2
                if (m_ReceiveMS.Length > 2)
                {
                    //进行循环拆分数据包
                    while (true)
                    {
                        //把数据流指针位置放在0处
                        m_ReceiveMS.Position = 0;

                        //currMsgLen =包体的长度
                        int currMsgLen = m_ReceiveMS.ReadUShort();

                        //总包的长度
                        int currFullMsgLen = 2 + currMsgLen;

                        if (m_ReceiveMS.Length >= currFullMsgLen)
                        {
                            //至少收到一个完整包
                            //定一包体的buffer
                            byte[] buffer = new byte[currMsgLen];

                            //把数据流指针位置放在包体处
                            m_ReceiveMS.Position = 2;

                            //把包体读到buffer中
                            m_ReceiveMS.Read(buffer, 0, currMsgLen);

                            //把数据压入队列
                            lock (m_ReceiveQueue)
                            {
                                m_ReceiveQueue.Enqueue(buffer);
                            }
                            //==========处理剩余字节数组=========

                            //剩余包长度
                            int remainLen = (int)m_ReceiveMS.Length - currFullMsgLen;
                            if (remainLen > 0)
                            {
                                //把指针放在第一个包的尾部
                                m_ReceiveMS.Position = currFullMsgLen;

                                //定义剩余字节数据组
                                byte[] remainBuffer = new byte[remainLen];

                                //把数据流得到剩余字节数组
                                m_ReceiveMS.Read(remainBuffer, 0, remainLen);

                                //请空数据流
                                m_ReceiveMS.Position = 0;
                                m_ReceiveMS.SetLength(0);

                                //把剩余字节数组重新写入数据流
                                m_ReceiveMS.Write(remainBuffer, 0, remainBuffer.Length);

                                remainBuffer = null;
                            }
                            else
                            {
                                //没有剩余字节
                                //请空数据流
                                m_ReceiveMS.Position = 0;
                                m_ReceiveMS.SetLength(0);
                                break;
                            }
                        }
                        else
                        {
                            //还没有收到完整包
                            break;
                        }
                    }
                }

                //进行下一次接收数据包
                ReceiveMsg();
            }
            else
            {
                //服务器断开连接
                Debug.Log(string.Format("服务器{0}断开连接", m_Client.RemoteEndPoint.ToString()));


            }
        }
        catch
        {
            //服务器断开连接
            Debug.Log(string.Format("服务器{0}断开连接", m_Client.RemoteEndPoint.ToString()));


        }
    }
    #endregion
    //==========================================================

    private void Update()
    {
        #region 从队列获取数据
        while (true)
        {
            if (m_ReceiveCount <= 5)
            {
                m_ReceiveCount++;
                lock (m_ReceiveQueue)
                {
                    if (m_ReceiveQueue.Count > 0)
                    {
                        //得到队列中的数据包                
                        byte[] buffer = m_ReceiveQueue.Dequeue();
                        //得到加密后的数据
                        byte[] bufferNew = new byte[buffer.Length - 3];

                        bool isCompress = false;
                        ushort crc = 0;

                        using (ByteMemoryStream ms = new ByteMemoryStream(buffer))
                        {
                            isCompress = ms.ReadBool();
                            crc = ms.ReadUShort();
                            ms.Read(bufferNew, 0, bufferNew.Length);
                        }

                        //先校验
                        int newCrc = Crc16.CalculateCrc16(bufferNew);
                        if (newCrc == crc)
                        {
                            //异或得到原始数据
                            bufferNew = SecurityUtil.Xor(bufferNew);

                            if (isCompress)
                            {
                                bufferNew = ZlibHelper.DeCompressBytes(bufferNew);
                            }

                            ushort protoCode = 0;
                            byte[] ProtoContent = new byte[bufferNew.Length - 2];
                            //协议编号
                            using (ByteMemoryStream ms = new ByteMemoryStream(bufferNew))
                            {
                                protoCode = ms.ReadUShort();
                                ms.Read(ProtoContent, 0, ProtoContent.Length);
                                //获取数据需要派发出去
                                TDFramework.EventSystem.EventCenter.Broadcast(protoCode, ProtoContent);
                               
                            }
                        }
                        else
                        {
                            Debug.Log("校验出错，不予接受");
                        }
                    }
                    else
                    { break; }
                }
            }
            else
            {
                m_ReceiveCount = 0;
                break;
            }
        }
        #endregion
    }


    protected override void OnDestroy()
    {
        DisConnect();
        base.OnDestroy();
    }

    /// <summary>
    /// 断开服务器
    /// </summary>
    public void DisConnect()
    {
        if (m_Client != null && m_Client.Connected)
        {
            m_Client.Shutdown(SocketShutdown.Both);
            m_Client.Close();
        }
    }

    //========发送消息=================
    #region 连接到socket服务器
    /// <summary>
    /// 连接到Socket服务器
    /// </summary>
    /// <param name="ip">ip</param>
    /// <param name="port">端口号</param>
    public void Connect(string ip, int port)
    {
        //如果docket已经存在 并且处于连接中状态 则直接返回
        if (m_Client != null && m_Client.Connected) return;

        m_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            m_Client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            mCheckSendQueue = OnCheckSendQueueCallBack;
            Debug.Log("连接成功");
            ReceiveMsg();

            if(OnConnectOk!=null)
            {
                OnConnectOk();//连接成功后执行委托
            }
        }
        catch (Exception ex)
        {
            Debug.Log("连接失败=" + ex.Message);
        }
    }
    #endregion

    #region 检查队列的委托回调
    /// <summary>
    /// 检查队列的委托回调
    /// </summary>
    private void OnCheckSendQueueCallBack()
    {
        lock (mSendQueue)
        {
            //如果队列中有数据包 则发送数据包
            if (mSendQueue.Count > 0)
            {
                //发送数据包
                Send(mSendQueue.Dequeue());
            }

        }
    }
    #endregion

    #region 封装数据包
    /// <summary>
    /// 封装数据包
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private byte[] MakeData(byte[] data)
    {
        byte[] retBuffer = null;

        //1.判断是否压缩
        bool isCompress = data.Length > mCompressLen ? true : false;
        if (isCompress)//进行压缩
        {
            data = ZlibHelper.DeCompressBytes(data);
        }

        //2.先异或
        data = SecurityUtil.Xor(data);

        //3.校验
        ushort crc = Crc16.CalculateCrc16(data);
        using (ByteMemoryStream ms = new ByteMemoryStream())
        {
            ms.WriteUShort((ushort)(data.Length + 3));
            ms.WriteBool(isCompress);
            ms.WriteUShort(crc);
            ms.Write(data, 0, data.Length);
            retBuffer = ms.ToArray();
        }
        return retBuffer;
    }
    #endregion

    #region 发送消息（把消息加入队列
    public void SendMsg(byte[] buffer)
    {
        //得到封装后的数据包
        byte[] sendBuffer = MakeData(buffer);

        lock (mSendQueue)
        {
            //把数据包加入队列
            mSendQueue.Enqueue(sendBuffer);

            //启动委托(执行委托)
            mCheckSendQueue.BeginInvoke(null, null);//m_CheckSendQueue()相当于m_CheckSendQueue.Invoke()执行委托。begininvoke是异步执行委托
        }
    }
    #endregion

    #region 真正发送数据包
    /// <summary>
    /// 真正发送服务包到服务器
    /// </summary>
    /// <param name="buffer"></param>
    private void Send(byte[] buffer)
    {
        m_Client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, m_Client);
    }
    #endregion

    #region 发送数据包的回调
    /// <summary>
    /// 发送数据包的回调
    /// </summary>
    /// <param name="ar"></param>
    private void SendCallBack(IAsyncResult ar)
    {
        m_Client.EndSend(ar);
        OnCheckSendQueueCallBack();
    }
    #endregion
    //================================
}
