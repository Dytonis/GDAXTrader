﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkVisualizer.Graphing
{
    public class Graph
    {
        private Graphics g;

        OrderBook book;
        decimal lowValueRange;
        decimal highValueRange;
        decimal lowCountRange;
        decimal highCountRange;

        int leftMargin;
        int bottomMargin;
        int topMargin;
        int rightMargin;

        int height;
        int width;

        BufferedGraphicsContext context;
        BufferedGraphics buffer;

        decimal MarketValue;

        public Graph(Graphics g, int left, int bottom, int width, int height)
        {
            this.g = g;
            leftMargin = left;
            bottomMargin = bottom;
            this.height = height;
            this.width = width;

            context = BufferedGraphicsManager.Current;
            buffer = context.Allocate(g, new Rectangle(0, 0, width, height));
            buffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
        }

        public void SetData(OrderBook book, decimal percentZoomValue, decimal percentCutoffCount)
        {
            if (book.Bids.Count <= 0 || book.Asks.Count <= 0)
                return;

            this.book = book;

            try
            {
                lowValueRange = book.Bids.Min(x => x[0]);
                highValueRange = book.Bids.Max(x => x[0]);

                lowCountRange = book.Bids.Min(x => x[1]);
                highCountRange = book.Bids.Max(x => x[1]);

                highCountRange *= percentCutoffCount;

                decimal midMarketValue = highValueRange + 0.005m;
                decimal range = midMarketValue * 0.005m;

                MarketValue = midMarketValue;

                lowValueRange = midMarketValue - range;
                highValueRange = midMarketValue + range;
            }
            catch(Exception ex)
            {
                Console.Out.WriteLine("ex");
            }
        }

        public void DrawDot(decimal Value, decimal Count)
        {
            DrawActions.Add(new Action(() => buffer.Graphics.DrawEllipse(Pens.Yellow, new Rectangle(ValueToX(Value) - 2, CountToY(Count) - 2, 4, 4))));
        }

        private List<Action> DrawActions = new List<Action>();

        public void Draw()
        {
            if (book == null)
                return;
            if (book.Bids == null || book.Asks == null)
                return;
            if (book.Bids.Count <= 0 || book.Asks.Count <= 0)
                return;

            buffer.Graphics.Clear(Color.FromArgb(255, 64, 64, 64));

            for(int i = 0; i < DrawActions.Count; i++)
            {
                DrawActions[i]();
            }

            DrawXLabels(15, bottomMargin, leftMargin);
            DrawYLabels(5, leftMargin, bottomMargin);

            decimal count = 0;

            for (int i = 0; i < book.Bids.Count; i += 1)
            {
                decimal value = book.Bids[i][0];
                count += book.Bids[i][1];
                decimal count2 = count + book.Bids[i + 1][1];
                decimal value2 = book.Bids[i + 1][0];

                if(DrawPair(value, count, value2, count2) == false) break;
            } 

            count = 0;

            for(int i = 0; i < book.Bids.Count; i+= 1)
            {
                decimal value = book.Asks[i][0];
                count += book.Asks[i][1];
                decimal count2 = count + book.Asks[i + 1][1];
                decimal value2 = book.Asks[i + 1][0];

                if(DrawPair(value, count, value2, count2) == false) break;
            }

            buffer.Render(g);
            //DrawActions.Clear();
        }

        private int ValueToX(decimal value)
        {
            if (highValueRange == 0)
                return 0;

            return (int)Mathm.Map(value, lowValueRange, highValueRange, leftMargin, width - rightMargin);
        }

        private int CountToY(decimal count)
        {
            if (highCountRange == 0)
                return 0;

            return (int)Mathm.Map(count, lowCountRange, highCountRange, bottomMargin, height - 20);
        }

        private bool DrawPair(decimal value, decimal count, decimal value2, decimal count2)
        {
            int x = (int)Mathm.Map(value, lowValueRange, highValueRange, leftMargin, width - rightMargin) + (leftMargin / 2);
            int y = height - (int)Mathm.Map(count, lowCountRange, highCountRange, bottomMargin, height - 20);
            int x2 = (int)Mathm.Map(value2, lowValueRange, highValueRange, leftMargin, width - rightMargin) + (leftMargin / 2);
            int y2 = height - (int)Mathm.Map(count2, lowCountRange, highCountRange, bottomMargin, height - 20);

            if (x2 < (width / 2) + leftMargin) //left side
            {
                if (y2 < 0)
                {
                    //g.FillRectangle(new SolidBrush(Color.FromArgb(127, 50, 50, 50)), new Rectangle(xOffset, 0, x-xOffset, (height - 20)));
                    buffer.Graphics.FillRectangle(Brushes.DarkGreen, new Rectangle(leftMargin, 0, x - leftMargin, (height - bottomMargin)));

                    buffer.Graphics.DrawLine(new Pen(Color.LightGreen, 2), new Point(x, y), new Point(x, 0));
                    return false;
                }
                else if (x2 < leftMargin)
                    return false;
                else
                {
                    if (count2 - count > 40 && y2 - 53 > 0)
                    {
                        buffer.Graphics.DrawString(value.ToString("C"), new Font("Cambria", 8), Brushes.Gray, new PointF(x + 30, y2 - 43));
                        buffer.Graphics.DrawLine(Pens.Black, new Point(x, y2), new Point(x + 30, y2 - 30));
                    }

                    buffer.Graphics.FillRectangle(Brushes.DarkGreen, new Rectangle(x2, y2, x - x2, (height - y2) - bottomMargin));
                    buffer.Graphics.DrawLine(new Pen(Color.LightGreen, 2), new Point(x, y), new Point(x, y2));
                    buffer.Graphics.DrawLine(new Pen(Color.LightGreen, 2), new Point(x, y2), new Point(x2, y2));
                }
            }
            else
            {
                if (y2 < 0)
                {
                    //g.FillRectangle(new SolidBrush(Color.FromArgb(127, 50, 50, 50)), new Rectangle(xOffset, 0, x-xOffset, (height - 20)));
                    buffer.Graphics.FillRectangle(Brushes.Maroon, new Rectangle(x - 1, y2, width - x2, height - y2 - bottomMargin));

                    buffer.Graphics.DrawLine(new Pen(Color.Salmon, 2), new Point(x, y), new Point(x, 0));
                    return false;
                }
                else if (x2 > width - rightMargin)
                    return false;
                else
                {
                    if (count2 - count > 70 && y2 - 53 > 0)
                    {
                        buffer.Graphics.DrawString(value2.ToString("C"), new Font("Cambria", 8), Brushes.Gray, new PointF(x - (value2.ToString("C").Length * 10), y2 - 43));
                        buffer.Graphics.DrawLine(Pens.Black, new Point(x, y2), new Point(x - 30, y2 - 30));
                    }

                    buffer.Graphics.FillRectangle(Brushes.Maroon, new Rectangle(x, y2, x2 - x, height - y2 - bottomMargin));
                    buffer.Graphics.DrawLine(new Pen(Color.Salmon, 2), new Point(x, y), new Point(x, y2));
                    buffer.Graphics.DrawLine(new Pen(Color.Salmon, 2), new Point(x, y2), new Point(x2, y2));

                }
            }

            return true;
        }

        private void DrawXLabels(int count, int size, int offset)
        {
            for (int i = offset; i < width - rightMargin; i += (width / count))
            {
                buffer.Graphics.DrawLine(Pens.Gray, new Point(i, height - size), new Point(i, height + 3 - size));
                decimal percent = Mathm.Normalize(i, offset, width);
                decimal v = Mathm.Sweep(percent, lowValueRange, highValueRange);
                buffer.Graphics.DrawString(v.ToString("C"), new Font("Cambria", 8), Brushes.Gray, new PointF(i - 12, height - 12));
            }
            buffer.Graphics.DrawString(MarketValue.ToString("C"), new Font("Cambria", 8), Brushes.Gray, new PointF(50, 100));
            buffer.Graphics.DrawLine(Pens.Gray, new Point((width / 2) + leftMargin, height - bottomMargin), new Point((width / 2) + leftMargin, 0));
        }

        private void DrawYLabels(int count, int size, int offset)
        {
            for (int i = height; i >= offset; i -= (height / count))
            {
                buffer.Graphics.DrawLine(Pens.Gray, new Point(size - 3, i - offset), new Point(size, i - offset));
                decimal percent = Mathm.Normalize(height - i, 0, height);
                decimal v = Mathm.Sweep(percent, lowCountRange, highCountRange);
                buffer.Graphics.DrawString(v.ToString("###,##0"), new Font("Cambria", 8), Brushes.Gray, new PointF(3, i - offset - 6));
            }
        }
    }
}
