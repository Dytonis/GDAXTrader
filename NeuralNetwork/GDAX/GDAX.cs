using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GDAX
{
    public class GDAX
    {
        private string key;
        private string passphrase;
        private string secret;      //base64

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public GDAX(string key, string passphrase, string base64Secret)
        {
            this.key = key;
            this.passphrase = passphrase;
            this.secret = base64Secret;
        }

        public HttpClient GetClient(string body, string endpoint, string method = "POST")
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("CB-ACCESS-KEY", key);
            client.DefaultRequestHeaders.Add("CB-ACCESS-PASSPHRASE", passphrase);

            long seconds = ToUnixTime(DateTime.Now);

            client.DefaultRequestHeaders.Add("CB-ACCESS-TIMESTAMP", seconds.ToString());

            byte[] data = Convert.FromBase64String(secret);

            string sign = seconds.ToString() + method + endpoint + body;

            string sha256 = HMAC(data, sign);

            client.DefaultRequestHeaders.Add("CB-ACCESS-SIGN", sign);
            client.DefaultRequestHeaders.Add("Content-Type", "application/json");

            return client;
        }

        public static string HMAC(byte[] secret, string dataToSign)
        {
            HMACSHA256 hmac = new HMACSHA256(secret);

            byte[] dataBytes = UTF8Encoding.UTF8.GetBytes(dataToSign);
            byte[] calcHash = hmac.ComputeHash(dataBytes);
            String calcHashString = Convert.ToBase64String(calcHash);
            return calcHashString;
        }

        public static long ToUnixTime(DateTime time)
        {
            return Convert.ToInt64((time - epoch).TotalSeconds);
        }
    }
}
