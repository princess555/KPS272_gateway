using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Text.RegularExpressions;

namespace KPS272_gateway
{
    static class Check
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

        public static void GetData(string packet)
        {
            try
            {
                string[] words = packet.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string serialNumber = null;

                //серийный номер
                if (packet.StartsWith("0xA5") && packet.EndsWith("0xA5"))
                {
                    int index = Array.IndexOf(words, "0x38");
                    int count = 0;

                    for (int i = index; i < words.Length; i++)
                    {
                        count++;
                        if (count <= 15)
                        {
                            serialNumber += words[i] + " ";
                        }
                        else { break; }
                    }
                            Console.Write(serialNumber);
                }
                Console.WriteLine();

                //time
                if (packet.StartsWith("0xA5") && packet.EndsWith("0xA5"))
                {
                    string pattern = "0x7F";
                    int index = Array.IndexOf(words, pattern);
                    
                    int count = 0;
                    string time = null;
                    string text = null;

                    for (int j = index; j < words.Length; j++)
                    {

                        if (pattern == words[j])
                        {
                            continue;
                        }
                       
                        else if (count <= 6)
                        {
                            count++;
                            time += words[j] + " ";
                            text= time.Remove(0, 4);
                        }
                        else
                        {
                            break;
                        }
                    }
                            Console.WriteLine("time:" + text);
                   
                }
                Console.WriteLine();

                //канал AI0
                if (packet.StartsWith("0xA5") && packet.EndsWith("0xA5"))
                {
                    //
                    int index = Array.IndexOf(words, "0x02");
                    string text = null;
                    int count = 0;

                    for (int i = index; i < words.Length; i++)
                    {
                        if (words[i] == "0x02" || words[i] == "0x00")
                        {
                            if (count < 2)
                            {
                                count++;
                                text += words[i] + " ";
                            }
                            else
                            {
                                if (count < 6)
                                {
                                    count++;
                                    text += words[i] + " ";
                                }
                            }

                        }
                    }

                    text = text.Remove(0, 9);

                    Console.Write("kanal "+ text);
                }
                Console.WriteLine();

                //температура
                if (packet.StartsWith("0xA5") && packet.EndsWith("0xA5"))
                {
                    int index = Array.IndexOf(words, "0x04");
                    string text = null;
                    int count = 0;

                    for (int i = index; i < words.Length; i++)
                    {

                        if  (words[i] == "0x04" || words[i] == "0x00")
                        {
                            if (count < 2)
                            {
                                count++;
                                text += words[i] + " ";
                            }
                            else{


                                text += words[i] + " ";

                            }
                            
                        }
                        else if(words[i] == "0x04")
                        {
                           // if (count < 6)
                            //{
                                count++;
                                text += words[i] + " ";
                            //}
                        }
                        else
                        {
                            text += words[i] + " ";
                        }
                    }

                    text = text.Remove(0, 425);

                    int index2 = Array.LastIndexOf(words, "0x04");

                    for (int i = index2; i < words.Length; i++)
                    {
                        text = words[i];
                    }  
                  

                    Console.Write("температура" + text);
                }

                //влажность 
                if (packet.StartsWith("0xA5") && packet.EndsWith("0xA5"))
                {
                    int index = Array.LastIndexOf(words, "0x04");
                    string text = null;
                    int count = 0;
                    
                    for (int i = index; i < words.Length-1; i++)
                    {
                        
                        if (words[i] == "0x04" || words[i] == "0x01")
                        {
                            if (count < 2)
                            {
                                count++;
                                text += words[i] + " ";
                            }
                            else
                            {
                               
                                    count++;
                                    text += words[i] + " ";
                                
                            }
                            Console.WriteLine(new string('*',5));
                        }
                        else
                        {
                            if(count< 6)
                            {
                                count++;
                                 text += words[i] + " ";
                            }
                            

                        }
                    }

                    text = text.Remove(0, 9);

                    Console.Write("влажность"+text);
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"ex.GetType()", ex.Message);
            }
        }

        public static void ShowArray(String[] arr)
        {
            
        }
    }
}