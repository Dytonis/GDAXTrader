using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkVisualizer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            DoubleBuffered = true;
            InitializeComponent();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GDAX.WebSocket socket = new GDAX.WebSocket();
            socket.OnReceivedMessage += Socket_OnReceivedMessage;
            socket.OnReceivedPacket += Socket_OnReceivedPacket;
            socket.Connect();

            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);


        }

        private void Socket_OnReceivedPacket(string raw)
        {

        }

        //sell = asks

        List<string[]> currentBids = new List<string[]>();
        List<string[]> currentAsks = new List<string[]>();
        private void Socket_OnReceivedMessage(GDAX.HttpResponse.WebSocketResponse response, string raw)
        {
            if (response.type == "snapshot")
            {
                currentBids = response.bids.ToList();
                currentAsks = response.asks.ToList();
            }
            else if(response.type == "l2update")
            {
                foreach(string[] change in response.changes)
                {
                    if (change[0] == "sell") //asks
                    {
                        bool contains = false;
                        List<int> removes = new List<int>();
                        for(int i = 0; i < currentAsks.Count; i++)
                        {
                            if(decimal.Parse(currentAsks[i][0]) == decimal.Parse(change[1]))
                            {
                                currentAsks[i][1] = change[2];
                                contains = true;

                                if (decimal.Parse(change[2]) == 0)
                                    removes.Add(i);
                            }
                        }

                        foreach (int i in removes)
                           currentAsks.RemoveAt(i);

                        if(!contains)
                        {
                            currentAsks.Add(new string[] { change[1], change[2] });
                            currentAsks = currentAsks.OrderBy(x => decimal.Parse(x[0])).ToList();
                        }
                    }
                    else if (change[0] == "buy") //asks
                    {
                        bool contains = false;
                        List<int> removes = new List<int>();
                        for (int i = 0; i < currentBids.Count; i++)
                        {
                            if (decimal.Parse(currentBids[i][0]) == decimal.Parse(change[1]))
                            {
                                currentBids[i][1] = change[2];
                                contains = true;

                                if (decimal.Parse(change[2]) == 0)
                                    removes.Add(i);
                            }
                        }

                        foreach (int i in removes)
                            currentBids.RemoveAt(i);

                        if (!contains)
                        {
                            currentBids.Add(new string[] { change[1], change[2] });
                            currentBids = currentBids.OrderByDescending(x => decimal.Parse(x[0])).ToList();
                        }
                    }
                }

                Invalidate();
            }
        }

        private void MainForm_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphing.Graph graph = new Graphing.Graph(e.Graphics, 45, 15, Width - 30, Height - 40);

            graph.SetData(currentBids.ToArray(), currentAsks.ToArray(), 0.0050m, 0.0030m);
            graph.DrawSnapshot();
        }
    }
}
