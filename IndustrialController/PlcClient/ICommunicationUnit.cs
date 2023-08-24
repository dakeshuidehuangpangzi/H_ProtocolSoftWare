using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PlcClient
{
    public interface ICommunicationUnit:IDisposable
    {

        /// <summary>超时时间</summary>
        public int ConnectTimeout { get; set; }
        public bool Active { get; }

        /// <summary>连接</summary>
        public Result Connected();

        /// <summary>
        /// 发送和接收信息
        /// </summary>
        /// <param name="req">发送报文</param>
        /// <param name="receiveLen">接收长度</param>
        /// <param name="errorLen">错误长度</param>
        /// <returns></returns>
        public Result<byte> SendAndReceive(List<byte> req, int receiveLen = 0, int errorLen = 0);

        /// <summary>
        /// 异步发送和接收信息
        /// </summary>
        /// <param name="req">发送报文</param>
        /// <param name="receiveLen">接收长度</param>
        /// <param name="errorLen">错误长度</param>
        /// <returns></returns>
        public Result<byte> AnsySendAndReceive(List<byte> req, int receiveLen = 0, int errorLen = 0);

        /// <summary>打开后触发。</summary>
        event EventHandler Opend;
        /// <summary>关闭后触发。可实现掉线重连</summary>
        event EventHandler Closed;
    }
}
