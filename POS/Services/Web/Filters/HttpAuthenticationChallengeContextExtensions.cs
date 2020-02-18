// Decompiled with JetBrains decompiler
// Type: Tjok.Pos.Services.Web.Filters.HttpAuthenticationChallengeContextExtensions
// Assembly: Tjok.Pos.Services.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C9A65D9-46A7-42C4-AC8A-535BCC00136E
// Assembly location: D:\lagae\Documents\POS Tjok\SDP Technologies\Web Service\Tjok.Pos.Services.Web.exe

using System;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Filters;
using Pos.Services.Web.Results;

namespace Pos.Services.Web.Filters
{
    public static class HttpAuthenticationChallengeContextExtensions
    {
        public static void ChallengeWith(this HttpAuthenticationChallengeContext context, string scheme)
        {
            context.ChallengeWith(new AuthenticationHeaderValue(scheme));
        }

        public static void ChallengeWith(
            this HttpAuthenticationChallengeContext context,
            string scheme,
            string parameter)
        {
            context.ChallengeWith(new AuthenticationHeaderValue(scheme, parameter));
        }

        public static void ChallengeWith(
            this HttpAuthenticationChallengeContext context,
            AuthenticationHeaderValue challenge)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            context.Result = (IHttpActionResult)new AddChallengeOnUnauthorizedResult(challenge, context.Result);
        }
    }
}