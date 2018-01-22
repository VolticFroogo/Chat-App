using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Chat_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Message.ConnectToServer();

            //Creating thread object to strat it
            Thread readThread = new Thread(Message.ReadBroadcasts);
            readThread.Start();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            Message.Send(InputBox.Text);
            InputBox.Text = "";
        }

        public static void AddMessage(string Message)
        {
            ChatBox.Invoke(new MethodInvoker(delegate { ChatBox.AppendText(Message + "\n"); }));
        }
    }
}
