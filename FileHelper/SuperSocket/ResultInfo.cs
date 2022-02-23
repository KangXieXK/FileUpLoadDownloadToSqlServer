using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public class MyData//20字节
    {
        /// <summary>
        /// 开始符号6字节, "!Start"
        /// </summary>
        public string Start { get; set; }
        /// <summary>
        /// 消息类型，1字节
        /// </summary>
        public byte key { get; set; }
        /// <summary>
        /// 主体消息数据包长度，4字节
        /// </summary>
        public uint Lenght { get; set; }
        /// <summary>
        /// 4字节唯一设备识别符（Unique Device Identifier）
        /// </summary>
        public uint DeviceUDID { get; set; }
        /// <summary>
        /// 目标命令类型1字节
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// 主体消息
        /// </summary>
        public byte[] Body { get; set; }
        /// <summary>
        /// 结束符号4字节, "$End" ,
        /// </summary>
        public string End { get; set; }

        public override string ToString()
        {
            return string.Format("开始符号:{0},消息类型:{1},数据包长度:{2},唯一设备识别符:{3},目标命令类型:{4},主体消息:{5},结束符号：{6}",
                Start, key, Lenght, Lenght, DeviceUDID, Type, Body, End);
        }
    }


    public class MyRequestInfo : RequestInfo<MyData>
    {
        public MyRequestInfo(string key, MyData myData)
        {
            //如果需要使用命令行协议的话，那么key与命令类名称myData相同
            Initialize(key, myData);
        }
    }

    public class MyReceiveFilter : IReceiveFilter<MyRequestInfo>
    {
        public Encoding encoding = Encoding.GetEncoding("gb2312");
        /// <summary>
        /// Gets the size of the left buffer.
        /// </summary>
        /// <value>
        /// The size of the left buffer.
        /// </value>
        public int LeftBufferSize { get; }
        /// <summary>
        /// Gets the next receive filter.
        /// </summary>
        public IReceiveFilter<MyRequestInfo> NextReceiveFilter { get; }

        public FilterState State { get; private set; }
        /// <summary>
        /// 该方法将会在 SuperSocket 收到一块二进制数据时被执行，接收到的数据在 readBuffer 中从 offset 开始， 长度为 length 的部分。
        /// </summary>
        /// <param name="readBuffer">接收缓冲区, 接收到的数据存放在此数组里</param>
        /// <param name="offset">接收到的数据在接收缓冲区的起始位置</param>
        /// <param name="length">本轮接收到的数据的长度</param>
        /// <param name="toBeCopied">表示当你想缓存接收到的数据时，是否需要为接收到的数据重新创建一个备份而不是直接使用接收缓冲区</param>
        /// <param name="rest">这是一个输出参数, 它应该被设置为当解析到一个为政的请求后，接收缓冲区还剩余多少数据未被解析</param>
        /// <returns></returns>
        /// 当你在接收缓冲区中找到一条完整的请求时，你必须返回一个你的请求类型的实例.
        /// 当你在接收缓冲区中没有找到一个完整的请求时, 你需要返回 NULL.
        /// 当你在接收缓冲区中找到一条完整的请求, 但接收到的数据并不仅仅包含一个请求时，设置剩余数据的长度到输出变量 "rest". SuperSocket 将会检查这个输出参数 "rest", 如果它大于 0, 此 Filter 方法 将会被再次执行, 参数 "offset" 和 "length" 会被调整为合适的值.
        public MyRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;
            if (length < 21)//没有数据
                return null;
            byte[] data = new byte[length];
            Buffer.BlockCopy(readBuffer, offset, data, 0, length);
            var str = encoding.GetString(data);
            MyData myData = new MyData();
            myData.Start = encoding.GetString(data, 0, 6);//6字节
            myData.key = data[6];//1字节
            myData.Lenght = BitConverter.ToUInt32(data, 7);//4字节  6 + 1
            if (length < myData.Lenght + 20)
                return null;
            myData.DeviceUDID = BitConverter.ToUInt32(data, 11);//4字节 6 + 1+4
            myData.Type = data[15];//1字节 6+1+4+4

            myData.Body = new byte[myData.Lenght];//myData.Lenght字节
            Buffer.BlockCopy(data, 16, myData.Body, 0, (int)myData.Lenght);

            myData.End = encoding.GetString(data, (int)(16 + myData.Lenght), 4);//4字节
            if (myData.Start != "!Start" || myData.End != "$End")
                return null;
            rest = (int)(length - (20 + myData.Lenght));//未处理数据
            return new MyRequestInfo(myData.key.ToString(), myData);
        }

        public void Reset()
        {

        }
    }

    /// <summary>
    /// Receive filter factory interface
    /// </summary>
    /// <typeparam name="TRequestInfo">The type of the request info.</typeparam>
    public class MyReceiveFilterFactory : IReceiveFilterFactory<MyRequestInfo>
    {
        /// <summary>
        /// Creates the receive filter.
        /// </summary>
        /// <param name="appServer">The app server.</param>
        /// <param name="appSession">The app session.</param>
        /// <param name="remoteEndPoint">The remote end point.</param>
        /// <returns>
        /// the new created request filer assosiated with this socketSession
        /// </returns>
        //MyReceiveFilter<MyRequestInfo> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint);
        public IReceiveFilter<MyRequestInfo> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint)
        {
            return new MyReceiveFilter();
        }
    }
}
