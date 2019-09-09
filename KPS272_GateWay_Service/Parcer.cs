using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KPS272_GateWay_Service
{
    class Parcer
    {
        bool enable = true;

        //public static Int32 _Port;
        public static Int32 port;
        public static IPAddress address;

        #region Start
        private void ThreadProc()
        {
            TcpListener server = null;
            try
            {
                // TcpListener server = new TcpListener(port);
                server = new TcpListener(address, port);

                // Start listening for client requests.
                server.Start();

                // Enter the listening loop.
                while (true)
                {
                    string hexValue, path = @"C:\KPS272_gateway\KPS272_gateway\NameModems.ini";

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
                            String data = null,
                                imei = null;

                            // Get a stream object for reading and writing
                            NetworkStream stream = client.GetStream();

                            int i;

                            // Loop to receive all the data sent by the client.
                            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                // address.ScopeId.ToString();

                                // Translate data bytes to a ASCII string.
                                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                                Console.ForegroundColor = ConsoleColor.Cyan;

                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"    [{DateTime.Now}] receive {i} bytes");

                                hexValue = Package.ToHex(bytes, i);
                                Console.WriteLine(hexValue);


                                //ini file
                                ini.IniWriteValue("Test", "555", "Modem1");
                                //string value = ini.IniReadValue("Test", "Modem1");

                                Console.WriteLine(value);
                                // Console.WriteLine(value);

                                //Package package = new Package();

                                // Console.WriteLine(hexValue);
                                byte[] cut = new byte[i];

                                Array.Copy(bytes, 0, cut, 0, i);

                                Package.Parce(cut);

                                //Package.Parce(new byte[]  { 165});
                                //Check.GetData(hexValue, ip);
                                //hexValue = int.Parse(data.ToString(), System.Globalization.NumberStyles.HexNumber);
                                //Console.WriteLine(hexValue);
                            }

                            ThreadProc();

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
        #endregion

        public bool IsAddressValid(string addressString)
        {
            return IPAddress.TryParse(addressString, out address);
        }

        public void Start()
        {
            
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            Console.WriteLine(string.Format("Version: {0}", assembly.GetName().Version));

            //int number_arg = 0;

            #region CheckValid
            //for (int i = 0; i < 2; i++)
            //{
            //    switch (number_arg)
            //    {
            //        case 0:
            //            try
            //            {

            //                address = IPAddress.Parse()
            //            }
            //            catch (Exception)
            //            {
            //                Console.WriteLine(string.Format("Error in option of IP address [{0}]", arg));
            //                break;
            //            }
            //            break;

            //        case 1:

            //            if (!Int32.TryParse(arg, out port))
            //            {
            //                Console.WriteLine(string.Format("Error in option of Port number[{0}]", arg));
            //                break;
            //            }
            //            break;
            //    }

            //    number_arg++;

            //}

            //if (number_arg != 2)
            //{
            //    Console.WriteLine("Error in runtime options");
            //    Console.WriteLine("Rules: [{0}], [{1}]", "IP address", "Port number");
            //    Console.ReadKey();
            //    Environment.Exit(0);
            //}
            #endregion

            Thread t = new Thread(new ThreadStart(ThreadProc));
            t.Start();

            byte[] text = { 0xA5, 0x8B, 0x00, 0x70, 0x01, 0x00, 0x01, 0x00, 0x38, 0x36, 0x38, 0x39, 0x39, 0x38, 0x30,
                 0x33, 0x37, 0x39, 0x31, 0x33, 0x35, 0x31, 0x30, 0x11, 0x00, 0x02, 0x7F, 0X7F, 0x13, 0x08, 0x14, 0x09, 0x2E, 0x26,
                 0x02, 0x00, 0x00, 0x00, 0xA8, 0x41, 0x03, 0x00, 0x00, 0x00, 0x00, 0x10, 0x03, 0x01, 0x00, 0x00, 0x00, 0x10,
                 0x03, 0x02, 0x00, 0x00, 0x00, 0x10, 0x03, 0x03, 0x00, 0x00, 0x00, 0x10, 0x03, 0x04, 0x00, 0x00, 0x00, 0x10,
                 0x03, 0x05, 0x00, 0x00, 0x00, 0x10, 0x03, 0x06, 0x00, 0x00, 0x00, 0x10, 0x03, 0x07, 0x00, 0x00, 0x00,
                 0x10, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x01, 0x00, 0x00, 0x00, 0x00, 0x05, 0x02, 0x00, 0x00,
                 0x00, 0x00, 0x05, 0x03, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x9A, 0x99, 0xC5, 0x41, 0x04, 0x01, 0x66,
                 0x66, 0x54, 0x42, 0x10, 0x00, 0x63, 0x00, 0x00, 0x00, 0x11, 0x00, 0x02, 0x00, 0x00, 0x00, 0x30, 0x00,
                 0xD7, 0xA3, 0x68, 0x41, 0x12, 0xF2, 0xA5 };

            //package.Parce(text);

            Console.ReadKey();
        }

        public void Stop()
        {
            enable = false;
        }
    }
}
