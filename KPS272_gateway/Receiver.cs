using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KPS272_gateway
{
    class Receiver
    {
        private IPAddress address;
        private Int32 port;

        public Receiver(IPAddress address, Int32 port)
        {
            this.address = address;
            this.port = port;
        }

        public  void GetDataFromModem()
        {
            TcpListener server = null;
            Buffer buffer;

            try
            {
                // TcpListener server = new TcpListener(port);
                server = new TcpListener(address, port);

                // Start listening for client requests.
                server.Start();

                // Enter the listening loop.
                while (true)
                {
                    string path = @"C:\KPS272_gateway\KPS272_gateway\NameModems.ini";

                    Ini ini = new Ini(path);

                    //ini file
                    string value = ini.IniReadValue("Test", "Modem1");
                    Console.WriteLine(value);

                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(string.Format("Waiting for a connection {0}:{1}", address, port));

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.ForegroundColor = ConsoleColor.Green;

                    Console.WriteLine(string.Format("  Connected client {0}:{1}",
                                                   ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(),
                                                   ((IPEndPoint)client.Client.RemoteEndPoint).Port.ToString()));

                    var childSocketThread = new Thread(() =>
                    {
                        try
                        {
                            // Buffer for reading data
                            Byte[] bytes = new Byte[256];
                            String data = null;

                            // Get a stream object for reading and writing
                            NetworkStream stream = client.GetStream();

                            int i;

                            // Loop to receive all the data sent by the client.
                            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                          
                                // Translate data bytes to a ASCII string.
                                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                               // Console.ForegroundColor = ConsoleColor.Cyan;

                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"\t[{DateTime.Now}] receive {i} bytes");

                                //ini file
                                ini.IniWriteValue("Test", "555", "Modem1");
                                string valueIni = ini.IniReadValue("Test", "Modem1");

                                byte[] cut = new byte[i];
                                Array.Copy(bytes, 0, cut, 0, i);

                                buffer = new Buffer(cut);

                                Thread thread = new Thread(new ThreadStart(buffer.Parce));
                                thread.IsBackground = true;
                                thread.Start();
                            }

                            GetDataFromModem();

                            // Shutdown and end connection
                            //stream.Close();
                            client.Close();
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(e.Message);
                        }

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("  Disconnected");

                    });

                    childSocketThread.Start();
                    Thread.Sleep(0);
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }
        }
    }
}
