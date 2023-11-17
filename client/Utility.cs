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
            //found = false;
            //index = list.ToList().IndexOf(item); //TODO: Add binary search, not this tempory solution
            //if (index == -1)
            //{
            //    index = 0;
            //}
            //else found = true;
            int lowerBound = 0;
            int upperBound = list.Length - 1;
            int midPoint = 0;
            found = false;
            while (found == false && upperBound != lowerBound)
            {
                midPoint = (upperBound - lowerBound) / 2;
                Console.WriteLine(midPoint);
                switch (string.Compare(list[midPoint], item))
                {
                    case 0:
                        found = true;
                        break;
                    case -1:
                        lowerBound = midPoint + 1;
                        break;
                    case 1:
                        upperBound = midPoint - 1;
                        break;
                }
            }
            if (found == true)
            {
                index = midPoint;
            }
            else
            {
                index = -1;
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
