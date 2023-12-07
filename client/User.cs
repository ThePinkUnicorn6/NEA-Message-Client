using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace NeaClient
{
    public class User
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string Token { get; set; }
        public byte[] PublicKey { get; set; }
        public byte[] PrivateKey { get; set; }

        public void GenerateKeys()
        {
            RSA rsa = RSA.Create();
            PublicKey = rsa.ExportRSAPublicKey();
            PrivateKey = rsa.ExportRSAPrivateKey();
        }
    }
}
