using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baidu.SDK.Push;
using Baidu.SDK.Push.Dto;

namespace LY.EMIS5.Common.BaiDu
{
    public class BaiDuPush
    {
        public static readonly BaiDuPush Instance = new BaiDuPush();
        private Client client = null;
        private BaiDuPush()
        {
            client = new Client("nGuXzpxVo1DW851lUTOPmP4B", "tIce25zgwXM9Yzv3jEmxqZxHgRAuao53");
        }

        public bool PushMessage(string message, string key, int userId = 0)
        {
            MessagePushResponse AndroidResponse = null;
            MessagePushResponse IOSResponse = null;
            if (userId == 0)
            {
                AndroidResponse = client.PushMessageBroadcast(new MessagePushBroadcastRequest(DeviceTypes.Android, MessageTypes.Message, new List<string> { message }, new List<string> { key }));
                IOSResponse = client.PushMessageBroadcast(new MessagePushBroadcastRequest(DeviceTypes.IOS, MessageTypes.Message, new List<string> { message }, new List<string> { key }));
            }
            else
            {
                AndroidResponse = client.PushMessageUnicast(new MessagePushUnicastRequest(DeviceTypes.Android, 0, userId.ToString(), MessageTypes.Message, new List<string> { message }, new List<string> { key }));
                IOSResponse = client.PushMessageUnicast(new MessagePushUnicastRequest(DeviceTypes.IOS, 0, userId.ToString(), MessageTypes.Message, new List<string> { message }, new List<string> { key }));
            }
            if (AndroidResponse.RequestId > 0 && IOSResponse.RequestId > 0)
            {
                return true;
            }
            return false;
        }

        public bool PushTagMessage(string message, string key, string tag)
        {
            MessagePushResponse AndroidResponse = null;
            MessagePushResponse IOSResponse = null;

            AndroidResponse = client.PushMessageTag(new MessagePushTagRequest(DeviceTypes.Android, tag, MessageTypes.Message, new List<string> { message }, new List<string> { key }));
            IOSResponse = client.PushMessageTag(new MessagePushTagRequest(DeviceTypes.Android, tag, MessageTypes.Message, new List<string> { message }, new List<string> { key }));

            if (AndroidResponse.RequestId > 0 && IOSResponse.RequestId > 0)
            {
                return true;
            }
            return false;
        }

        public bool PushTagMessage(Dictionary<string, string> dictionary, string tag)
        {
            MessagePushResponse AndroidResponse = null;
            MessagePushResponse IOSResponse = null;
            AndroidResponse = client.PushMessageTag(new MessagePushTagRequest(DeviceTypes.Android, tag, MessageTypes.Message, dictionary.Select(c => c.Value).ToList(), dictionary.Select(c => c.Key).ToList()));
            IOSResponse = client.PushMessageTag(new MessagePushTagRequest(DeviceTypes.IOS, tag, MessageTypes.Message, dictionary.Select(c => c.Value).ToList(), dictionary.Select(c => c.Key).ToList()));
            if (AndroidResponse.RequestId > 0 && IOSResponse.RequestId > 0)
            {
                return true;
            }
            return false;
        }

     
    }
}
