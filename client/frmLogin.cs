using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
namespace NeaClient
{
    public partial class frmLogin : Form
    {
        public User user;
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
                user = new User
                {
                    Token = jsonResponseObject.token.ToString()
                };
                // TODO: ask user to enter a backup of the private key, or generate a new key if the old one is lost
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
            HttpResponseMessage response;
            try
            {
                //TODO: Check adress contains no commas
                client = new() { BaseAddress = new Uri("http://" + server) };
                user = new User
                {
                    Name = txtUsername.Text,
                };
                user.GenerateKeys();
                var content = new
                {
                    userName = user.Name,
                    passHash = passHash,
                    publicKey = user.PublicKey
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
                user.Token = jsonResponseObject.token.ToString();
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