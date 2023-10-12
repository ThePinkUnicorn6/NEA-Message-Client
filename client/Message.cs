using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        public void Encrypt(byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;

                byte[] iv = aes.IV;
                byte[] messageByteArray = Encoding.UTF8.GetBytes(Text);
                MemoryStream stream = new MemoryStream(messageByteArray);
                stream.Write(iv, 0, iv.Length);

                using (CryptoStream cryptoStream = new(
                    stream,
                    aes.CreateEncryptor(),
                    CryptoStreamMode.Write))
                {
                    using (StreamWriter encryptWriter = new(cryptoStream, Encoding.Unicode))
                    {
                        encryptWriter.WriteLine(Text);
                    }
                }
            }
        }
        public void Decrypt(string guildKey)
        {

        }
    }
}
