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
        public string KeyDigest;
        public List<Channel> Channels;
        public byte[] Key;
        public void GetKey()
        {
            Utility utility = new Utility();
            var keys = utility.getKeysFromFile();
            List<string> keyGuilds = new List<string> { };
            for (int i = 0; i < keys.Count; i++)
            {
                keyGuilds.Add(keys[i][0]); // Create list of just the guilds to be used to search through
            }
            utility.binarySearch(keyGuilds.ToArray(), ID, out bool found, out int keyIndex);
            if (found)
            {
                Key = Convert.FromBase64String(keys[keyIndex][1]);
            }
        }
    }
}
