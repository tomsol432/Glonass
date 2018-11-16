﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Glonass
{
    class Genetics
    {
        Vector[] dataSet; //OrigianlDataSet
        int[] order; //OriginalOrder
        int[] bestOrder; //ContainsDataOfTheBestGeneratedOrderForWholePopulation
        List<GenVector> PopulationData; //AllPopulationData
        List<GenVector> PopulationChildrenData; //NewPopulationList
        double shortestDistance = double.MaxValue; //ItWorksLikeABestScore
        int amountOfCities; //AmountOfCitiesForEachPopulationSet
        Canvas CanvasMap; //IamUsingThisObjectToDrawDataOnMainWindowCanvas

        internal List<GenVector> PopulationData1 { get => PopulationData; set => PopulationData = value; }
        public double ShortestDistance { get => shortestDistance; set => shortestDistance = value; }
        public int[] BestOrder { get => bestOrder; set => bestOrder = value; }

        public Genetics(ref Canvas canvas, int amountofCities)
        {
            this.CanvasMap = canvas;
            this.amountOfCities = amountofCities;
            this.PopulationData = new List<GenVector>();
            this.order = new int[amountofCities];
            this.BestOrder = new int[order.Length];
            GenerateOriginalOrder();

        }
        public void DrawGenRoads(Canvas CanvasMap, SolidColorBrush colorBrush)
        {
            for (int i = 0; i < dataSet.Length - 1; i++)
            {
                //works almost ok
                Line line = new Line()
                {
                    X1 = dataSet[order[i]].X,
                    X2 = dataSet[order[i + 1]].X,
                    Y1 = dataSet[order[i]].Y,
                    Y2 = dataSet[order[i + 1]].Y,
                    Fill = null,
                    Stroke = colorBrush,
                    StrokeThickness = 4,
                };
                CanvasMap.Dispatcher.Invoke(new Action(() => { CanvasMap.Children.Add(line); }));

                if (i == dataSet.Length - 2)
                {
                    Line line2 = new Line()
                    {
                        X1 = dataSet[order[i + 1]].X,
                        X2 = dataSet[order[0]].X,
                        Y1 = dataSet[order[i + 1]].Y,
                        Y2 = dataSet[order[0]].Y,
                        Fill = null,
                        Stroke = colorBrush,
                        StrokeThickness = 4,
                    };
                    CanvasMap.Dispatcher.Invoke(new Action(() => { CanvasMap.Children.Add(line2); }));

                }
            }
        }
        public void DrawBestRoads(Canvas CanvasMap, SolidColorBrush colorBrush)
        {

            for (int i = 0; i < dataSet.Length - 1; i++)
            {
                //works almost ok
                Line line = new Line()
                {
                    X1 = dataSet[BestOrder[i]].X,
                    X2 = dataSet[BestOrder[i + 1]].X,
                    Y1 = dataSet[BestOrder[i]].Y,
                    Y2 = dataSet[BestOrder[i + 1]].Y,
                    Fill = null,
                    Stroke = colorBrush,
                    StrokeThickness = 4,
                };
                CanvasMap.Dispatcher.Invoke(new Action(() => { CanvasMap.Children.Add(line); }));

                if (i == dataSet.Length - 2)
                {
                    Line line2 = new Line()
                    {
                        X1 = dataSet[BestOrder[i + 1]].X,
                        X2 = dataSet[BestOrder[0]].X,
                        Y1 = dataSet[BestOrder[i + 1]].Y,
                        Y2 = dataSet[BestOrder[0]].Y,
                        Fill = null,
                        Stroke = colorBrush,
                        StrokeThickness = 4,
                    };
                    CanvasMap.Dispatcher.Invoke(new Action(() => { CanvasMap.Children.Add(line2); }));

                }
            }

        }

        public void DrawGenCities(ref Canvas CanvasMap, SolidColorBrush firstCity)
        {
            for (int i = 0; i < dataSet.Length; i++)
            {

                Ellipse e = new Ellipse();
                e.Width = 10;
                e.Height = 10;
                e.Fill = firstCity;
                e.Margin = new Thickness(dataSet[order[i]].X, dataSet[order[i]].Y, 0, 0);
                e.Visibility = Visibility.Visible;
                CanvasMap.Children.Add(e);

            }
        }
        public void PushCitiesDataToAllObjects()
        {

            for (int i = 0; i < dataSet.Length; i++)
            {
                PopulationData1.Add(new GenVector(i, dataSet, order));
            }

        }
        public void GenerateDataSet()
        {
            Random r = new Random();
            dataSet = new Vector[amountOfCities];
            for (int i = 0; i < dataSet.Length; i++)
            {
                dataSet[i] = new Vector(r.Next(50, (int)CanvasMap.ActualWidth), r.Next(50, (int)CanvasMap.ActualHeight));

            }
        }
        public void GenerateOriginalOrder()
        {
            for (int i = 0; i < amountOfCities; i++)
            {
                order[i] = i;

            }
        }

        public void Shake(List<GenVector> Population)
        {
            foreach (var item in Population)
            {
                RandomiizeArrayOrder(item.Order);

            }


        }
        public void CalculateRoadForEachElementInPopulation(List<GenVector> Population)
        {
            foreach (var item in Population)
            {
                item.CalculateRoadForSingleArray(item.Cities1);
                MutateOrder(item.Order);
            }
            PickShortestDistance(Population);
            SelectNextPopulationObjects(Population);
        }

        public void PickShortestDistance(List<GenVector> Population)
        {
            foreach (var item in Population)
            {
                if (item.TotalRoad < ShortestDistance && item.TotalRoad != 0)
                {
                    BestOrder = new int[order.Length];
                    ShortestDistance = item.TotalRoad;
                    Console.WriteLine("Shortest distance: " + ShortestDistance.ToString());
                    for (int i = 0; i < order.Length; i++)
                    {
                        BestOrder[i] = order[i];

                    }
                }
            }
        }


        public void SelectNextPopulationObjects(List<GenVector> Population)
        {
            PopulationChildrenData = new List<GenVector>();
            List<GenVector> SortedPopulationList = Population.OrderBy(o => o.TotalRoad).ToList();
            for (int i = 0; i < SortedPopulationList.Count - (int)(SortedPopulationList.Count * 0.5); i++)
            {
                PopulationChildrenData.Add(SortedPopulationList[i]);

            }
            for (int i = PopulationChildrenData.Count; i < SortedPopulationList.Count; i++)
            {
                int[] newRandomOrder = RandomiizeArrayOrderT(order);
                PopulationChildrenData.Add(new GenVector(dataSet.Length, newRandomOrder));
            }
            PopulationData = PopulationChildrenData;
            foreach (var item in PopulationData)
            {
                item.Order = MutateOrder(item.Order);
            }

        }
        public int[] MutateOrder(int[] order)
        {
            int[] temp;
            Random r = new Random();
            int pick = r.Next(0, 100);
            if (pick > 0)
            {
                temp = Randomiize2ArrayOrder(order);
                return temp;
            }
            else
            {
                return order;
            }
        }


        public void RandomiizeListOrder(List<GenVector> genVectors)
        {
            Random r = new Random();
            for (int t = 0; t < genVectors.Capacity; t++)
            {
                var tmp = genVectors[t];
                int place = r.Next(0, genVectors.Capacity);
                genVectors[t] = genVectors[place];
                genVectors[place] = tmp;
            }
        }
        public int[] RandomiizeArrayOrder(int[] order)
        {
            Random r = new Random();
            for (int t = 0; t < order.Length; t++)
            {
                var tmp = order[t];
                int place = r.Next(0, order.Length);
                order[t] = order[place];
                order[place] = tmp;
            }
            return order;
        }
        public int[] Randomiize2ArrayOrder(int[] order)
        {
            int[] newOrder = order;
            Random r = new Random();
            for (int t = 0; t <Math.Ceiling(order.Length - (order.Length * 0.9)); t++)
            {
                var tmp = order[t];
                int place = r.Next(0, order.Length);
                order[t] = order[place];
                order[place] = tmp;
            }
            return order;

        }
        public int[] RandomiizeArrayOrderT(int[] orderT)
        {
            Random r = new Random();
            for (int t = 0; t < orderT.Length; t++)
            {
                var tmp = orderT[t];
                int place = r.Next(0, orderT.Length);
                orderT[t] = orderT[place];
                orderT[place] = tmp;
            }
            return orderT;
        }
    }
}
