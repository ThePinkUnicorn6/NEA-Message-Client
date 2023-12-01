using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
namespace NeaClient
{
    public partial class frmLogin : Form
    {
        public string token;
        public string server;
        public HttpClient client;
        public frmLogin()
        {
            InitializeComponent();
        }
        static string hash(string plaintext)
        {
            return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(plaintext)));
        }
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string passHash = hash(txtPassword.Text);
            server = txtServerAddress.Text.Take(7).ToArray() == "http://".ToArray() ? txtServerAddress.Text.Remove(0, 7) : txtServerAddress.Text; // Remove http:// if it is in the url.
            HttpResponseMessage response;
            try
            {
                client = new() { BaseAddress = new Uri("http://" + server) };
                var content = new
                {
                    userName = txtUsername.Text,
                    passHash = passHash,
                };
                response = await client.PostAsJsonAsync("/api/account/login", content);
            }
            catch
            {
                response = new HttpResponseMessage();
                MessageBox.Show("Could not connect to " + txtServerAddress.Text, "Connection Error.");
                return;
            }
            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
            
            if (jsonResponseObject.token is not null)
            {
                token = jsonResponseObject.token.ToString();
                server = txtServerAddress.Text;
                
                Close();
            }
            else
            {
                MessageBox.Show(jsonResponseObject.error.ToString(), "Error: " + jsonResponseObject.errcode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnCreateAccount_Click(object sender, EventArgs e)
        {
            string passHash = hash(txtPassword.Text);
            server = txtServerAddress.Text.ToLower().Take(7).ToArray() == "http://".ToArray() ? txtServerAddress.Text.Remove(0, 7) : txtServerAddress.Text; // Remove http:// if it is in the url.
            server = txtServerAddress.Text.ToLower().Take(7).ToArray() == "https://".ToArray() ? txtServerAddress.Text.Remove(0, 8) : txtServerAddress.Text; //Remove https:// if that is in the url.
            HttpResponseMessage response;
            try
            {
                client = new() { BaseAddress = new Uri("http://" + server) };
                var content = new
                {
                    userName = txtUsername.Text,
                    passHash = passHash,
                    pubicKey = ""
                };
                response = await client.PostAsJsonAsync("/api/account/create", content);
            }
            catch
            {
                response = new HttpResponseMessage();
                MessageBox.Show("Could not connect to " + txtServerAddress.Text, "Connection Error.");
                return;
            }
            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
            if (jsonResponseObject.token is not null)
            {
                token = jsonResponseObject.token.ToString();
                server = txtServerAddress.Text;
                Close();
            }
            else if (jsonResponseObject.errcode == "NAME_IN_USE")
            {
                MessageBox.Show("That username is taken: try adding numbers and symbols to make it unique, or press login if you already have an account.");
            }
            else
            {
                MessageBox.Show(jsonResponseObject.error.ToString(), "Error: " + jsonResponseObject.errcode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}