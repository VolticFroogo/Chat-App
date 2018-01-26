using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Chat_App
{
    public partial class Chat : Form
    {
        private Thread readThread;

        public Chat()
        {
            InitializeComponent();
        }

        private void Chat_Load(object sender, EventArgs e)
        {
            //Creating thread object to strat it
            readThread = new Thread(Message.ReadBroadcasts);
            readThread.Start();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            Message.Send(InputBox.Text);
            InputBox.Text = "";
        }

        private void Chat_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            readThread.Abort();
            Message.DisconnectFromServer();
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Message.Send(InputBox.Text);
                InputBox.Text = "";
            }
        }

        private void ChatBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        public static void AddMessage(string Message)
        {
            ChatBox.Invoke(new MethodInvoker(delegate {
                // Append new message to the ChatBox.
                ChatBox.AppendText(Message + "\n");
                // Set the current caret position to the end
                ChatBox.SelectionStart = ChatBox.Text.Length;
                // Scroll it automatically
                ChatBox.ScrollToCaret();
            }));
        }
    }
}
