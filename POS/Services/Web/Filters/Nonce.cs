using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using Util;

namespace Pos.Services.Web.Filters
{
    public class Nonce
    {
        private static ConcurrentDictionary<string, Tuple<int, DateTime>> nonces = new ConcurrentDictionary<string, Tuple<int, DateTime>>();

        public static string Generate()
        {
            byte[] numArray = new byte[16];
            RandomNumberGenerator.Create().GetBytes(numArray);
            string md5Hash = numArray.ToMD5Hash();
            Nonce.nonces.TryAdd(md5Hash, new Tuple<int, DateTime>(0, DateTime.Now.AddMinutes(10.0)));
            return md5Hash;
        }

        public static bool IsValid(string nonce, string nonceCount)
        {
            Tuple<int, DateTime> tuple;
            try
            {
                Nonce.nonces.TryGetValue(nonce, out tuple);
            }
            catch (Exception ex)
            {
                return false;
            }
            if (tuple == null || int.Parse(nonceCount) <= tuple.Item1 || !(tuple.Item2 > DateTime.Now))
                return false;
            Nonce.nonces[nonce] = new Tuple<int, DateTime>(tuple.Item1 + 1, tuple.Item2);
            return true;
        }
    }
}