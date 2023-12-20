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
using System.Net.Http.Json;

namespace NeaClient
{
    public partial class frmInvites : Form
    {
        List<string[]> tokens;
        List<string> inviteCodes;
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
        private async void frmInvites_Load(object sender, EventArgs e)
        {
            await fetchInvites();
            displayInvites();
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
                var content = new
                {
                    token = tokens[activeToken][1],
                    guildID = guild.ID
                };
                response = await client.PostAsJsonAsync("/api/guild/createInvite", content);
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
                    inviteCodes.Add(jsonResponseObject.code.ToString());
                    cbInvites.Items.Add(jsonResponseObject.code.ToString());
                }
            }
        }
        private async Task fetchInvites()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                response = await client.GetAsync("/api/guild/listInvites?token=" + tokens[activeToken][1] + "&guildID=" + guild.ID);
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
                    Close();
                }
                else
                {
                    inviteCodes = new List<string>(jsonResponseObject.inviteCodes.ToObject<string[]>());
                }
            }
        }
        private void displayInvites()
        {
            cbInvites.Items.Clear();
            if (inviteCodes != null && inviteCodes.Count > 0)
            {
                foreach (string code in inviteCodes)
                {
                    cbInvites.Items.Add(code);
                }
            }
        }
        private void cbInvites_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clipboard.SetText((string)cbInvites.SelectedItem);
        }
    }
}
