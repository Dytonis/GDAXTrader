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
        OrderBook book = new OrderBook();
        Graphing.Graph graph;

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

            Strategy strategy = new Strategy(book);
        }

        private void Socket_OnReceivedPacket(string raw)
        {

        }

        //sell = asks

        private void Socket_OnReceivedMessage(GDAX.HttpResponse.WebSocketResponse response, string raw)
        {
            if (response.type == "snapshot")
            {
                book.SetAsks(response.asks);
                book.SetBids(response.bids);
            }
            else if (response.type == "l2update")
            {
                book.Update(response.changes);
            }

            Invalidate();
        }

        private void MainForm_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            graph = new Graphing.Graph(e.Graphics, 45, 15, Width - 30, Height - 75);
            if (book.Bids != null)
            {
                graph.SetData(book, 0.0050m, 0.0030m);
                graph.Draw();
            }
            Graphing.GraphContext.Current = graph;
        }
    }
}
