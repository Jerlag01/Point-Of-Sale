using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Util
{
    public static class Util
    {
        public static string HexToString(string hex)
        {
            hex = hex.Replace(" ", "");
            byte[] bytes = new byte[hex.Length /2];
            for (int index = 0; index < bytes.Length; ++index)
                bytes[index] = Convert.ToByte(hex.Substring(index * 2, 2), 16);
            return Encoding.ASCII.GetString(bytes);
        }

        public static string ToMD5Hash(this byte[] bytes)
        {
            StringBuilder hash = new StringBuilder();
            ((IEnumerable<byte>) MD5.Create().ComputeHash(bytes)).ToList<byte>().ForEach((Action<byte>) (b => hash.AppendFormat("{0:x2}", (object) b)));
            return hash.ToString();
        }

        public static string ToMD5Hash(this string inputString)
        {
            return Encoding.UTF8.GetBytes(inputString).ToMD5Hash();
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            return new string(Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length).Select<string, char>((Func<string, char>)(s => s[random.Next(s.Length)])).ToArray<char>());
        }

        public static List<string> SystemPrinters
        {
            get
            {
                return PrinterSettings.InstalledPrinters.Cast<string>().ToList<string>();
            }
        }
    }
}
