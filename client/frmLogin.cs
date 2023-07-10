using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
namespace NeaClient
{
    public partial class frmLogin : Form
    {
        public string token;
        public string server;
        public frmLogin()
        {
            InitializeComponent();
        }
        static string hash(string plaintext)
        {
            return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(plaintext)));
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {

        }

        private async void btnCreateAccount_Click(object sender, EventArgs e)
        {
            string passHash = hash(txtPassword.Text);
            HttpResponseMessage response;
            try
            {
                HttpClient request = new() { BaseAddress = new Uri("http://" + txtServerAddress.Text) };
                response = await request.GetAsync("/api/account/create?userName=" + txtUsername.Text + "&password=" + passHash + "&publicKey=''");
            }
            catch
            {
                response = new HttpResponseMessage();
                MessageBox.Show("Could not connect to " + txtServerAddress.Text);
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
                MessageBox.Show("Unknown error: " + jsonResponseObject.errcode);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}