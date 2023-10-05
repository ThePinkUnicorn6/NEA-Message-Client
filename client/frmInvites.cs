using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeaClient
{
    public partial class frmInvites : Form
    {
        List<string[]> tokens;
        List<string> invites;
        int activeToken;
        Guild guild;
        HttpClient client;
        public frmInvites(Guild guild, List<string[]> tokens, int activeToken)
        {
            InitializeComponent();
            this.tokens = tokens;
            this.activeToken = activeToken;
            this.guild = guild;
            client = new() { BaseAddress = new Uri("http://" + tokens[activeToken][0]) };
        }
        private static void showError(dynamic jsonResponse)
        {
            MessageBox.Show(jsonResponse.error.ToString(), "Error: " + jsonResponse.errcode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private async void btnCreate_Click(object sender, EventArgs e)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                response = await client.GetAsync("/api/guild/createInvite?token=" + tokens[activeToken][1] + "&guildID=" + guild.ID);
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
                if (jsonResponseObject.ContainsKey("errcode"))
                {
                    showError(jsonResponseObject);
                }
                else
                {
                    invites.Add(jsonResponseObject.code);
                    cbInvites.Items.Add(jsonResponseObject.code);
                }
            }
        }
        private async void fetchInvites()
        {

        }
        private void displayInvies()
        {

        }
    }
}
