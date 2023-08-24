using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PlcClient
{
    public abstract class SocketBase : ICommunicationUnit
    {
        public SocketBase( string IP,int Port ,int TimeOut)
        {
            this.Port = Port;
            this.Ip = IP;
            this.ConnectTimeout = TimeOut;
        }
        #region 属性
        Socket Socket;
        public int ConnectTimeout { get; set; } = 15000;
        public bool Active => Socket != null && Socket.Connected;

        public string Ip { get; set; }
        public int  Port { get; set; }

        ManualResetEvent TimeoutObject = new ManualResetEvent(false);

        #endregion

        #region 事件
        public event EventHandler Opend;
        public event EventHandler Closed;
        #endregion

        #region 连接
        public virtual void Dispose()
        {
            try
            {
                if (Socket != null && Socket.Connected)
                {
                    Socket.Shutdown(SocketShutdown.Both);
                    Socket.Dispose();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public virtual Result Connected()
        {
            Result result = new Result();

            try
            {
                Stopwatch sw = Stopwatch.StartNew();

                while (sw.ElapsedMilliseconds < ConnectTimeout)
                {
                    try
                    {
                        Socket?.Close();
                        Socket?.Dispose();
                        Socket = null;

                        Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        Socket.ReceiveTimeout = ConnectTimeout;
                        // Socket.Connect(IPAddress.Parse(Ip), Convert.ToInt16(Port));
                        Socket.BeginConnect(Ip, Port, callback => {

                            var caSocket = callback.AsyncState as Socket;
                            if (caSocket != null)
                            {
                                if (caSocket.Connected)
                                {
                                    caSocket.EndConnect(callback);//结束异步连接请求
                                }
                            }
                            TimeoutObject.Set();
                        }, Socket);
                        TimeoutObject.WaitOne(ConnectTimeout, false);
                    }
                    catch (SocketException ex)
                    {
                        if (ex.ErrorCode == 10060)
                            throw new Exception(ex.Message);
                    }
                    catch (Exception) { }
                    if (Socket == null || !Socket.Connected || ((Socket.Poll(200, SelectMode.SelectRead) && (Socket.Available == 0))))
                    {
                        throw new Exception("网络连接失败");
                    }

                }


            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.Err = ex.Message;
                result.Status = false;
                result.Code = 404;
            }
            return result.EndTime();
        }
        #endregion

        public virtual Result<byte> SendAndReceive(List<byte> req, int receiveLen = 0, int errorLen = 0)
        {
            throw new NotImplementedException();
        }

        public virtual Result<byte> AnsySendAndReceive(List<byte> req, int receiveLen = 0, int errorLen = 0)
        {
            throw new NotImplementedException();
        }





        /// <summary>
        ///  析构函数
        /// </summary>
        ~SocketBase() 
        {
            Dispose();
        }
    }
}
