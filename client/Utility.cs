using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeaClient
{
    internal class Utility
    {
        const string keyFile = "guildKeys.csv";
        public void binarySearch(string[] array, string item, out bool found, out int index)
        {
            found = false;
            index = array.ToList().IndexOf(item); //TODO: Add binary search, not this tempory solution
            if (index == -1)
            {
                index = 0;
            }
            else found = true;
        }
        public List<string[]> getKeys()
        {
            List<string[]> keys = new();
            if (File.Exists(keyFile))
            {
                try
                {
                    keys = File.ReadLines(keyFile).Select(x => x.Split(',')).ToList();
                }
                catch { }
            }
            return keys;
        }
        public void requestGuildKey(string guildID)
        {
            // TODO: make key request
        }
        public async Task checkNewMessages()
        {

        }
    }
}
