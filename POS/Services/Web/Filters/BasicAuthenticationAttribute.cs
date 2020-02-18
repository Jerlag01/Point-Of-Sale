using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using Pos.Services.Web.Results;

namespace Pos.Services.Web.Filters
{
    public abstract class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter, IFilter
    {
        public string Realm { get; set; }

        public async Task AuthenticateAsync(
          HttpAuthenticationContext context,
          CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;
            if (authorization == null || authorization.Scheme != "Basic")
                return;
            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = (IHttpActionResult)new AuthenticationFailureResult("Missing credentials", request);
            }
            else
            {
                Tuple<string, string> userNameAndPassword = BasicAuthenticationAttribute.ExtractUserNameAndPassword(authorization.Parameter);
                if (userNameAndPassword == null)
                {
                    context.ErrorResult = (IHttpActionResult)new AuthenticationFailureResult("Invalid credentials", request);
                }
                else
                {
                    IPrincipal principal = await this.AuthenticateAsync(userNameAndPassword.Item1, userNameAndPassword.Item2, cancellationToken);
                    if (principal == null)
                        context.ErrorResult = (IHttpActionResult)new AuthenticationFailureResult("Invalid username or password", request);
                    else
                        context.Principal = principal;
                }
            }
        }

        protected abstract Task<IPrincipal> AuthenticateAsync(
          string userName,
          string password,
          CancellationToken cancellationToken);

        private static Tuple<string, string> ExtractUserNameAndPassword(
          string authorizationParameter)
        {
            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(authorizationParameter);
            }
            catch (FormatException ex)
            {
                return (Tuple<string, string>)null;
            }
            Encoding encoding = (Encoding)Encoding.ASCII.Clone();
            encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
            string str;
            try
            {
                str = encoding.GetString(bytes);
            }
            catch (DecoderFallbackException ex)
            {
                return (Tuple<string, string>)null;
            }
            if (string.IsNullOrEmpty(str))
                return (Tuple<string, string>)null;
            int length = str.IndexOf(':');
            return length == -1 ? (Tuple<string, string>)null : new Tuple<string, string>(str.Substring(0, length), str.Substring(length + 1));
        }

        public Task ChallengeAsync(
          HttpAuthenticationChallengeContext context,
          CancellationToken cancellationToken)
        {
            this.Challenge(context);
            return (Task)Task.FromResult<int>(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = !string.IsNullOrEmpty(this.Realm) ? "realm=\"" + this.Realm + "\"" : (string)null;
            context.ChallengeWith("Basic", parameter);
        }

        public virtual bool AllowMultiple
        {
            get
            {
                return false;
            }
        }
    }
}
