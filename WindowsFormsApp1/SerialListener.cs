using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace WindowsFormsApp1
{
    

    public class SerialListener()
    {
        #region port listening

        private void SerialPort serialPort = null;

        private void StartMonitor()
        {
            List<string> comList = GetComlist(false);
            if (comList.Count == 0)
            {
                
            }
        }
        
        
        
    }
}
