using System.IO.Ports;

namespace Api.CashDrawerApi
{
    public class SerialSettings
    {
        public readonly int BaudRate = 9600;
        public readonly int DataBits = 8;
        public readonly StopBits StopBits = StopBits.One;
        public string SerialPortName;
        public readonly Parity Parity;
    }
}