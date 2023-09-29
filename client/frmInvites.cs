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
        string token;
        public Guild guild;
        public frmInvites(Guild guild, string token)
        {
            InitializeComponent();
            this.token = token;
            this.guild = guild;
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
                response = await client.GetAsync("/api/guild/createInvite?token=" + token + "&guildID=" + guildID);
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
            }
        }
    }
}
