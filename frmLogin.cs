using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
namespace NeaClient
{
    public partial class frmLogin : Form
    {
        public string token;
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
                MessageBox.Show("Could not connect to server: " + txtServerAddress.Text);
                return;                    
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
            
            if (jsonResponseObject.token is not null)
            {
                token = jsonResponseObject.token.ToString();
                this.Close();
            }
        }
    }
}