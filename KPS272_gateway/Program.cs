using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace KPS272_gateway
{
    class Program
    {
        public static Int32 port;
        public static IPAddress address;

        static void Main(string[] args)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            Console.WriteLine(string.Format("Version: {0}", assembly.GetName().Version));
            
            int number_arg = 0;

            foreach (string arg in args)
            {
                switch (number_arg)
                {
                    case 0:
                        try
                        {
                            address = IPAddress.Parse(arg);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(string.Format("Error in option of IP address [{0}]", arg));
                            break;
                        }
                        break;

                    case 1:

                        if (!Int32.TryParse(arg, out port))
                        {
                            Console.WriteLine(string.Format("Error in option of Port number[{0}]", arg));
                            break;
                        }
                        break;
                }
                number_arg++;
            }

            if (number_arg != 2)
            {
                Console.WriteLine("Error in runtime options");
                Console.WriteLine("Rules: [{0}], [{1}]", "IP address", "Port number");
                Console.ReadKey();
                Environment.Exit(0);
            }

            Receiver receiver = new Receiver(address, port);
            Thread t = new Thread(new ThreadStart(receiver.GetDataFromModem));
            t.IsBackground = true;
            
            t.Start();

            Console.ReadKey();
        }
    }
}
