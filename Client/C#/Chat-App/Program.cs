using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Chat_App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public class Message
    {
        private static TcpClient client;
        private static NetworkStream stream;

        private class Type
        {
            public struct Connect
            {
                public string Name { get; set; } // The actual message
                public string Email { get; set; } // The actual message
            }

            public struct Content
            {
                public int Type { get; set; } // Type of message
                public string Message { get; set; } // The actual message
            }
        }

        public static void Send(string content)
        {
            // Declare message.
            Type.Content message = new Type.Content
            {
                Type = 0,
                Message = content
            };

            // Serialize message to JSON.
            string messageJSON = JsonConvert.SerializeObject(message);

            // Translate JSON into ASCII.
            Byte[] data = Encoding.ASCII.GetBytes(messageJSON);

            // Write data to stream.
            stream.Write(data, 0, data.Length);
        }

        public static void ReadBroadcasts()
        {
            while(true)
            {
                Byte[] data = new Byte[1024];
                Int32 bytes = stream.Read(data, 0, data.Length);

                string messageJSON = Encoding.ASCII.GetString(data);

                JObject msg = (JObject)JsonConvert.DeserializeObject(messageJSON);

                string message = msg["user"] + ": " + msg["message"];
                Form1.AddMessage(message);
            }
        }

        public static void ConnectToServer()
        {
            try
            {
                // Create a TCP Client.
                client = new TcpClient("localhost", 3737);

                // Get a client stream for reading and writing.
                stream = client.GetStream();
                Login();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        private static void Login()
        {
            // Declare message.
            Type.Connect loginInfo = new Type.Connect
            {
                Name = "Froogo",
                Email = "harry@froogo.co.uk"
            };

            // Serialize message to JSON.
            string loginInfoJSON = JsonConvert.SerializeObject(loginInfo);

            // Translate JSON into ASCII.
            Byte[] data = Encoding.ASCII.GetBytes(loginInfoJSON);

            // Write data to stream.
            stream.Write(data, 0, data.Length);
        }
    }
}
