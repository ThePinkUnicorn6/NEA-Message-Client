using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

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

        }
        private async void newGuild()
        {
            HttpResponseMessage response;
            try
            {
                HttpClient request = new() { BaseAddress = new Uri("http://" + serverDetails[0]) };
                response = await request.GetAsync("/api/guild/create?token=" + serverDetails[1] + "&guildName=" + txtGuildName.Text);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrWhiteSpace(txtGuildDescription.Text)) // If description is not empty, send it to server.
                    {
                        response = await request.GetAsync("/api/guild/setDetails?token=" + serverDetails[1] + "&guildID=" + jsonResponseObject.GuildID.ToString() + "&guildDesc=" + txtGuildDescription.Text);
                    }
                    Close();
                }
                else
                {
                    MessageBox.Show(jsonResponseObject.error.ToString(), "Error: " + jsonResponseObject.errcode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                response = new HttpResponseMessage();
                MessageBox.Show("Could not connect to " + serverDetails[0], "Connection Error.");
            }
        }
        private async void editGuild()
        {
            HttpResponseMessage response;
            try
            {
                HttpClient request = new() { BaseAddress = new Uri("http://" + serverDetails[0]) };
                response = await request.GetAsync("/api/guild/setDetails?token=" + serverDetails[1] + "&");
            }
            catch
            {
                response = new HttpResponseMessage();
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
