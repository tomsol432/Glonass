﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Genetic;

namespace Glonass.Aforge
{
    class MyAforgePopulation : Population
    {
        int size;
        Aforge.AforgeChromosome ancestor;
        Aforge.AforgeFitness fitnessFunction;
        ISelectionMethod selectionMethod;

        public MyAforgePopulation(int size, AforgeChromosome ancestor, AforgeFitness fitnessFunction, ISelectionMethod selectionMethod) : base(size, ancestor, fitnessFunction, selectionMethod)
        {
            this.size = size;
            this.ancestor = ancestor;
            this.fitnessFunction = fitnessFunction;
            this.selectionMethod = selectionMethod;
        }

        public override void Mutate()
        {
            ancestor.Mutate();
            
        }
        public override void Selection()
        {
            base.Selection();
        }
        
    }
}
