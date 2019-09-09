using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KPS272_gateway
{
    delegate void StartHandler();

    public class Buffer
    {
        private byte[] packet;

        public Buffer(byte[] packet)
        {
            this.packet = packet;
        }

        private bool CheckIntegrity(List<byte> list)
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

        private byte GetSize(List<byte> list)
        {
            byte length = list.ElementAt(0);

            if (list.Count > 0 && length != 0)
            {
                list.RemoveAt(0);
            }

            return length;
        }

        private bool GetUnkownByte(List<byte> list)
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
        /// Находим imei
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>9
        private string GetIMEI(List<byte> list)
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

                imei = System.Text.Encoding.ASCII.GetString(p);
                if (p.Length == 15)
                {
                    list.RemoveRange(0, 15);
                }

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
        private bool GetUnkown3Byte(List<byte> list)
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
        private DateTime GetDataTime(List<byte> packet)
        {
            DateTime time_stamp = new DateTime();

            if (packet.Count > 2 && packet[0] == 127 && packet[1] == 127)
            {
                packet.RemoveRange(0, 2);

                List<int> date = new List<int>();

                for (int i = 0; i < 6; i++)
                {
                    date.Add(packet[i]);
                }

                string year = date[0].ToString();
                //year = year.Remove(0, 1);
                year = 20 + year;
                date[0] = Convert.ToInt32(year);

               
                //time_stamp = new DateTime();
                //time_stamp.AddYears(date[0]);
                //time_stamp.AddMonths(date[1]);
                //time_stamp.AddDays(date[2]);

                //time_stamp.AddHours(date[3]);
                //time_stamp.AddMinutes(date[4]);
                //time_stamp.AddSeconds(date[5]);

                time_stamp = new DateTime(date[0], date[1], date[2], date[3], date[4], date[5]);

                //Console.WriteLine($"{time_stamp.Year}/{time_stamp.Month}/{time_stamp.Day} {time_stamp.Hour}:{time_stamp.Minute}:{time_stamp.Second}");

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
        private float GetAI(List<byte> packet, int ch)
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
     
        private float GetDI(List<byte> packet, int num)
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
        private byte[] GetUnkownDataFive(List<byte> packet, int num)
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
        private byte[] GetUnkownData10(List<byte> packet, int num)
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
        private float GetTemperature(List<byte> packet)
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
        private float GetWetness(List<byte> packet)
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
        private byte[] GetCRC(List<byte> packet)
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
        public void Parce()
        {

            HandlerNew handler;
            try
            {
                List<byte> list = new List<byte>(); 
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

                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine($"\t{time_stamp.ToString("[dd.MM.yyyy hh:mm:ss]")} - [{imei}]");

                                handler = new HandlerNew(imei, time_stamp, f5_0, f5_1, f5_2, f5_3, temperature, humidity, 
                                    ai0, ai1, ai2, ai3, ai4, ai5, 
                                    di_0, di_1, di_2, di_3, di_4, di_5, di_6, di_7);

                                StartHandler start = handler.SaveDataInDb;
                                Thread thread = new Thread(new ThreadStart(start)) { IsBackground = true };
                                thread.Start();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine($"\t{new string(' ', 22)}[{imei}]");

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    struct Packet
    {
        public string imei;

        public DateTime time_stamp;

        public float ai0, ai1, ai2, ai3, ai4, ai5;
        public float di_0, di_1, di_2, di_3, di_4, di_5, di_6, di_7;

        public float temperature, humidity;

        public Packet(float ai0, float ai1, float ai2, float ai3, float ai4, float ai5, 
            float di_0, float di_1, float di_2, float di_3, float di_4, float di_5, float di_6, float di_7,
            string imei, DateTime time, float temperature, float humidity)
        {
            this.ai0 = ai0;
            this.ai1 = ai1;
            this.ai2 = ai2;
            this.ai3 = ai3;
            this.ai4 = ai4;
            this.ai5 = ai5;

            this.di_0 = di_0;
            this.di_1 = di_1;
            this.di_2 = di_2;
            this.di_3 = di_3;
            this.di_4 = di_4;
            this.di_5 = di_5;
            this.di_6 = di_6;
            this.di_7 = di_7;

            this.imei = imei;
            time_stamp = time;
            this.temperature = temperature;
            this.humidity = humidity;
        }
    }
}