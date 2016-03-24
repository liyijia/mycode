
using System;
using System.Configuration;
using System.IO;

namespace LY.EMIS5.Const
{
    public class GlobalMember
    {
        /// <summary>
        /// 送货方式
        /// </summary>
        public enum DeliverType
        {
            OrdinaryPost = 0, //普通邮寄
            ExpressDelivery = 1, //快递
            Ems = 2 //EMS
        }

        /// <summary>
        /// 送货状态
        /// </summary>
        public enum DeliveryStatus
        {
            ToSend = 0, //待发送
            InTransit = 1, //运送中
            Extension = 2, //延期中
            Arrive = 3 //到货
        }

        /// <summary>
        /// 订单状态
        /// </summary>
        public enum OrderStatus
        {
            Start = 0, //订单申请开始
            End = 1 //订单申请结束
        }

        /// <summary>
        /// 兑换方式
        /// </summary>
        public enum ExchangeMode
        {
            VirtualCurrency = 0, //虚拟币
            Integral = 1 //积分
        }

        /// <summary>
        /// Http前缀
        /// </summary>
        public static string HttpPrefix = ConfigurationManager.AppSettings["httpPreFix"];

        /// <summary>
        /// 图片保存路径
        /// </summary>
        public static string ImagePath = ConfigurationManager.AppSettings["UploadConfig"];

        /// <summary>
        /// 获取图片的Url
        /// </summary>
        /// <param name="strImagePath">图片Url</param>
        /// <returns></returns>
        public static string GetImageUrl(string strImagePath)
        {
            if (string.IsNullOrEmpty(strImagePath))
                return "~/UploadFile/default.jpg";

            string strFilePath = ImagePath;

            var strImageFilePath = (ImagePath + strImagePath);

            if (!File.Exists(strImageFilePath))
                return "~/UploadFile/default.jpg";

            return HttpPrefix + strImagePath;
        }
    }
}