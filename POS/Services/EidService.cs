using Swelio.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using Dal.Model;

namespace Pos.Services
{
    public class EidService : IDisposable
    {
        private Manager CardManager;
        private CardReader CardReader;
        private string selectedReader;

        public event EventHandler EidInserted;

        public event EventHandler EidRemoved;

        public EidService.EidDataFields EidData { get; private set; }

        public static List<string> ReaderList
        {
            get
            {
                return ((IEnumerable<string>)new Manager()
                {
                    Active = true
                }.ListReaders()).ToList<string>();
            }
        }

        public string SelectedReader
        {
            get
            {
                return this.selectedReader;
            }
            set
            {
                string[] strArray = this.CardManager.ListReaders();
                if (strArray == null)
                    return;
                int readerNumber = ((IEnumerable<string>)strArray).ToList<string>().IndexOf(value);
                if (readerNumber == -1)
                    return;
                this.CardManager.SetDefaultReader(readerNumber);
                this.selectedReader = value;
            }
        }

        public EidService()
        {
            this.CardManager = new Manager() { Active = true };
            this.CardManager.CardInserted += new EventHandler<CardEventArgs>(this.CardInserted);
            this.CardManager.CardRemoved += new EventHandler<CardEventArgs>(this.CardRemoved);
            this.CardManager.TraceEvents = true;
        }

        public void ReadEid()
        {
            this.CardReader = this.CardManager.GetReader();
            if (this.CardReader == null)
            {
                this.EidData = (EidService.EidDataFields)null;
            }
            else
            {
                Card card = this.CardReader.GetCard(true);
                if (card != null && !card.Inserted)
                {
                    this.CardManager = new Manager() { Active = true };
                    int readerNumber = ((IEnumerable<string>)this.CardManager.ListReaders()).ToList<string>().IndexOf(this.selectedReader);
                    if (readerNumber != -1)
                        this.CardManager.SetDefaultReader(readerNumber);
                    this.CardManager.CardInserted += new EventHandler<CardEventArgs>(this.CardInserted);
                    this.CardManager.CardRemoved += new EventHandler<CardEventArgs>(this.CardRemoved);
                    this.CardManager.TraceEvents = true;
                    this.CardReader = this.CardManager.GetReader();
                    card = this.CardReader.GetCard(true);
                }
                if (card != null)
                {
                    Identity identity = card.ReadIdentity();
                    Address address = card.ReadAddress();
                    this.EidData = new EidService.EidDataFields();
                    if (identity == null || address == null)
                        return;
                    this.EidData.Address = address.Street;
                    this.EidData.BirthDate = identity.BirthDate;
                    this.EidData.City = address.Municipality;
                    this.EidData.ZipCode = address.Zip;
                    this.EidData.CountryIsoCode = identity.NationalityISO;
                    this.EidData.FirstName = identity.FirstName1;
                    this.EidData.LastName = identity.Surname;
                    this.EidData.RRNumber = identity.NationalNumber;
                    this.EidData.Gender = identity.Sex.ToString() == "Male" ? Member.Genders.Male : Member.Genders.Female;
                }
                else
                    this.EidData = (EidService.EidDataFields)null;
            }
        }

        protected virtual void OnEidInserted(EventArgs e)
        {
            EventHandler eidInserted = this.EidInserted;
            if (eidInserted == null)
                return;
            eidInserted((object)this, e);
        }

        protected virtual void OnEidRemoved(EventArgs e)
        {
            EventHandler eidRemoved = this.EidRemoved;
            if (eidRemoved == null)
                return;
            eidRemoved((object)this, e);
        }

        private void CardInserted(object o, CardEventArgs cea)
        {
            this.OnEidInserted(EventArgs.Empty);
        }

        private void CardRemoved(object o, CardEventArgs cea)
        {
            this.EidData = new EidService.EidDataFields();
            this.OnEidRemoved(EventArgs.Empty);
        }

        public void Dispose()
        {
            this.CardManager.CardInserted -= new EventHandler<CardEventArgs>(this.CardInserted);
            this.CardManager.CardRemoved -= new EventHandler<CardEventArgs>(this.CardRemoved);
            this.CardManager.Dispose();
            this.CardReader.Dispose();
        }

        public class EidDataFields
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string ZipCode { get; set; }

            public string City { get; set; }

            public string Address { get; set; }

            public DateTime BirthDate { get; set; }

            public string CountryIsoCode { get; set; }

            public Member.Genders Gender { get; set; }

            public string RRNumber { get; set; }
        }
    }
}
