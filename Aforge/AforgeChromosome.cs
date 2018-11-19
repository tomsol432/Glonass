using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AForge.Genetic;

namespace Glonass.Aforge
{
    class AforgeChromosome : PermutationChromosome
    {
        Random r = new Random();
        int[] order;
        Vector[] dataset;

        public AforgeChromosome(int[] order, Vector[] dataset) : base(order.Length)
        {
            this.order = order;
            this.dataset = dataset;
        }

        protected AforgeChromosome(AforgeChromosome source) : base(source)
        {
            this.order = source.order;
            this.dataset = source.dataset;
        }

        public override IChromosome CreateNew()
        {
            return new AforgeChromosome(this.order,this.dataset);
        }
        public override IChromosome Clone()
        {
            return new AforgeChromosome(this);
        }
        public override void Mutate()
        {
            for (int t = 0; t < order.Length ; t++)
            {
                var tmp = order[t];
                int place = r.Next(0, order.Length);
                order[t] = order[place];
                order[place] = tmp;
            }
        }
        public void Crossover(AforgeChromosome pair)
        {
            int splitPlace = r.Next(1, order.Length);
            //int[] firstDNA = { };
            //int[] seconDDNA = { };
            int[] finalDNA = { };
            for (int i = 0; i < splitPlace; i++)
            {
               // firstDNA[i] = order[i];
                finalDNA[i] = order[i];
            }
            for (int i = splitPlace; i < order.Length; i++)
            {
               // seconDDNA[i] = pair.order[i];
                finalDNA[i] = pair.order[i];
            }

            if (finalDNA.Length != finalDNA.Distinct().Count())
            {
                //HAS DUPS
            }
            else
            {
                order = finalDNA;
            }



        }
    }
}
