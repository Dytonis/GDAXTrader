using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDAX.HttpResponse
{
    public class WebSocketResponse
    {
        public string product_id { get; set; }
        public string type { get; set; }
        public string[][] bids { get; set; }
        public string[][] asks { get; set; }
        public string[][] changes { get; set; }
    }
}
