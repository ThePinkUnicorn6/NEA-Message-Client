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
        User activeUser;
        string guildID;
        Utility utility = new Utility();

        public frmGuildSettings(User activeUser, string guildID = null)
        {
            InitializeComponent();
            this.activeUser = activeUser;
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
            HttpClient client = new() { BaseAddress = new Uri("http://" + activeUser.ServerURL) };
            try
            {
                var content = new
                {
                    Token = activeUser.Token,
                    GuildName = txtGuildName.Text,
                    guildKeyDigest = keyDigest,
                };
                response = await client.PostAsJsonAsync("/api/guild/create", content);            }
            catch
            {
                MessageBox.Show("Could not connect to " + activeUser.ServerURL, "Connection Error.");
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
                            token = activeUser.Token,
                            guildID = guildID,
                            guildDesc = txtGuildDescription.Text
                        };
                        response = await client.PostAsJsonAsync("/api/guild/setDetails", content);
                    }
                                
                    catch
                    {
                        MessageBox.Show("Could not connect to " + activeUser.ServerURL, "Connection Error.");
                        return;
                    }
                }
                if (!File.Exists(keyFile))
                {
                    File.WriteAllText(keyFile, guildID + "," + keyString);
                    Close();
                    return;
                }
                utility.saveKey(guildID, key);
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
                HttpClient request = new() { BaseAddress = new Uri("http://" + activeUser.ServerURL) };
                response = await request.GetAsync("/api/guild/setDetails?token=" + activeUser.Token + "&"); // TODO: finish
            }
            catch
            {
                MessageBox.Show("Could not connect to " + activeUser.ServerURL, "Connection Error.");
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
