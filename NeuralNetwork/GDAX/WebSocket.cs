using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using GDAX.HttpResponse;
using Newtonsoft.Json;

namespace GDAX
{
    public class WebSocket
    {
        public delegate void ReceivedMessage(WebSocketResponse response, string raw);
        public event ReceivedMessage OnReceivedMessage;

        public delegate void ReceivedPacket(string raw);
        public event ReceivedPacket OnReceivedPacket;

        public async Task Connect()
        {
            ClientWebSocket socket = new ClientWebSocket();
            await socket.ConnectAsync(new Uri("wss://ws-feed.gdax.com"), CancellationToken.None);

            if (socket.State == WebSocketState.Open)
            {
                string json = JsonConvert.SerializeObject(new
                {
                    type = "subscribe",
                    product_ids = new[] { "ltc-usd" },
                    channels = new[] { "level2" }
                });

                byte[] requestBytes = UTF8Encoding.UTF8.GetBytes(json);
                await socket.SendAsync(new ArraySegment<byte>(requestBytes), WebSocketMessageType.Text, true, new CancellationToken());

                while (socket.State == WebSocketState.Open)
                {
                    List<byte> memory = new List<byte>();

                    byte[] recBytes = new byte[1024];
                    while (true)
                    {
                        ArraySegment<byte> t = new ArraySegment<byte>(recBytes);
                        WebSocketReceiveResult receiveAsync = await socket.ReceiveAsync(t, CancellationToken.None);
                        string jsonString = Encoding.UTF8.GetString(recBytes);
                        OnReceivedPacket.Invoke(jsonString);
                        byte[] corrected = t.Array.Where(x => x != 0).ToArray();
                        memory.AddRange(corrected);
                        recBytes = new byte[1024];

                        if (receiveAsync.EndOfMessage)
                            break;
                    }

                    string text = Encoding.ASCII.GetString(memory.ToArray());
                    OnReceivedMessage.Invoke(JsonConvert.DeserializeObject<WebSocketResponse>(text), text);
                }
            }
        }
    }
}
