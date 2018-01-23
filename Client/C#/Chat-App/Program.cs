using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
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
            Application.Run(new Login());
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

            public enum ID {
                message,
                connected,
                disconnected,
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
            while (true)
            {
                Byte[] data = new Byte[1024];
                Int32 bytes = stream.Read(data, 0, data.Length);

                string messageJSON = Encoding.ASCII.GetString(data);

                JObject msg = (JObject)JsonConvert.DeserializeObject(messageJSON);

                switch(msg["type"].ToObject<int>())
                {
                    case (int) Type.ID.message:
                        string message = msg["user"] + ": " + msg["message"];
                        Chat.AddMessage(message);
                        break;

                    case (int) Type.ID.connected:
                        message = msg["user"] + " has connected!";
                        Chat.AddMessage(message);
                        break;

                    case (int)Type.ID.disconnected:
                        message = msg["user"] + " has disconnected.";
                        Chat.AddMessage(message);
                        break;
                }
            }
        }

        public static bool RecieveSuccess()
        {
            Byte[] data = new Byte[32];
            Int32 bytes = stream.Read(data, 0, data.Length);

            string messageJSON = Encoding.ASCII.GetString(data);

            JObject msg = (JObject)JsonConvert.DeserializeObject(messageJSON);

            return msg["success"].ToObject<bool>();
        }

        public static bool ConnectToServer()
        {
            try
            {
                // Get server IP from DNS
                var address = Dns.GetHostAddresses("chat.froogo.co.uk")[0];

                // Create a TCP Client.
                client = new TcpClient(address.ToString(), 3737);

                // Get a client stream for reading and writing.
                stream = client.GetStream();
                return true;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            return false;
        }

        public static void DisconnectFromServer()
        {
            stream.Close();
            client.Close();
        }

        public static void Login(string name, string email)
        {
            // Declare message.
            Type.Connect loginInfo = new Type.Connect
            {
                Name = name,
                Email = email
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
