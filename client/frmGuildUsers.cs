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
    public partial class frmGuildUsers : Form
    {
        List<string[]> tokens;
        Dictionary<string, dynamic> userInfo;
        int activeToken;
        Guild guild;
        HttpClient client;
        public frmGuildUsers(Guild guild, List<string[]> tokens, int activeToken)
        {
            InitializeComponent();
            this.tokens = tokens;
            this.activeToken = activeToken;
            this.guild = guild;
            client = new() { BaseAddress = new Uri("http://" + tokens[activeToken][0]) };
        }
        private async void frmInvites_Load(object sender, EventArgs e)
        {
            await fetchUsers();
            displayUsers();
        }

        private static void showError(dynamic jsonResponse)
        {
            MessageBox.Show(jsonResponse.error.ToString(), "Error: " + jsonResponse.errcode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private async void btnTogglePerms_Click(object sender, EventArgs e)
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
                response = await client.PostAsJsonAsync("/api/guild/users/setPerms", content);
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
                    //inviteCodes.Add(jsonResponseObject.code.ToString());
                    //tblUsers.Items.Add(jsonResponseObject.code.ToString());
                }
            }
        }
        private async Task fetchUsers()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                response = await client.GetAsync("/api/guild/users/list?token=" + tokens[activeToken][1] + "&guildID=" + guild.ID);
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
                    userInfo = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonResponse);
                }
            }
        }
        private void displayUsers()
        {
            tblUsers.Controls.Clear();
            if (userInfo != null && userInfo.Count > 0)
            {
                int i = 0;
                foreach (var user in userInfo)
                {
                    int permission = user.Value.permissions;
                    char badge;

                    if (permission == 4) { badge = 'A'; }
                    else if (permission == 5) { badge = 'O'; }
                    else { badge = ' '; }
                    Label lblBadge = new()
                    {
                        Padding = new Padding(0,6,0,0),
                        Text = badge.ToString()
                    };
                    CheckBox checkBox = new()
                    {
                        Text = user.Value.userName,
                        Tag = user.Key
                    };
                    checkBox.CheckedChanged += new EventHandler(CheckBox_CheckedChanged);
                    tblUsers.Controls.Add(checkBox, 0, i);
                    tblUsers.Controls.Add(lblBadge, 1, i);

                    i++;
                }
            }
        }

        private static void CheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            
        }

        private void btnKick_Click(object sender, EventArgs e)
        {

        }
    }
}
