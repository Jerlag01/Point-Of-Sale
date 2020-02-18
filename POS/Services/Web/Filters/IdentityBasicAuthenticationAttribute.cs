using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Pos.Services.Web.Properties;

namespace Pos.Services.Web.Filters
{
    public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute
    {
        protected override async Task<IPrincipal> AuthenticateAsync(
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return !(userName == Settings.Default.RemoteLogin) || !(password == Settings.Default.RemotePassword) ? (IPrincipal)null : (IPrincipal)new ClaimsPrincipal((IPrincipal)new GenericPrincipal((IIdentity)new GenericIdentity(userName), new string[0]));
        }
    }
}