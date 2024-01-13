using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Cryptography;

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
        public List<string[]> getKeysFromFile()
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
        public void saveKey(string guildID, byte[] key)
        {
            List<string[]> keys = new();
            if (File.Exists(keyFile))
            {
                try
                {
                    keys = File.ReadLines(keyFile).Select(x => x.Split(',')).ToList(); // Reads the key file
                }
                catch { MessageBox.Show("Key file corrupt!", "Error"); } 
            }
            if (keys.Count == 0)
            {
                File.WriteAllText(keyFile, guildID + "," + Convert.ToBase64String(key)); // If the file is empty, write to it.
            }
            else
            {
                List<string> keyGuilds = new();
                for (int i = 0; i < keys.Count; i++)
                {
                    keyGuilds.Add(keys[i][0]); // Converts the keys and guilds array to an array of just gilds to search for the place to insert to.
                }
                binarySearch(keyGuilds.ToArray(), guildID, out bool found, out int index);
                if (!found)
                {
                    keys.Insert(index, new string[] { guildID, Convert.ToBase64String(key) }); // Inserts the new key and guild at the location needed
                    File.WriteAllText(keyFile, ""); // Clear file before writing all the data back to it with the new guild included
                    for (int i = 0; i < keys.Count; i++)
                    {
                        File.AppendAllText(keyFile, keys[i][0] + "," + keys[i][1] + "\r\n"); // Saves the data to the file
                    }
                }
            }
        }
        public async Task<bool> requestGuildKey(string guildID, User user, string keyDigest)
        {
            HttpClient client = new() { BaseAddress = new Uri("http://" + user.ServerURL) };
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            byte[] keyCypherText;
            try
            {
                var content = new
                {
                    token = user.Token,
                    guildID = guildID
                };
                response = await client.PostAsJsonAsync("/api/guild/key/request", content);
                successfullConnection = true;
            }
            catch
            {
                successfullConnection = false;
            }
            if (successfullConnection)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                if ((int)response.StatusCode == 200 && jsonResponseObject != null && jsonResponseObject.ContainsKey("key")) // If the client has requested the keys previously and another user has submitted the keys, 
                {
                    byte[] guildKey;
                    keyCypherText = Convert.FromBase64String((string)jsonResponseObject.key);
                    // Decrypt guild key
                    using (RSA rsa = RSA.Create())
                    {
                        rsa.ImportRSAPrivateKey(user.PrivateKey, out _);
                        guildKey = rsa.Decrypt(keyCypherText, RSAEncryptionPadding.OaepSHA256);
                    }
                    // Check if key is valid
                    if (Convert.ToBase64String(SHA256.HashData(guildKey)) == keyDigest)
                    {
                        // If the key that has been submited is correct, save it to file.
                        saveKey(guildID, guildKey);
                        return true;
                    }
                }
                else if (jsonResponseObject != null && jsonResponseObject.ContainsKey("errcode"))
                {
                    MessageBox.Show(jsonResponseObject.error.ToString(), "Error: " + jsonResponseObject.errcode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {

                 MessageBox.Show("Could not connect to: " + user.ServerURL, "Connection Error.");

            }
            return false;
        }
    }
}
