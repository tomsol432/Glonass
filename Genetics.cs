using System;
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
        double shortestDistance = double.MaxValue;
        int amountOfCities; //AmountOfCitiesForEachPopulationSet
        Canvas CanvasMap; //IamUsingThisObjectToDrawDataOnMainWindowCanvas

        internal List<GenVector> PopulationData1 { get => PopulationData; set => PopulationData = value; }

        public Genetics(ref Canvas canvas, int populationSize,int amountofCities)
        {
            this.CanvasMap = canvas;
            this.amountOfCities = amountofCities;
            this.PopulationData1 = new List<GenVector>();
            this.order = new int[amountofCities];
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
            if (bestOrder != null)
            {
                for (int i = 0; i < dataSet.Length - 1; i++)
                {
                    //works almost ok
                    Line line = new Line()
                    {
                        X1 = dataSet[bestOrder[i]].X,
                        X2 = dataSet[bestOrder[i + 1]].X,
                        Y1 = dataSet[bestOrder[i]].Y,
                        Y2 = dataSet[bestOrder[i + 1]].Y,
                        Fill = null,
                        Stroke = colorBrush,
                        StrokeThickness = 4,
                    };
                    CanvasMap.Dispatcher.Invoke(new Action(() => { CanvasMap.Children.Add(line); }));

                    if (i == dataSet.Length - 2)
                    {
                        Line line2 = new Line()
                        {
                            X1 = dataSet[bestOrder[i + 1]].X,
                            X2 = dataSet[bestOrder[0]].X,
                            Y1 = dataSet[bestOrder[i + 1]].Y,
                            Y2 = dataSet[bestOrder[0]].Y,
                            Fill = null,
                            Stroke = colorBrush,
                            StrokeThickness = 4,
                        };
                        CanvasMap.Dispatcher.Invoke(new Action(() => { CanvasMap.Children.Add(line2); }));

                    }
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
                PopulationData1.Add(new GenVector(i,dataSet,order));
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
                
               // Console.WriteLine(item.TotalRoad);
               
                
                item.RandomiizeArrayOrder(item.Order);
            }
            PickShortestDistance(Population);
            SelectNextPopulationObjects(Population);
        }

        public void PickShortestDistance(List<GenVector> Population)
        {
            foreach (var item in Population)
            {
                if(item.TotalRoad < shortestDistance && shortestDistance != 0)
                {
                    shortestDistance = item.TotalRoad;
                    Console.WriteLine("Shortest distance: " + shortestDistance.ToString());
                    bestOrder = item.Order;
                }
            }
        }


        public void SelectNextPopulationObjects(List<GenVector> Population)
        {
            PopulationChildrenData = new List<GenVector>();
            List<GenVector> SortedPopulationList = Population.OrderBy(o => o.TotalRoad).ToList();
            for (int i = 0; i < SortedPopulationList.Count - (SortedPopulationList.Count*0.05); i++)
            {
                PopulationChildrenData.Add(SortedPopulationList[i]);
            }
            for (int i = PopulationChildrenData.Count; i < SortedPopulationList.Count; i++)
            {
                int[] newRandomOrder = RandomiizeArrayOrderT(order);
                PopulationChildrenData.Add(new GenVector(dataSet.Length, newRandomOrder));
            }
            this.PopulationData = PopulationChildrenData;
            PopulationChildrenData = null;

            //Mix'emABit
        }
        public void mutateOrder()
        {
            foreach (var item in PopulationData)
            {
                Random r = new Random();
                int pick = r.Next(0, 100);
                if(pick > 90)
                {
                    Randomiize2ArrayOrder(item.Order);
                }

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
        public int [] RandomiizeArrayOrder(int[] order)
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
        public void Randomiize2ArrayOrder(int[] order)
        {
            Random r = new Random();
            int f = r.Next(0, order.Length);
            int s = r.Next(0, order.Length);
            if (f != s)
            {
                var tmp = order[f];
                order[f] = order[s];
                order[s] = tmp;
            }
            
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
