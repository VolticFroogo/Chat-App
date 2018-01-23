using System;
using System.Windows.Forms;

namespace Chat_App
{
    public partial class Login : Form
    {
        private bool connectedToServer = false;

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            while (!connectedToServer)
            {
                if (Message.ConnectToServer())
                {
                    connectedToServer = true;
                    StatusLabel.Text = "Connected to server!";
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        public void Login_FormClosing(object sender, EventArgs e)
        {
            Message.DisconnectFromServer();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!connectedToServer) return;

            string username = UsernameTextBox.Text,
            email = EmailTextBox.Text;
            if(username == "" || email == "")
            {
                StatusLabel.Text = "Please enter a username and email!";
                return;
            }

            Message.Login(username, email);

            if (!Message.RecieveSuccess())
            {
                StatusLabel.Text = "Invalid login.";
                return;
            }

            Chat chat = new Chat();
            Hide();
            chat.ShowDialog();
            Close();
        }
    }
}
