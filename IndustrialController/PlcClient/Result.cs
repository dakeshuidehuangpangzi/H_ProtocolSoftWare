using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcClient
{
    public class Result
    {
        public Result()
        {
        }
        public bool Status { get; set; } = true;
        public int Code { get; set; } = 200;

        private string _Err;

        public string Err
        {
            get
            {
                return _Err;
            }
            set
            {
                _Err = value;
                AddErr2List();
            }
        }
        /// <summary>
        /// 请求报文
        /// </summary>
        public string Requst { get; set; }

        /// <summary>
        /// 响应报文
        /// </summary>
        public string Response { get; set; }

        public Exception Exception { get; set; }
        public DateTime InitialTime { get; protected set; } = DateTime.Now;
        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        public double? TimeConsuming { get; private set; }

        /// <summary>
        /// 结束时间统计
        /// </summary>
        public Result EndTime()
        {
            TimeConsuming = (DateTime.Now - InitialTime).TotalMilliseconds;
            return this;
        }
        public List<string> ErrList { get; private set; } = new List<string>();

        public void AddErr2List()
        {
            if (!ErrList.Contains(Err))
                ErrList.Add(Err);
        }

    }

    public class Result<T>:Result
    {
        public Result() { }
     
        public Result(Result Result, List<T> data)
        {
            Value = data;
        }
        public Result( List<T> data)
        {
            Value = data;
        }
        /// <summary>数据结果（泛型） </summary>
        public List<T> Value { get; set; }
    }
}
