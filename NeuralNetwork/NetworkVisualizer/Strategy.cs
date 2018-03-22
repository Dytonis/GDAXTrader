using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkVisualizer
{
    public class Strategy
    {
        public int MinSizeOfStepToConsider { get; set; }
        public float BidsVsAsksStepBias { get; set; }
        public decimal OvercutBuyOrder { get; set; }
        public decimal UndercutSellOrder { get; set; }

        private decimal MaxMinRatio;
        private decimal MinMaxRatio;

        private OrderBook book;

        public Strategy(OrderBook book)
        {
            this.book = book;

            book.OnBookUpdated += Book_OnBookUpdated;
        }

        private void Book_OnBookUpdated(OrderBook b)
        {
            MaxMinRatio = b.CurrentMaxBid[1] / b.CurrentMinAsk[1];
            MinMaxRatio = b.CurrentMinAsk[1] / b.CurrentMaxBid[1];

            if (b.CurrentMinAsk[1] >= 50 && MinMaxRatio > 25)
            {
                var a = Graphing.GraphContext.Current;
                a.DrawDot(b.CurrentMinAsk[0], b.CurrentMinAsk[1]);
            }
            else if (b.CurrentMaxBid[1] >= 50 && MaxMinRatio > 25)
            {
                var a = Graphing.GraphContext.Current;
                a.DrawDot(b.CurrentMaxBid[0], b.CurrentMaxBid[1]);
            }

            var c = Graphing.GraphContext.Current;
            c.DrawDot(b.CurrentMaxBid[0], b.CurrentMaxBid[1]);
        }
    }

    public class Order
    {
        public OrderTypes OrderType;
        public decimal Value;
        public decimal Count;
    }

    public class BuyCondition
    {
        
    }

    public enum OrderTypes
    {
        Buy,
        Sell,
        TakeBuy,
        TakeSell
    }
}
