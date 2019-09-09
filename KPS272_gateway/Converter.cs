using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPS272_gateway
{
    static class Converter
    {
        public static string ToHex(this byte[] input, int len)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < len; i++)
            {
                sb.AppendFormat("0x{0:X2} ", input[i]);
            }
            return sb.ToString().Trim();
        }
    }
}
