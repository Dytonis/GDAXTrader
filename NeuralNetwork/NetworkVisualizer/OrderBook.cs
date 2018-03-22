using System.Collections.Generic;
using System.Linq;

namespace NetworkVisualizer
{
    public class OrderBook
    {
        public delegate void BookUpdated(OrderBook b);
        public event BookUpdated OnBookUpdated;

        /// <summary>
        /// Bids[x][0] as Value
        /// Bids[x][1] as Count
        /// </summary>
        public List<decimal[]> Bids = new List<decimal[]>();
        /// <summary>
        /// Bids[x][0] as Value
        /// Bids[x][1] as Count
        /// </summary>
        public List<decimal[]> Asks = new List<decimal[]>();

        public List<Step> BidSteps = new List<Step>();
        public List<Step> AskSteps = new List<Step>();

        public decimal[] CurrentMaxBid;
        public decimal[] CurrentMinAsk;

        public decimal MidMarketPrice;

        public void Update(string[][] changes)
        {
            foreach (string[] change in changes)
            {
                if (change[0] == "sell") //asks
                {
                    bool contains = false;
                    List<int> removes = new List<int>();
                    for (int i = 0; i < Asks.Count; i++)
                    {
                        if (Asks[i][0] == decimal.Parse(change[1]))
                        {
                            Asks[i][1] = decimal.Parse(change[2]);
                            contains = true;

                            if (decimal.Parse(change[2]) == 0)
                                removes.Add(i);
                        }
                    }

                    foreach (int i in removes)
                        Asks.RemoveAt(i);

                    if (!contains)
                    {
                        Asks.Add(new decimal[] { decimal.Parse(change[1]), decimal.Parse(change[2]) });
                        Asks = Asks.OrderBy(x => x[0]).ToList();
                    }
                }
                else if (change[0] == "buy") //asks
                {
                    bool contains = false;
                    List<int> removes = new List<int>();
                    for (int i = 0; i < Bids.Count; i++)
                    {
                        if (Bids[i][0] == decimal.Parse(change[1]))
                        {
                            Bids[i][1] = decimal.Parse(change[2]);
                            contains = true;

                            if (decimal.Parse(change[2]) == 0)
                                removes.Add(i);
                        }
                    }

                    foreach (int i in removes)
                        Bids.RemoveAt(i);

                    if (!contains)
                    {
                        Bids.Add(new decimal[] { decimal.Parse(change[1]), decimal.Parse(change[2]) });
                        Bids = Bids.OrderByDescending(x => x[0]).ToList();
                    }
                }
            }

            decimal[] minAsk = Asks.ElementAt(0);
            CurrentMinAsk = new decimal[] { minAsk[0], minAsk[1] };

            decimal[] maxBid = Bids.ElementAt(0);
            CurrentMaxBid = new decimal[] { maxBid[0], maxBid[1] };

            OnBookUpdated?.Invoke(this);
        }

        public void CalculateSteps(decimal MinSize)
        {
            for (int i = 0; i < Asks.Count; i++)
            {
                if (i == 0)
                {
                    if (Asks[0][1] >= MinSize)
                        AskSteps.Add(new Step()
                        {
                            Count = Asks[0][1],
                        });
                }
            }
        }

        public void SetBids(string[][] bids)
        {
            Bids = new List<decimal[]>();
            Bids.Clear();
            foreach (string[] s in bids)
            {
                Bids.Add(new decimal[] { decimal.Parse(s[0]), decimal.Parse(s[1]) });
            }

            decimal[] maxBid = Bids.ElementAt(0);
            CurrentMaxBid = new decimal[] { maxBid[0], maxBid[1] };

        }

        public void SetAsks(string[][] asks)
        {
            Asks = new List<decimal[]>();
            Asks.Clear();
            foreach(string[] s in asks)
            {
                Asks.Add(new decimal[] { decimal.Parse(s[0]), decimal.Parse(s[1]) });
            }

            decimal[] minAsk = Asks.ElementAt(0);
            CurrentMinAsk = new decimal[] { minAsk[0], minAsk[1] };
        }
    }

    public class Step
    {
        /// <summary>
        /// The price of the product at this step
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// The amount of product avaiable at this price point
        /// </summary>
        public decimal Count { get; set; }
        /// <summary>
        /// Amount of product that makes up this step
        /// </summary>
        public decimal Height { get; set; }
        /// <summary>
        /// Distance from mid-market value
        /// </summary>
        public decimal Width { get; set; }
    }
}
