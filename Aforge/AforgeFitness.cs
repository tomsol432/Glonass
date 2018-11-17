using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AForge.Genetic;

namespace Glonass.Aforge
{
    class AforgeFitness : IFitnessFunction
    {
        private int[] order = null;
        public int[] topOrder = null;
        private Vector[] dataset = null;
        public double topScore = double.MaxValue;
        public AforgeFitness(int[] order, Vector[] dataset)
        {
            this.dataset = dataset;
            this.order = order;
        }

        public double Evaluate(IChromosome chromosome)
        {
            return 1 / (PathLength(chromosome) + 1);
            
        }
        public double PathLength(IChromosome chromosome)
        {
            double totalRoad = 0;
            for (int i = 0; i < dataset.Length; i++)
            {
                
                    if (i == dataset.Length - 1)
                    {
                        totalRoad += GetDistance(dataset[order[i]], dataset[0]);
                    }
                    else
                    {
                        totalRoad += GetDistance(dataset[order[i]], dataset[order[i + 1]]);

                    }
                    
            }
            if(totalRoad < topScore)
            {
                topScore = totalRoad;
                topOrder = order;
            }
            
            return totalRoad;
        }
        private static double GetDistance(Vector A, Vector B)
        {
            double a = (double)(B.X - A.X);
            double b = (double)(B.Y - A.Y);

            return Math.Sqrt(a * a + b * b);
        }
    }
}
