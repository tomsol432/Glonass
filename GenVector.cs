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

        public GenVector(int dataSetSize, int [] RandomOrder)
        {
            this.Cities = new Vector[dataSetSize];
            this.order = RandomOrder; 
        }

        public GenVector(int id, Vector[] dataSet, int[] order)
        {
            this.Id = id;
            this.Cities = dataSet;
            this.Order = order;
        }
        
        public void CalculateRoadForSingleArray(Vector[] Cities)
        {
            //this.totalRoad = 0;
            for (int i = 0; i < Cities.Length; i++)
            {
                if (i != Cities.Length - 1)
                {
                    double a = (double)(Cities[order[i]].X - Cities[order[i+1]].X);
                    double b = (double)(Cities[order[i]].Y - Cities[order[i+1]].Y);
                    totalRoad += Math.Sqrt(a * a + b * b);
                }
                else if(i == Cities.Length - 1)
                {
                    double a = (double)(Cities[order[i]].X - Cities[order[0]].X);
                    double b = (double)(Cities[order[i]].Y - Cities[order[0]].Y);
                    totalRoad += Math.Sqrt(a * a + b * b);
                }
            }
            

            
        }
        

        public int Id { get => id; set => id = value; }
        public Vector[] Cities1 { get => Cities; set => Cities = value; }
        public int[] Order { get => order; set => order = value; }
        public double TotalRoad { get => totalRoad; set => totalRoad = value; }
    }

}
