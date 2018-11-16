using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Glonass
{
    class GenVector
    {
        int id;
        Vector[] Cities;
        int[] order;
        double totalRoad = 0;

        public GenVector(int dataSetSize, int [] order)
        {
            this.Cities = new Vector[dataSetSize];
            this.order = order;
            RandomiizeArrayOrder(this.order);
        }

        public GenVector(int id, Vector[] dataSet, int[] order)
        {
            this.Id = id;
            this.Cities = dataSet;
            this.order = order;
            RandomiizeArrayOrder(this.order);
        }
        
        public void CalculateRoadForSingleArray(TextBox tb)
        {
            this.totalRoad = 0;
            for (int i = 0; i < Cities.Length; i++)
            {
                if (i != Cities.Length - 1)
                {
                    double a = Cities[order[i]].X - Cities[order[i + 1]].X;
                    double b = Cities[order[i]].Y - Cities[order[i + 1]].Y;
                    totalRoad += Math.Sqrt((a * a) + (b * b));
                }
                else if(i == Cities.Length - 1)
                {
                    double a = Cities[order[i]].X - Cities[order[0]].X;
                    double b = Cities[order[i]].Y - Cities[order[0]].Y;
                    totalRoad += Math.Sqrt((a * a) + (b * b));
                }
            }
            tb.AppendText(totalRoad + " \r\n");
                
        }
        public int[] MutateOrder()
        {
            int[] temp;
            Random r = new Random();
            int pick = r.Next(0, 100);
            if (pick > 0)
            {
                temp = Randomiize2ArrayOrder();
                return temp;
            }
            else
            {
                return order;
            }
        }
        public int[] Randomiize2ArrayOrder()
        {
            int[] newOrder = order;
            Random r = new Random();
            for (int t = 0; t < Math.Ceiling(order.Length - (order.Length * 0.8)); t++)
            {
                var tmp = order[t];
                int place = r.Next(0, order.Length);
                order[t] = order[place];
                order[place] = tmp;
            }
            return order;

        }
        public void RandomiizeArrayOrder(int[] order)
        {
            for (int t = 0; t < order.Length; t++)
            {
                Random r = new Random();
                var tmp = order[t];
                int place = r.Next(0, order.Length);
                order[t] = order[place];
                order[place] = tmp;
            }
        }

        public int Id { get => id; set => id = value; }
        public Vector[] Cities1 { get => Cities; set => Cities = value; }
        public int[] Order { get => order; set => order = value; }
        public double TotalRoad { get => totalRoad; set => totalRoad = value; }
    }

}
