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
        public string PlainText { get; set; }
        public string CypherText { get; set; }
        public string Time { get; set; }
        public byte[] IV { get; set; }

        public override string ToString() => UserName + "\r\n" + PlainText;

        public void Encrypt(byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                IV = aes.IV;
                byte[] messageByteArray = Encoding.UTF8.GetBytes(PlainText); // Convert the message to a byte array
                MemoryStream stream = new MemoryStream();
                {
                    using (CryptoStream cryptoStream = new(stream, aes.CreateEncryptor(key, IV), CryptoStreamMode.Write)) // Start a crypto stream to encrypt the message
                    {
                        cryptoStream.Write(messageByteArray, 0, messageByteArray.Length); // Write the message to the crypto stream to encrypt it
                    }
                    CypherText = Convert.ToBase64String(stream.ToArray()); // Replace Text with its encrypted version
                }
            }
        }
        public async Task Decrypt(byte[] key)
        {
            if (IV == null) { throw new Exception("IV not set"); }
            byte[] messageByteArray = Convert.FromBase64String(CypherText);
            MemoryStream stream = new MemoryStream(messageByteArray);
            using (Aes aes = Aes.Create())
            {
                using CryptoStream cryptoStream = new(stream, aes.CreateDecryptor(key, IV), CryptoStreamMode.Read);
                {
                    using(StreamReader decryptor = new StreamReader(cryptoStream))
                    {
                        PlainText = await decryptor.ReadToEndAsync();
                    }
                }
            }
        }
    }
}
