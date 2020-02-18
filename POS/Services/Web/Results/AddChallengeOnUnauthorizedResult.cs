using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Pos.Services.Web.Results
{
    public class AddChallengeOnUnauthorizedResult : IHttpActionResult
    {
        public AddChallengeOnUnauthorizedResult(
            AuthenticationHeaderValue challenge,
            IHttpActionResult innerResult)
        {
            this.Challenge = challenge;
            this.InnerResult = innerResult;
        }

        public AuthenticationHeaderValue Challenge { get; private set; }

        public IHttpActionResult InnerResult { get; private set; }

        public async Task<HttpResponseMessage> ExecuteAsync(
            CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponseMessage = await this.InnerResult.ExecuteAsync(cancellationToken);
            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized && httpResponseMessage.Headers.WwwAuthenticate.All<AuthenticationHeaderValue>((Func<AuthenticationHeaderValue, bool>)(h => h.Scheme != this.Challenge.Scheme)))
                httpResponseMessage.Headers.WwwAuthenticate.Add(this.Challenge);
            return httpResponseMessage;
        }
    }
}