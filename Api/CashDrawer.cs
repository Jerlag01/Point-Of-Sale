using System;
using System.IO.Ports;

namespace Api.CashDrawerApi
{
    public class CashDrawer : IDisposable
    {
        private SerialPort ActiveSerialPort;
        private string ReceivedDataBuffer;

        public DrawerState DrawerState { get; private set; }

        public SerialSettings SerialSettings { get; set; }

        public CashDrawer(string serialPortName)
        {
            this.DrawerState = DrawerState.Unknown;
            if (string.IsNullOrEmpty(serialPortName))
                throw new ArgumentException("Serial port name cannot be empty");
            this.SerialSettings = new SerialSettings()
            {
                SerialPortName = serialPortName
            };
        }

        public void OpenCashDrawer()
        {
            if (this.ActiveSerialPort == null || !this.ActiveSerialPort.IsOpen)
                return;
            this.ActiveSerialPort.Write("\a");
        }

        public void OpenSerialPort()
        {
            this.ActiveSerialPort = new SerialPort(this.SerialSettings.SerialPortName, this.SerialSettings.BaudRate, this.SerialSettings.Parity, this.SerialSettings.DataBits, this.SerialSettings.StopBits);
            this.ActiveSerialPort.DataReceived += new SerialDataReceivedEventHandler(this.DataReceivedHandler);
            if (this.ActiveSerialPort.IsOpen)
                return;
            this.ActiveSerialPort.Open();
        }

        public void CloseSerialPort()
        {
            if (this.ActiveSerialPort == null || !this.ActiveSerialPort.IsOpen)
                return;
            this.ActiveSerialPort.Close();
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            this.ReceivedDataBuffer += ((SerialPort)sender).ReadExisting();
            if (this.ReceivedDataBuffer.Contains(Constants.CloseReturnValue))
            {
                this.DrawerState = DrawerState.Closed;
                this.ReceivedDataBuffer = string.Empty;
            }
            if (!this.ReceivedDataBuffer.Contains(Constants.OpenReturnValue))
                return;
            this.DrawerState = DrawerState.Open;
            this.ReceivedDataBuffer = string.Empty;
        }

        public void Dispose()
        {
            this.ActiveSerialPort?.Dispose();
        }
    }
}
