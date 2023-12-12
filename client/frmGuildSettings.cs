using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace NeaClient
{
    public partial class frmGuildSettings : Form
    {
        string[] serverDetails;
        string guildID;
        public frmGuildSettings(string[] serverDetails, string guildID = null)
        {
            InitializeComponent();
            this.serverDetails = serverDetails;
            this.guildID = guildID;
        }
        private void frmGuildSettings_Load(object sender, EventArgs e)
        {
            if (guildID != null)
            {
                fetchGuildDetails();
            }
        }
        private async void fetchGuildDetails()
        {

        }
        private async void newGuild()
        {
            HttpResponseMessage response;
           
            byte[] key = RandomNumberGenerator.GetBytes(16);
            string keyString = Convert.ToBase64String(key);
            string keyDigest = Convert.ToBase64String(SHA256.HashData(key));
            HttpClient client = new() { BaseAddress = new Uri("http://" + serverDetails[0]) };
            try
            {
                var content = new
                {
                    Token = serverDetails[1],
                    GuildName = txtGuildName.Text,
                    guildKeyDigest = keyDigest,
                };
                response = await client.PostAsJsonAsync("/api/guild/create", content);            }
            catch
            {
                MessageBox.Show("Could not connect to " + serverDetails[0], "Connection Error.");
                return;
            }
            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

            if (response.IsSuccessStatusCode)
            {
                const string keyFile = "GuildKeys.csv";
                guildID = jsonResponseObject.GuildID.ToString();
                if (!string.IsNullOrWhiteSpace(txtGuildDescription.Text)) // If description is not empty, send it to server.
                {
                    try
                    {
                        var content = new
                        {
                            token = serverDetails[1],
                            guildID = guildID,
                            guildDesc = txtGuildDescription.Text
                        };
                        response = await client.PostAsJsonAsync("/api/guild/setDetails", content);
                    }
                                
                    catch
                    {
                        MessageBox.Show("Could not connect to " + serverDetails[0], "Connection Error.");
                        return;
                    }
                }
                if (!File.Exists(keyFile))
                {
                    File.WriteAllText(keyFile, guildID + "," + keyString);
                    Close();
                    return;
                }

                List<string[]> keys = new();
                try
                {
                    keys = File.ReadLines(keyFile).Select(x => x.Split(',')).ToList(); // Reads the key file
                }
                catch { MessageBox.Show("Key file corrupt!", "Error"); }
                if (keys.Count == 0) 
                {
                    File.WriteAllText(keyFile, guildID + "," + keyString); // If something has gone wrong and the file is empty, write to it.
                    Close();
                    return;
                }

                List<string> keyGuilds = new();
                for (int i = 0; i < keys.Count; i++)
                {
                    keyGuilds.Add(keys[i][0]); // Converts the keys and guilds array to an array of just gilds to search for the place to insert to.
                }
                Utility utility = new();
                utility.binarySearch(keyGuilds.ToArray(), guildID, out bool found, out int index);
                keys.Insert(index, new string[] { guildID, keyString }); // Inserts the new key and guild at the location needed
                File.WriteAllText(keyFile, ""); // Clear file before writing all the data back to it with the new guild included
                for (int i = 0; i < keys.Count; i++)
                {
                    File.AppendAllText(keyFile, keys[i][0] + "," + keys[i][1] + "\r\n"); // Saves the data to the file
                }
                Close();
            }
            else
            {
                MessageBox.Show(jsonResponseObject.error.ToString(), "Error: " + jsonResponseObject.errcode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void editGuild()
        {
            HttpResponseMessage response;
            try
            {
                HttpClient request = new() { BaseAddress = new Uri("http://" + serverDetails[0]) };
                response = await request.GetAsync("/api/guild/setDetails?token=" + serverDetails[1] + "&"); // TODO: finish
            }
            catch
            {
                MessageBox.Show("Could not connect to " + serverDetails[0], "Connection Error.");
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (guildID != null)
            {
                editGuild();
            }
            else
            {
                newGuild();
            }
        }
    }
}
