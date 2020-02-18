using NLog;
using System;
using System.Collections.Generic;
using Api.CashDrawerApi;

namespace Pos.Services
{
    public static class CashDrawerService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static CashDrawer CashDrawer;

        public static string ComPortName { get; set; }

        public static List<string> SerialPorts
        {
            get
            {
                return SerialHelper.GetSerialPortNames();
            }
        }

        public static bool Connect()
        {
            try
            {
                if (!string.IsNullOrEmpty(CashDrawerService.ComPortName))
                    CashDrawerService.CashDrawer = new CashDrawer(CashDrawerService.ComPortName);
                if (CashDrawerService.CashDrawer == null)
                    return false;
                CashDrawerService.CashDrawer.OpenSerialPort();
                return true;
            }
            catch (Exception ex)
            {
                CashDrawerService.logger.Warn("Could not open serial port for the cashdrawer: " + ex.Message);
                return false;
            }
        }

        public static void Disconnect()
        {
            CashDrawerService.CashDrawer?.CloseSerialPort();
        }

        public static void OpenDrawer()
        {
            CashDrawerService.CashDrawer?.OpenCashDrawer();
        }

        public static DrawerState DrawerState
        {
            get
            {
                return CashDrawerService.CashDrawer != null ? CashDrawerService.CashDrawer.DrawerState : DrawerState.Unknown;
            }
        }
    }
}