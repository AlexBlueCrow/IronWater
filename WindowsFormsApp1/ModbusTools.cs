using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class ModbusTools
    {
        public static float convertor(int value, int type)
        {
            float max = 0, min = 0;

            switch (type)
            {
                case 0x00:
                    max = 15;
                    min = -15;
                    break;
                case 0x01:
                    max = 50;
                    min = -50;
                    break;
                case 0x02:
                    max = 100;
                    min = -100;
                    break;
                case 0x03:
                    max = 500;
                    min = -500;
                    break;
                case 0x04:
                    max = 1000;
                    min = -1000;
                    break;
                case 0x05:
                    max = 2500;
                    min = 0;
                    break;
                case 0x06:
                    max = 20;
                    min = -20;
                    break;
                case 0x07:
                    max = 20;
                    min = 4;
                    break;
                case 0x0E:
                    max = 1100;
                    min = -200;
                    break;
                case 0x0F:
                    max = 1400;
                    min = -250;
                    break;
                case 0x10:
                    max = 400;
                    min = -250;
                    break;
                case 0x11:
                    max = 900;
                    min = -250;
                    break;
                case 0x12:
                    max = 1750;
                    min = 0;
                    break;
                case 0x13:
                    max = 1750;
                    min = 0;
                    break;
                case 0x14:
                    max = 1800;
                    min = 0;
                    break;
                case 0x15:
                    max = 1300;
                    min = -250;
                    break;
                case 0x16:
                    max = 2310;
                    min = 0;
                    break;


            }

            float res = value * (max - min) / 65535 + (min);
            return res;
        }

        public static ushort TypeToCode(string type)
        {
           
            ushort res = 99;
            switch (type)
            {
                
                case "J":
                    res = 0x0E;
                    break;
                case "K":
                    res = 0x0F;
                    break;
                case "T":
                    res = 0x10;
                    break;
                case "E":
                    res = 0x11;
                    break;
                case "R":
                    res = 0x12;
                    break;
                case "S":
                    res = 0x13;
                    break;
                case "B":
                    res = 0x14;
                    break;
                case "N":
                    res = 0x15;
                    break;
                case "W":
                    res = 0x16;
                    break;

            }
            Console.WriteLine(res);
            return res;
        }

        public static void setChanel()
        {
            
        }
    }
}
