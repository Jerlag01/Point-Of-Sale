using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Pos.Services.Web.Properties;
using Util;

namespace Pos.Services.Web.Filters
{
    public class DigestAuthenticationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
          HttpRequestMessage request,
          CancellationToken cancellationToken)
        {
            try
            {
                if (request.Headers.Authorization != null)
                {
                    Header header = new Header(request.Headers.Authorization.Parameter, request.Method.Method);
                    if (Nonce.IsValid(header.Nonce, header.NounceCounter) && Settings.Default.RemoteLogin == header.UserName)
                    {
                        string remotePassword = Settings.Default.RemotePassword;
                        string md5Hash1 = string.Format("{0}:{1}:{2}", (object)header.UserName, (object)header.Realm, (object)remotePassword).ToMD5Hash();
                        string md5Hash2 = string.Format("{0}:{1}", (object)header.Method, (object)header.Uri).ToMD5Hash();
                        string md5Hash3 = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", (object)md5Hash1, (object)header.Nonce, (object)header.NounceCounter, (object)header.Cnonce, (object)"auth", (object)md5Hash2).ToMD5Hash();
                        if (string.CompareOrdinal(header.Response, md5Hash3) == 0)
                        {
                            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal((IEnumerable<ClaimsIdentity>)new ClaimsIdentity[1]
                            {
                new ClaimsIdentity((IEnumerable<Claim>) new List<Claim>()
                {
                  new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", header.UserName),
                  new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password")
                }, "Digest")
                            });
                            Thread.CurrentPrincipal = (IPrincipal)claimsPrincipal;
                            request.GetRequestContext().Principal = (IPrincipal)claimsPrincipal;
                        }
                    }
                }
                return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>((Func<Task<HttpResponseMessage>, HttpResponseMessage>)(task =>
                {
                    HttpResponseMessage result = task.Result;
                    if (result.StatusCode == HttpStatusCode.Unauthorized)
                        result.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Digest", Header.UnauthorizedResponseHeader.ToString()));
                    return result;
                }), cancellationToken);
            }
            catch (Exception ex)
            {
                return Task<HttpResponseMessage>.Factory.StartNew((Func<HttpResponseMessage>)(() => new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Headers = {
            WwwAuthenticate = {
              new AuthenticationHeaderValue("Digest", Header.UnauthorizedResponseHeader.ToString())
            }
          }
                }));
            }
        }
    }
}
