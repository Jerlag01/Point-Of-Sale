using NLog;
using System;
using System.Timers;
using Dal;
using Dal.Model;

namespace Pos.Services
{
    public sealed class AuthenticationService
    {
        private static readonly Lazy<AuthenticationService> lazy = new Lazy<AuthenticationService>((Func<AuthenticationService>)(() => new AuthenticationService()));
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private PcscService PcscService;
        private Timer AuthTimer;

        public static AuthenticationService Instance
        {
            get
            {
                return AuthenticationService.lazy.Value;
            }
        }

        private AuthenticationService()
        {
            AuthenticationService.logger.Debug("Authentication Service instance created");
        }

        public bool SuspendService { get; set; }

        public Member AuthenticatedMember { get; private set; }

        public Member PreviousAuthenticatedMember { get; private set; }

        public event AuthenticationService.AuthenticationEventHandler MemberAuthenticated;

        public event AuthenticationService.AuthenticationEventHandler MemberDeauthenticated;

        public void Inititialize(PcscService service, int authenticationTimeout)
        {
            this.AuthTimer = new Timer((double)authenticationTimeout);
            this.AuthTimer.AutoReset = false;
            this.AuthTimer.Elapsed += new ElapsedEventHandler(this.TimeElapsed);
            this.PcscService = service;
            this.PcscService.CardInserted += new EventHandler(this.SmartcardInserted);
            this.PcscService.CardRemoved += new EventHandler(this.SmartcardRemoved);
            if (this.PcscService.CardPresent && !string.IsNullOrEmpty(this.PcscService.CardId))
                this.SmartcardInserted((object)null, EventArgs.Empty);
            this.SuspendService = false;
        }

        private void SmartcardInserted(object o, EventArgs e)
        {
            if (this.SuspendService)
                return;
            this.AuthTimer.Stop();
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    if (this.PcscService.CardId == null)
                    {
                        this.Deauthenticate();
                        this.PcscService.Acr122BlinkLed();
                    }
                    else
                    {
                        MemberCard smartcardIdNoTracking = unitOfWork.MemberCardRepository.GetBySmartcardIdNoTracking(this.PcscService.CardId);
                        if (smartcardIdNoTracking != null && smartcardIdNoTracking.Member != null && smartcardIdNoTracking.Member?.Roles != null)
                        {
                            Member member = smartcardIdNoTracking.Member;
                            int num1;
                            if (member == null)
                            {
                                num1 = 0;
                            }
                            else
                            {
                                int? count = member.Roles?.Count;
                                int num2 = 0;
                                num1 = count.GetValueOrDefault() == num2 ? (count.HasValue ? 1 : 0) : 0;
                            }
                            if (num1 == 0)
                            {
                                this.AuthenticatedMember = smartcardIdNoTracking.Member;
                                if (this.AuthenticatedMember == null)
                                    return;
                                this.OnMemberAuthenticated(new AuthenticationService.AuthenticationEventArgs(this.AuthenticatedMember));
                                return;
                            }
                        }
                        this.Deauthenticate();
                        this.PcscService.Acr122BlinkLed();
                    }
                }
            }
            catch (Exception ex)
            {
                this.AuthenticatedMember = (Member)null;
                this.Deauthenticate();
            }
        }

        private void SmartcardRemoved(object o, EventArgs e)
        {
            this.AuthTimer.Start();
        }

        private void TimeElapsed(object o, EventArgs e)
        {
            if (!this.SuspendService)
            {
                this.Deauthenticate();
                this.AuthTimer.Stop();
            }
            else
            {
                this.AuthTimer.Stop();
                this.AuthTimer.Start();
            }
        }

        private void Deauthenticate()
        {
            this.OnMemberDeauthenticated(new AuthenticationService.AuthenticationEventArgs(this.AuthenticatedMember));
            this.PreviousAuthenticatedMember = this.AuthenticatedMember;
            this.AuthenticatedMember = (Member)null;
        }

        private void OnMemberAuthenticated(AuthenticationService.AuthenticationEventArgs e)
        {
            AuthenticationService.AuthenticationEventHandler memberAuthenticated = this.MemberAuthenticated;
            if (memberAuthenticated == null)
                return;
            memberAuthenticated((object)this, e);
        }

        private void OnMemberDeauthenticated(AuthenticationService.AuthenticationEventArgs e)
        {
            AuthenticationService.AuthenticationEventHandler memberDeauthenticated = this.MemberDeauthenticated;
            if (memberDeauthenticated == null)
                return;
            memberDeauthenticated((object)this, e);
        }

        public delegate void AuthenticationEventHandler(
          object sender,
          AuthenticationService.AuthenticationEventArgs e);

        public class AuthenticationEventArgs : EventArgs
        {
            public Member AuthenticatedMember { get; set; }

            public AuthenticationEventArgs(Member authenticatedMember)
            {
                this.AuthenticatedMember = authenticatedMember;
            }
        }
    }
}
