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
        public async void newGuild()
        {
            HttpResponseMessage response;
            try
            {
                HttpClient request = new() { BaseAddress = new Uri("http://" + serverDetails[0]) };
                response = await request.GetAsync("/api/guild/create?token=" + serverDetails[1] + "&guildName=" + txtGuildName.Text);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    response = await request.GetAsync("/api/guild/setDetails?token=" + serverDetails[1] + "&guildID=" + jsonResponseObject.GuildID.ToString() + "&guildDesc=" + txtGuildDescription.Text);
                    Close();
                }
                else
                {
                    MessageBox.Show("Failed to create guild, error from server: " + jsonResponse);
                }
            }
            catch
            {
                response = new HttpResponseMessage();
                MessageBox.Show("Could not connect to " + serverDetails[0]);
            }
        }
        public async void editGuild()
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
                MessageBox.Show("Could not connect to " + serverDetails[0]);
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
