using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcClient
{
    public abstract class PLCBase
    {
        /// <summary>实现通讯类型</summary>
        public ICommunicationUnit communicationUnit {  get; set; }
        /// <summary>字符类型</summary>


    }
}
