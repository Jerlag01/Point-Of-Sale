using NLog;
using PCSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pos.Services
{
    public class PcscService : IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly SCardContext Context;
        private readonly SCardMonitor Monitor;
        private string selectedReader;

        public event EventHandler CardInserted;

        public event EventHandler CardRemoved;

        public string SelectedReader
        {
            get
            {
                return this.selectedReader;
            }
            set
            {
                if (PcscService.GetReaders().IndexOf(value) == -1)
                    return;
                this.selectedReader = value;
            }
        }

        public string CardId { get; private set; }

        public bool CardPresent
        {
            get
            {
                try
                {
                    using (SCardReader scardReader = new SCardReader((ISCardContext)this.Context))
                        return scardReader.Connect(this.selectedReader, SCardShareMode.Shared, SCardProtocol.Any) == SCardError.Success;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public PcscService(string selectedReader)
        {
            try
            {
                this.SelectedReader = selectedReader;
                this.Context = new SCardContext();
                this.Context.Establish(SCardScope.System);
                this.Monitor = new SCardMonitor((ISCardContext)this.Context, SCardScope.System, true);
                this.Monitor.CardInserted += new CardInsertedEvent(this.CardInsertedHandler);
                this.Monitor.CardRemoved += new CardRemovedEvent(this.CardRemovedHandler);
                this.Monitor.MonitorException += new MonitorExceptionEvent(this.MonitorExceptionHandler);
                this.Monitor.Start(this.SelectedReader);
                if (!this.CardPresent)
                    return;
                this.CardInsertedHandler((object)null, new CardStatusEventArgs());
            }
            catch (Exception ex)
            {
                PcscService.logger.Error("Error while initializing the Pcsc service class: " + ex.Message);
            }
        }

        ~PcscService()
        {
            try
            {
                this.Monitor.CardInserted -= new CardInsertedEvent(this.CardInsertedHandler);
                this.Monitor.CardRemoved -= new CardRemovedEvent(this.CardRemovedHandler);
                this.Monitor.MonitorException -= new MonitorExceptionEvent(this.MonitorExceptionHandler);
                this.Monitor.Cancel();
                this.Monitor.Dispose();
                this.Context.Dispose();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                // ISSUE: explicit finalizer call
                base.Finalize();
            }
        }

        public byte[] SendApdu(byte[] apdu)
        {
            byte[] receiveBuffer = new byte[263];
            using (SCardReader scardReader = new SCardReader((ISCardContext)this.Context))
            {
                SCardError code1 = scardReader.Connect(this.selectedReader, SCardShareMode.Shared, SCardProtocol.Any);
                if (code1 == SCardError.Success)
                {
                    SCardError code2 = scardReader.Transmit(apdu, ref receiveBuffer);
                    if (code2 == SCardError.Success)
                        return receiveBuffer;
                    PcscService.logger.Warn("Error while sending APDU to smartcard. Error message: " + SCardHelper.StringifyError(code2));
                    return (byte[])null;
                }
                PcscService.logger.Warn("No card inserted or reader is reserved exclusively by another application. Error message: " + SCardHelper.StringifyError(code1));
                return (byte[])null;
            }
        }

        public static List<string> GetReaders()
        {
            List<string> list;
            using (SCardContext scardContext = new SCardContext())
            {
                scardContext.Establish(SCardScope.System);
                list = ((IEnumerable<string>)scardContext.GetReaders()).ToList<string>();
                scardContext.Release();
            }
            if (list.Count == 0)
                PcscService.logger.Warn("There are no PCSC smartcard readers connected.");
            return list;
        }

        private static string ByteArrayToString(ICollection<byte> ba)
        {
            if (ba == null)
                return string.Empty;
            StringBuilder stringBuilder = new StringBuilder(ba.Count * 2);
            foreach (byte num in (IEnumerable<byte>)ba)
                stringBuilder.AppendFormat("{0:x2}", (object)num);
            return stringBuilder.ToString();
        }

        private void CardInsertedHandler(object sender, CardStatusEventArgs args)
        {
            this.Acr122DisableBuzzerOnNewCard();
            this.CardId = this.GetCardId();
            this.OnCardInserted(EventArgs.Empty);
            PcscService.logger.Debug("NFC card placed on reader, cardID: " + this.CardId);
        }

        private void CardRemovedHandler(object sender, CardStatusEventArgs args)
        {
            this.CardId = string.Empty;
            EventHandler cardRemoved = this.CardRemoved;
            if (cardRemoved != null)
                cardRemoved((object)this, EventArgs.Empty);
            this.OnCardRemoved(EventArgs.Empty);
            PcscService.logger.Debug("NFC card removed from reader");
        }

        private void MonitorExceptionHandler(object sender, PCSCException ex)
        {
            PcscService.logger.Error("PCSC Monitor exited due an error: " + SCardHelper.StringifyError(ex.SCardError));
        }

        protected virtual void OnCardInserted(EventArgs e)
        {
            EventHandler cardInserted = this.CardInserted;
            if (cardInserted == null)
                return;
            cardInserted((object)this, e);
        }

        protected virtual void OnCardRemoved(EventArgs e)
        {
            EventHandler cardRemoved = this.CardRemoved;
            if (cardRemoved == null)
                return;
            cardRemoved((object)this, e);
        }

        public string GetCardId()
        {
            return PcscService.ByteArrayToString((ICollection<byte>)this.SendApdu(new byte[5]
            {
        byte.MaxValue,
        (byte) 202,
        (byte) 0,
        (byte) 0,
        (byte) 0
            }));
        }

        public void Acr122DisableBuzzerOnNewCard()
        {
            this.SendApdu(new byte[5]
            {
        byte.MaxValue,
        (byte) 0,
        (byte) 82,
        (byte) 0,
        (byte) 0
            });
        }

        public void Acr122BlinkLed()
        {
            this.SendApdu(new byte[9]
            {
        byte.MaxValue,
        (byte) 0,
        (byte) 64,
        (byte) 213,
        (byte) 4,
        (byte) 1,
        (byte) 1,
        (byte) 3,
        (byte) 1
            });
        }

        public void Dispose()
        {
            this.Monitor.CardInserted -= new CardInsertedEvent(this.CardInsertedHandler);
            this.Monitor.CardRemoved -= new CardRemovedEvent(this.CardRemovedHandler);
            this.Monitor.MonitorException -= new MonitorExceptionEvent(this.MonitorExceptionHandler);
            this.Monitor.Cancel();
            this.Monitor.Dispose();
            this.Context.Dispose();
        }
    }
}
