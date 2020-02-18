using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Dal.Model;
using Util.MicroMvvm;

namespace Pos.ViewModels
{
    public class TicketViewModel : ObservableObject
    {
        private Ticket ticket;
        private bool newTicket;
        private Visibility visibility;
        private TicketLine selectedTicketLine;
        private bool editMode;

        public Ticket Ticket
        {
            get
            {
                return this.ticket;
            }
            set
            {
                this.ticket = value;
                this.OnPropertyChanged<Ticket>((System.Linq.Expressions.Expression<Func<Ticket>>)(() => this.Ticket));
            }
        }

        public bool NewTicket
        {
            get
            {
                return this.newTicket;
            }
            set
            {
                this.newTicket = value;
                this.OnPropertyChanged<bool>((System.Linq.Expressions.Expression<Func<bool>>)(() => this.NewTicket));
            }
        }

        public Visibility Visibility
        {
            get
            {
                return this.visibility;
            }
            set
            {
                this.visibility = value;
                this.OnPropertyChanged<Visibility>((System.Linq.Expressions.Expression<Func<Visibility>>)(() => this.Visibility));
            }
        }

        public TicketLine SelectedTicketLine
        {
            get
            {
                return this.selectedTicketLine;
            }
            set
            {
                this.selectedTicketLine = value;
                this.OnPropertyChanged<TicketLine>((System.Linq.Expressions.Expression<Func<TicketLine>>)(() => this.SelectedTicketLine));
            }
        }

        public bool EditMode
        {
            get
            {
                return this.editMode;
            }
            set
            {
                this.editMode = value;
                this.OnPropertyChanged<bool>((System.Linq.Expressions.Expression<Func<bool>>)(() => this.EditMode));
            }
        }

        public ICommand ItemSelectedCommand { get; set; }

        public ICommand PlusCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.PlusCommandExec));
            }
        }

        public ICommand MinusCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.MinusCommandExec));
            }
        }

        public ICommand TicketControlClicked { get; set; }

        private void PlusCommandExec()
        {
            if (this.SelectedTicketLine == null || this.Ticket == null || this.Ticket.TicketLines == null)
                return;
            TicketLine ticketLine = this.Ticket.TicketLines.FirstOrDefault<TicketLine>((Func<TicketLine, bool>)(t => t.Product == this.selectedTicketLine.Product));
            if (ticketLine != null)
                ++ticketLine.Amount;
            this.Ticket.RecalculatePrice();
            this.Ticket.TotalRemaining = this.Ticket.TotalPrice;
            this.OnPropertyChanged<Ticket>((System.Linq.Expressions.Expression<Func<Ticket>>)(() => this.Ticket));
        }

        private void MinusCommandExec()
        {
            if (this.SelectedTicketLine == null || this.Ticket == null || this.Ticket.TicketLines == null)
                return;
            TicketLine ticketLine = this.Ticket.TicketLines.FirstOrDefault<TicketLine>((Func<TicketLine, bool>)(t => t.Product == this.selectedTicketLine.Product));
            if (ticketLine == null)
                return;
            --ticketLine.Amount;
            if (ticketLine.Amount <= (short)0)
                this.Ticket.TicketLines.Remove(this.SelectedTicketLine);
            this.Ticket.RecalculatePrice();
            this.Ticket.TotalRemaining = this.Ticket.TotalPrice;
            this.OnPropertyChanged<Ticket>((System.Linq.Expressions.Expression<Func<Ticket>>)(() => this.Ticket));
        }
    }
}
