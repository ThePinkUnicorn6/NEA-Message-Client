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
        Guild guild;
        HttpClient client;
        User activeUser;
        public frmGuildUsers(Guild guild, User activeUser)
        {
            InitializeComponent();
            this.tokens = tokens;
            this.guild = guild;
            this.activeUser = activeUser;
            client = new() { BaseAddress = new Uri("http://" + activeUser.ServerURL) };
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
            for (int i = 0; i <= tblUsers.RowCount; i++)
            {
                CheckBox checkBox = (CheckBox)tblUsers.GetControlFromPosition(0, i);
                if (checkBox.CheckState == CheckState.Checked)
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    bool successfullConnection;
                    try
                    {
                        var content = new
                        {
                            token = activeUser.Token,
                            guildID = guild.ID,
                            userID = checkBox.Tag.ToString(),
                        };
                        response = await client.PostAsJsonAsync("/api/guild/users/togglePerms", content);
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
                        if (jsonResponseObject != null && jsonResponseObject.ContainsKey("errcode"))
                        {
                            showError(jsonResponseObject);
                        }
                        else if (response.IsSuccessStatusCode)
                        {
                            int permissions = userInfo[(string)checkBox.Tag].permissions;
                            if (permissions == 3) permissions++;
                            else if (permissions == 4) permissions--;
                            userInfo[(string)checkBox.Tag].permissions = permissions;
                        }
                    }
                }
            }
            displayUsers();
        }
        private async Task fetchUsers()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                response = await client.GetAsync("/api/guild/users/list?token=" + activeUser.Token + "&guildID=" + guild.ID);
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
                    tblUsers.Controls.Add(checkBox, 0, i);
                    tblUsers.Controls.Add(lblBadge, 1, i);

                    i++;
                }
            }
        }
        private async void btnKick_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < tblUsers.Controls.Count; i++)
            {
                CheckBox checkBox = (CheckBox)tblUsers.GetControlFromPosition(0, i);
                if (checkBox.CheckState == CheckState.Checked)
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    bool successfullConnection;
                    try
                    {
                        var content = new
                        {
                            userID = checkBox.Text,
                            token = activeUser.Token,
                            guildID = guild.ID
                        };
                        response = await client.PostAsJsonAsync("/api/guild/users/kick", content);
                        successfullConnection = true;
                    }
                    catch
                    {
                        successfullConnection = false;
                    }
                }
            }
        }
    }
}
