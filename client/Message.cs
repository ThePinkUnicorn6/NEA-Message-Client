using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeaClient
{
    public class Message
    {
        public string ID { get; set; }
        public string ChannelID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }

        public string ComposeString()
        {
            string messageString = UserName + "\r\n" + Text;
            return messageString;
        }
        public void Encrypt()
        {

        }
        public void Decrypt()
        {

        }
    }
}
