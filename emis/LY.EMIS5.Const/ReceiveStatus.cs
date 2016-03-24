using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    /// <summary>
    /// 短信接收状态
    /// </summary>
    public enum ReceiveStatus
    {
        /// <summary>
        /// 未发送
        /// </summary>
        [Description("未发送")]
        Waiting = 0,
        /// <summary>
        /// 已发送，接收状态未知
        /// </summary>
        [Description("已发送")]
        Sended = 1,
        /// <summary>
        /// 发送失败，失败原因参照错误代码
        /// </summary>
        [Description("发送失败")]
        SendFailed = 2,
        /// <summary>
        /// 发送成功
        /// </summary>
        [Description("发送成功")]
        SendSuccess = 3,
        /// <summary>
        /// 接收失败，失败原因参照错误代码
        /// </summary>
        [Description("接收失败")]
        ReceiveFailed = 4,
        /// <summary>
        /// 接收成功
        /// </summary>
        [Description("接收成功")]
        ReceiveSuccess = 5,
    }
}
