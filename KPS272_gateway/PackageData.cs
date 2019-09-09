using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPS272_gateway
{
    public class PackageData
    {
        public int ID { get; set; }
        public int ModemID { get; set; }
        public string SerialNumber { get; set; }
        public DateTime time { get; set; }
        public float AiO { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
    }
}
