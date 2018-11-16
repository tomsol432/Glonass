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
        Random random = new Random();

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
                    int firstOrd = order[i];
                    int secondOrd = order[i + 1];
                    double a = Cities[firstOrd].X - Cities[secondOrd].X;
                    double b = Cities[firstOrd].Y - Cities[secondOrd].Y;
                    totalRoad += Math.Sqrt((a * a) + (b * b));
                }
                else if(i == Cities.Length - 1)
                {
                    int firstOrd = order[i];
                    int secondOrd = order[0];
                    double a = Cities[firstOrd].X - Cities[0].X;
                    double b = Cities[firstOrd].Y - Cities[0].Y;
                    totalRoad += Math.Sqrt((a * a) + (b * b));
                }
            }
            //tb.AppendText(totalRoad + " \r\n");
            

        }
        public int[] MutateOrder()
        {
            int[] temp;
            int pick = random.Next(0, 100);
            if (pick > 20)
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
           
            for (int t = 0; t < Math.Ceiling(order.Length - (order.Length * 0.6)); t++)
            {
                var tmp = order[t];
                int place = random.Next(0, order.Length);
                order[t] = order[place];
                order[place] = tmp;
            }
            return order;

        }
        public void RandomiizeArrayOrder(int[] order)
        {
            for (int t = 0; t < order.Length; t++)
            {
                
                var tmp = order[t];
                int place = random.Next(0, order.Length);
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
