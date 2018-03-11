using GDAX;
using GDAX.HttpResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader
{
    class Program
    {
        static void Main(string[] args)
            => MainAsync().GetAwaiter().GetResult();

        public static async Task MainAsync()
        {
            WebSocket websocket = new WebSocket();
            websocket.OnReceivedMessage += Websocket_OnWebsocketUpdated;
            websocket.OnReceivedPacket += Websocket_OnReceivedPacket;
            await websocket.Connect();
            await Task.Delay(-1);
        }

        private static void Websocket_OnReceivedPacket(string raw)
        {
            //Console.WriteLine(raw);
        }

        private static void Websocket_OnWebsocketUpdated(WebSocketResponse response, string raw)
        {

        }
    }
}
