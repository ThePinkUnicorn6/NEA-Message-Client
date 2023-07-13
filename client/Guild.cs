using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeaClient
{
    class Guild
    {
        public string Name;
        public string ID;
        public string OwnerID;
        public string Description;
        public List<Channel> Channels;
    }
}
