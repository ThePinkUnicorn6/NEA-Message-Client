using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeaClient
{
    public class Guild
    {
        public string Name;
        public string ID;
        public string OwnerID;
        public string Description;
        public string guildKey;
        public List<Channel> Channels;
        public byte[] Key;
    }
}
