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
using System.Net;

namespace NeaClient
{
    public partial class frmGuildSettings : Form
    {
        User activeUser;
        string guildID;
        Utility utility = new Utility();
        HttpClient client;
        public frmGuildSettings(User activeUser, string guildID = null)
        {
            InitializeComponent();
            this.activeUser = activeUser;
            this.guildID = guildID;
            client = new() { BaseAddress = new Uri("http://" + activeUser.ServerURL) };
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
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                response = await client.GetAsync("/api/guild/fetchDetails?token=" + activeUser.Token + "&guildID=" + guildID);
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
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    txtGuildName.Text = jsonResponseObject.Name;
                    txtGuildDescription.Text = jsonResponseObject.Description;
                }
                else
                {
                    MessageBox.Show(jsonResponseObject.error.ToString(), "Error: " + jsonResponseObject.errcode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Could not connect to " + activeUser.ServerURL, "Connection Error.");
            }     
        }
        private async void newGuild()
        {
            if (string.IsNullOrWhiteSpace(txtGuildName.Text))
            {
                MessageBox.Show("Guild name cannot be empty");
                return;
            }
            HttpResponseMessage response;
           
            byte[] key = RandomNumberGenerator.GetBytes(16);
            string keyString = Convert.ToBase64String(key);
            string keyDigest = Convert.ToBase64String(SHA256.HashData(key));
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
            if (string.IsNullOrWhiteSpace(txtGuildName.Text))
            {
                MessageBox.Show("Guild name cannot be empty");
                return;
            }
            try
            {
                HttpClient request = new() { BaseAddress = new Uri("http://" + activeUser.ServerURL) };
                var content = new
                {
                    token = activeUser.Token,
                    guildID = guildID,
                    guildName = txtGuildName.Text,
                    guildDesc = txtGuildDescription.Text
                };
                response = await request.PostAsJsonAsync("/api/guild/setDetails", content);
                if (response.IsSuccessStatusCode)
                {
                    Close();
                }
                else
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    MessageBox.Show(jsonResponseObject.error.ToString(), "Error: " + jsonResponseObject.errcode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
