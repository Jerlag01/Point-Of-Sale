using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace Api.CashDrawerApi
{
    public static class SerialHelper
    {
        public static List<string> GetSerialPortNames()
        {
            return ((IEnumerable<string>)SerialPort.GetPortNames()).ToList<string>();
        }
    }
}