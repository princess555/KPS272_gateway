using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KPS272_gateway
{
    interface IPipe
    {
        void Send(string SendStr, string PipeName, int TimeOut = 1000);


        //public void SendeData()
        //{
        //    pipesClient = new PipesClient();
        //    pipesClient.Send(imei.ToString() + " " + count.ToString() + " " + time_stamp.ToString() + "Success", "Send Packet", 1000);

        //}
    }
}
