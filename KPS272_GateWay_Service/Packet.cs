using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KPS272_GateWay_Service
{
    static class Package
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

        private static bool CheckIntegrity(List<byte> list)
        {

            if (list.Count > 1 && list.First() == 165 && list.Last() == 165)
            {
                list.RemoveAt(0);
                list.RemoveAt(list.Count - 1);
                return true;
            }
            else
            {


                return false;
            }
        }

        private static byte GetSize(List<byte> list)
        {
            byte length = list.ElementAt(0);

            if (list.Count > 0 && length != 0)
            {
                list.RemoveAt(0);
            }

            return length;
        }

        private static bool GetUnkownByte(List<byte> list)
        {
            List<byte> unkown4bytes = new List<byte>();

            if (list.Count > 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    unkown4bytes.Add(list[i]);
                }

                if (unkown4bytes.Count == 4)
                {
                    list.RemoveRange(0, 4);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Находим серийный номер
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>9
        private static string GetIMEI(List<byte> list)
        {
            string imei = null;

            if (list.Count > 2 && list[0] == 1 && list[1] == 0)
            {
                list.RemoveRange(0, 2);

                byte[] p = new byte[15];

                //находим серийный номер
                for (int i = 0; i < 15; i++)
                {
                    //serialNumber += list[i];
                    p[i] = list[i];
                }

                Console.WriteLine();
                imei = System.Text.Encoding.ASCII.GetString(p);

                Console.WriteLine($"Серийный номер -  {imei}");

                if (p.Length == 15)
                {
                    list.RemoveRange(0, 15);
                }

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine();

                return imei;
            }
            else
            {
                return " - ";
            }
        }

        /// <summary>
        /// Неиз данные 3 байта
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static bool GetUnkown3Byte(List<byte> list)
        {
            List<byte> unkown3bytes = new List<byte>();

            if (list.Count > 2)
            {
                if (list[0] == 127 && list[1] == 127)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        unkown3bytes.Add(list[i]);
                    }

                    if (unkown3bytes.Count == 3)
                    {
                        list.RemoveRange(0, 3);
                    }

                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// получаем дату
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private static DateTime GetDataTime(List<byte> packet)
        {
            DateTime time_stamp = new DateTime();

            if (packet.Count > 2 && packet[0] == 127 && packet[1] == 127)
            {
                packet.RemoveRange(0, 2);

                List<byte> date = new List<byte>();

                for (int i = 0; i < 6; i++)
                {
                    date.Add(packet[i]);
                }

                time_stamp = new DateTime(date[0], date[1], date[2], date[3], date[4], date[5]);

                Console.Write("Date time - ");
                Console.ForegroundColor = ConsoleColor.Red;


                Console.WriteLine($"{time_stamp.Year}/{time_stamp.Month}/{time_stamp.Day} {time_stamp.Hour}:{time_stamp.Minute}:{time_stamp.Second}");

                if (date.Count == 6)
                {
                    packet.RemoveRange(0, 6);
                }

                return time_stamp;
            }
            else
            {
                DateTime time = new DateTime();
                return time;
            }
        }

        /// <summary>
        /// находим AIO
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private static float GetAI(List<byte> packet, int ch)
        {

            if (packet.Count >= 6 && packet[0] == 2 && packet[1] == ch)
            {

                packet.RemoveRange(0, 2);

                byte[] byteAi = new byte[4];

                for (int i = 0; i < 4; i++)
                {
                    byteAi[i] = packet[i];
                }

                if (byteAi.Length == 4)
                {
                    packet.RemoveRange(0, 4);
                }

                return System.BitConverter.ToSingle(byteAi, 0);
            }
            else
            {
                return 0;
            }
        }

        private static float GetDI(List<byte> packet, int num)
        {
            byte[] f3_0 = new byte[4];


            if (packet.Count > 2 && (packet[0] == 3 && packet[1] == 0) || (packet[0] == 3 && packet[1] == 1) || (packet[0] == 3 && packet[1] == 2) || (packet[0] == 3 && packet[1] == 3) ||
                (packet[0] == 3 && packet[1] == 4) || (packet[0] == 3 && packet[1] == 5) || (packet[0] == 3 && packet[1] == 6) || (packet[0] == 3 && packet[1] == 7))
            {

                packet.RemoveRange(0, 2);

                for (int j = 0; j < 4; j++)
                {
                    f3_0[j] = packet[j];
                }

                if (f3_0.Length == 4)
                {
                    packet.RemoveRange(0, 4);
                }


                return System.BitConverter.ToSingle(f3_0, 0);
            }
            else
            {
                return 0;
            }

        }
        private static byte[] GetUnkownDataFive(List<byte> packet, int num)
        {
            byte[] f3_0 = new byte[4];


            if (packet.Count > 2 && (packet[0] == 5 && packet[1] == 0) || (packet[0] == 5 && packet[1] == 1) || (packet[0] == 5 && packet[1] == 1) ||
                (packet[0] == 5 && packet[1] == 2) || (packet[0] == 5 && packet[1] == 3))
            {

                packet.RemoveRange(0, 2);

                for (int j = 0; j < 4; j++)
                {
                    f3_0[j] = packet[j];
                }

                if (f3_0.Length == 4)
                {
                    packet.RemoveRange(0, 4);
                }
                return f3_0;
            }
            else
            {
                return f3_0;
            }
        }

        private static byte[] GetUnkownData10(List<byte> packet, int num)
        {
            byte[] f3_0 = new byte[4];

            if (packet.Count > 2 && (packet[0] == 10 && packet[1] == 0) || ((packet[0] == 11 || packet[1] == 0)))
            {
                packet.RemoveRange(0, 2);

                for (int j = 0; j < 4; j++)
                {
                    f3_0[j] = packet[j];
                }

                if (f3_0.Length == 4)
                {
                    packet.RemoveRange(0, 4);
                }
            }
            return f3_0;
        }

        private static float GetTemperature(List<byte> packet)
        {

            if (packet.Count > 2 && packet[0] == 4 && packet[1] == 0)
            {
                packet.RemoveRange(0, 2);


                byte[] temp = new byte[4];

                for (int i = 0; i < 4; i++)
                {
                    temp[i] = packet[i];
                }

                float temperature = System.BitConverter.ToSingle(temp, 0);

                if (temp.Length == 4)
                {
                    packet.RemoveRange(0, 4);
                }

                return temperature;
            }
            else
            {
                return 0;
            }

        }
        private static float GetWetness(List<byte> packet)
        {
            List<byte> wetness = new List<byte>();
            List<byte> unkownData = new List<byte>();

            if (packet.Count > 2 && packet[0] == 4 && packet[1] == 1)
            {
                packet.RemoveRange(0, 2);


                byte[] wetness1 = new byte[4];

                for (int i = 0; i < 4; i++)
                {
                    wetness1[i] = packet[i];
                }

                float hudimity = System.BitConverter.ToSingle(wetness1, 0);

                if (wetness1.Length < 4)
                {
                    packet.RemoveRange(0, 4);
                }

                return hudimity;
            }
            else
            {
                return 0;
            }
        }

        private static byte[] GetCRC(List<byte> packet)
        {
            List<byte> unknown2Byte = new List<byte>();
            byte[] crc = new byte[6];

            if (packet.Count > 2 && packet[0] == 30 && packet[1] == 0)
            {
                packet.RemoveRange(0, 2);

                for (int i = 0; i < 6; i++)
                {
                    crc[i] = packet[i];
                }

                if (crc.Length < 6)
                {
                    packet.RemoveRange(0, 6);
                }
                return crc;
            }

            else
            {
                for (int i = 0; i < packet.Count; i++)
                {
                    unknown2Byte.Add(packet[i]);
                }

                return null;

            }
        }

        /// <summary>
        /// запуск всех методов 
        /// </summary>
        /// <param name="packet"></param>
        public static void Parce(byte[] packet)
        {
            List<byte> list = new List<byte>(); //
            List<byte> unknown2Byte = new List<byte>();

            for (int i = 0; i < packet.Length; i++)
            {
                list.Add(packet[i]);
            }
            bool unkown3bytes;

            if (CheckIntegrity(list))
            {
                if ((GetSize(list) > 2))
                {
                    if (GetUnkownByte(list))// первые 4 байта неизвестные данные
                    {
                        string imei = GetIMEI(list);// серийный номер

                        if (list.Count > 2)
                        {
                            unkown3bytes = GetUnkown3Byte(list);

                            DateTime time_stamp = GetDataTime(list); //неизвестный данные (3 байта) после серийного номера
                            float ai0 = GetAI(list, 0);//канал AI0
                            float ai1 = GetAI(list, 1);//канал AI1
                            float ai2 = GetAI(list, 2);//канал AI2
                            float ai3 = GetAI(list, 3);//канал AI3
                            float ai4 = GetAI(list, 4);//канал AI4
                            float ai5 = GetAI(list, 5);//канал AI5

                            float di_0 = GetDI(list, 0);
                            float di_1 = GetDI(list, 1);
                            float di_2 = GetDI(list, 2);
                            float di_3 = GetDI(list, 3);
                            float di_4 = GetDI(list, 4);
                            float di_5 = GetDI(list, 5);
                            float di_6 = GetDI(list, 6);
                            float di_7 = GetDI(list, 7);

                            byte[] f5_0 = GetUnkownDataFive(list, 0);
                            byte[] f5_1 = GetUnkownDataFive(list, 1);
                            byte[] f5_2 = GetUnkownDataFive(list, 2);
                            byte[] f5_3 = GetUnkownDataFive(list, 3);

                            float temperature = GetTemperature(list);
                            float humidity = GetWetness(list);

                            byte[] f10_0 = GetUnkownData10(list, 0);
                            byte[] f11_0 = GetUnkownData10(list, 1);
                            byte[] crc = GetCRC(list);

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"Time - {time_stamp}, IMEI - {imei}, AI - {ai0},{ai1}, {ai2}, {ai3},{ai4}, {ai5}");

                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine($"DI - {Math.Round(di_0, 3)},{Math.Round(di_1, 3)},{Math.Round(di_2, 3)},{Math.Round(di_3, 3)},{Math.Round(di_4, 3)},{Math.Round(di_5, 3)},{Math.Round(di_6, 3)},{Math.Round(di_7, 3)}\n");

                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write($"Температура - {Math.Round(temperature, 2)}");

                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write($"  Влажность - {Math.Round(humidity, 2)} \n");
                        }
                        else
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                unknown2Byte.Add(list[i]);
                            }
                        }
                    }
                }
                else
                {


                }
            }
        }

        private static List<byte> NewData(byte[] packet)
        {
            List<byte> vs = new List<byte>();

            if (packet[0] == 2 && packet[1] == 1)
            {
                for (int i = 0; i < 6; i++)
                {
                    vs.Add(packet[i]);
                }

                return vs;
            }
            else
            {
                return vs;
            }
        }
    }
}
