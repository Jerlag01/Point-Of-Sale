using System.Text;

namespace Pos.Services.Web.Filters
{
    public class Header
    {
        public Header()
        {
        }

        public Header(string header, string method)
        {
            string str1 = header.Replace("\"", string.Empty);
            char[] chArray = new char[1] { ',' };
            foreach (string str2 in str1.Split(chArray))
            {
                int length = str2.IndexOf("=");
                string str3 = str2.Substring(0, length).Trim();
                string str4 = str2.Substring(length + 1);
                switch (str3)
                {
                    case "cnonce":
                        this.Cnonce = str4;
                        break;
                    case nameof(method):
                        this.Method = str4;
                        break;
                    case "nc":
                        this.NounceCounter = str4;
                        break;
                    case "nonce":
                        this.Nonce = str4;
                        break;
                    case "realm":
                        this.Realm = str4;
                        break;
                    case "response":
                        this.Response = str4;
                        break;
                    case "uri":
                        this.Uri = str4;
                        break;
                    case "username":
                        this.UserName = str4;
                        break;
                }
            }
            if (!string.IsNullOrEmpty(this.Method))
                return;
            this.Method = method;
        }

        public string Cnonce { get; private set; }

        public string Nonce { get; private set; }

        public string Realm { get; private set; }

        public string UserName { get; private set; }

        public string Uri { get; private set; }

        public string Response { get; private set; }

        public string Method { get; private set; }

        public string NounceCounter { get; private set; }

        public static Header UnauthorizedResponseHeader
        {
            get
            {
                return new Header()
                {
                    Realm = "Tjok.Pos.Services.Web",
                    Nonce = Pos.Services.Web.Filters.Nonce.Generate()
                };
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("realm=\"{0}\"", (object)this.Realm);
            stringBuilder.AppendFormat(", nonce=\"{0}\"", (object)this.Nonce);
            stringBuilder.AppendFormat(", qop=\"{0}\"", (object)"auth");
            return stringBuilder.ToString();
        }
    }
}
