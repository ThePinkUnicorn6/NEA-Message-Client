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
        public void binarySearch(string[] list, string item, out bool found, out int index)
        {
            int lower = 0;
            int upper = list.Length - 1;
            int mid = 0;
            found = false;
            while (!found && lower <= upper)
            {
                mid = (upper + lower) / 2;
                switch (string.Compare(list[mid], item))
                {
                    case 0:
                        found = true;
                        break;
                    case < 0:
                        lower = mid + 1;
                        break;
                    case > 0:
                        upper = mid - 1;
                        break;
                }
            }
            if (found)
            {
                index = mid;
            }
            else
            {
                index = lower;
            }
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
