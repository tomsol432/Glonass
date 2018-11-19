using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AForge.Genetic;

namespace Glonass
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        #region var
        Random r = new Random();
        int CitiesCounter;
        double bestScore = double.MaxValue;
        
        Thread[] pool = new Thread[1];
        Vector[] Roads;
        Vector[] sweetRoad;
        List<Vector[]> AllCombinations = new List<Vector[]>();
        Thread threadGen;
        Genetics genetics;
        List<GenVector> genVectors;
        int[] bestOrderFromGenVector;
        
        
        #endregion
        #region BrushDef
        SolidColorBrush cityBrush = new SolidColorBrush(Color.FromRgb( 0, 55, 255));
        SolidColorBrush firstCity = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        SolidColorBrush roadBrush = new SolidColorBrush(Color.FromArgb(255,0, 255, 0));
        SolidColorBrush sweetBrush = new SolidColorBrush(Color.FromArgb(125,255, 45, 255));
        
        #endregion
        
    private void CanvasMap_Loaded(object sender, RoutedEventArgs e)
        {

        }



        public void DrawCities(Vector[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Ellipse e = new Ellipse();
                e.Width = 10;
                e.Height = 10;
                if (i == 0)
                {
                    e.Fill = firstCity;
                }
                else
                {
                    e.Fill = cityBrush;
                }
                e.Margin = new Thickness(array[i].X, array[i].Y, 0, 0);
                e.Visibility = Visibility.Visible;
                CanvasMap.Children.Add(e);

            }
        }

        public void DrawRoads(Vector[] array, SolidColorBrush brush, int thicc)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                //works almost ok
                Line line = new Line()
                {
                    X1 = array[i].X,
                    X2 = array[i + 1].X,
                    Y1 = array[i].Y,
                    Y2 = array[i + 1].Y,
                    Fill = null,
                    Stroke = brush,
                    StrokeThickness = thicc,
                };        
                    CanvasMap.Dispatcher.Invoke(new Action(() => { CanvasMap.Children.Add(line); }));

                if (i == array.Length - 2)
                {
                    Line line2 = new Line()
                    {
                        X1 = array[i +1].X,
                        X2 = array[0].X,
                        Y1 = array[i +1].Y,
                        Y2 = array[0].Y,
                        Fill = null,
                        Stroke = brush,
                        StrokeThickness = thicc,
            };
                    CanvasMap.Dispatcher.Invoke(new Action(() => { CanvasMap.Children.Add(line2); }));

                }
            }
            
        }
    

        private void SliderCitiesCounter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ButtonGenerate.Content = "Generate (" + SliderCitiesCounter.Value + ")";
        }

        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            
            bestScore = double.MaxValue;
            CitiesCounter = (int)SliderCitiesCounter.Value;
            Roads = new Vector[CitiesCounter];
            sweetRoad = new Vector[CitiesCounter];
            CanvasMap.Children.Clear();

            for (int i = 0; i < Roads.Length; i++)
            {
                Roads[i] = new Vector(r.Next(50,(int) CanvasMap.ActualWidth), r.Next(50, (int) CanvasMap.ActualHeight));
                sweetRoad[i] = Roads[i]; 
            }
            DrawCities(Roads);
            DrawRoads(Roads, roadBrush,3);
            
        }
        
        public void CalcRoad(Vector[] array)
        {
            double totalRoad = 0;
            
            for (int i = 0; i < array.Length; i++)
            {
                
                if (totalRoad > bestScore)
                {
                    break;
                }
                else
                {
                    if (i == array.Length - 1)
                    {
                        totalRoad += GetDistance(array[i], array[0]);
                    }
                    else
                    {
                        totalRoad += GetDistance(array[i], array[i + 1]);

                    }
                }
                
            }
            if (totalRoad < bestScore)
            {
                bestScore = totalRoad;
                tbLog.Dispatcher.Invoke(new Action(() => { tbLog.Text +="Best so far: "+ bestScore + "\r\n"; }));
                array.CopyTo(sweetRoad, 0);  
            }
        }
        private void ButtonSlove_Click(object sender, RoutedEventArgs e)
        {
            DoEverything(Roads);
        }
        public void DoEverything(Vector[] array)
        {
            tbLog.Clear();
            bestScore = double.MaxValue;
            for (int i = 0; i < pool.Length; i++)
            {
                
                pool[i] = new Thread(() =>
                {
                    TakeMeHome(array);

                });

            }
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].Start();

            }
        }

        public void TakeMeHome(Vector[] array)
        {
            int ctr = 0;
            while (true)
            {

                CalcRoad(array);
                if (ctr % 1000000 == 0)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() => {
                        SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(150,(byte)r.Next(1,255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
                        CanvasMap.Children.Clear();
                        Application.Current.Dispatcher.Invoke(new Action(() => { DrawRoads(sweetRoad, scb, 8); }));
                        DrawRoads(array, roadBrush, 3);
                    }));

                    ctr = 0;
                }

                RandomiizeArrayOrder(array);

                ctr++;
            }
        }
        


        private static double GetDistance(Vector A, Vector B)
        {
            double a = (double)(B.X - A.X);
            double b = (double)(B.Y - A.Y);

            return Math.Sqrt(a * a + b * b);
        }

        public void RandomiizeArrayOrder(Vector[] CountryRoads)
        {
            for (int t = 0; t < CountryRoads.Length; t++)
            {
                var tmp = CountryRoads[t];
                int place = r.Next(0, CountryRoads.Length);
                CountryRoads[t] = CountryRoads[place];
                CountryRoads[place] = tmp;
            }
        }

        public void GetAllCombinations(Vector[] tmpList)
        {
            

            

        }

        private Vector[] GetPermutations<T>()
        {
            throw new NotImplementedException();
        }

        private void ButtonStopSolving_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].Abort(null);
            }
            
        }

        private void ButtonShowBest_Click(object sender, RoutedEventArgs e)
        {
            CanvasMap.Children.Clear();
            DrawCities(sweetRoad);
            DrawRoads(sweetRoad, sweetBrush,5);
        }

        private void tbLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            tbLog.ScrollToEnd();
        }
        

        private void ButtonDrawGen_Click(object sender, RoutedEventArgs e)
        {
            
            CanvasMap.Children.Clear();
            
                genetics = new Genetics(ref CanvasMap, (int)SliderCitiesCounter.Value);
                genetics.GenerateDataSet();
                genetics.PushCitiesDataToAllObjects();
                genetics.Shake(genetics.PopulationData1);
                genetics.DrawGenCities(ref CanvasMap, firstCity);
                genetics.DrawGenRoads(CanvasMap, roadBrush);
            
        
            
        }
      
        private void ButtonStepp_Click(object sender, RoutedEventArgs e)
        {
            tbLog.Clear();
            
            threadGen = new Thread(() => {
                
                    for (int j = 0; j < 10000; j++)
                    {
                        genetics.CalculateRoadForEachElementInPopulation(genetics.PopulationData1, tbLog, CanvasMap, sweetBrush, j);
                        labelPopCount.Dispatcher.Invoke(new Action(() => 
                    {
                        labelPopCount.Content = "Generation: " + j; 

                    }));
                    }
                genVectors = genetics.PopulationData1;
                bestOrderFromGenVector = genetics.BestOrder;
              //  DoBruteForce(genetics.DataSet, bestOrderFromGenVector);
                
            });
            threadGen.Start();
              
        }

        private void DoBruteForce(Vector[] dataset, int[] order)
        {
            double bestSoFar = genetics.ShortestDistance;
            for (int i = 0; i < 20000; i++)
            {
                CalcRoad(dataset);

            }

        }

        private void buttonStopThread_Click(object sender, RoutedEventArgs e)
        {
            threadGen.Abort(null);
            CanvasMap.Children.Clear();
            genetics.DrawGenCities(ref CanvasMap, cityBrush);
            genetics.DrawBestRoads(CanvasMap, sweetBrush, tbLog);
            
            
        }

        private void ButtonAforgeBase_Click(object sender, RoutedEventArgs e)
        {
            Vector[] dataset = new Vector[(int)SliderCitiesCounter.Value];
            int[] order = new int[dataset.Length];
            int[] topOrder = null;
            double shotrestRoad = double.MaxValue;
            double topFitness = 0;
            for (int i = 0; i < dataset.Length; i++)
            {
                dataset[i] = new Vector(r.Next(50,(int) CanvasMap.ActualWidth), r.Next(50,(int) CanvasMap.ActualHeight));
                order[i] = i;
            }
            
            Aforge.AforgeChromosome aforgeChromosome = new Aforge.AforgeChromosome(order,dataset);
            Aforge.AforgeFitness aforgeFitness = new Aforge.AforgeFitness(order,dataset);
            Aforge.AforgeSelectionMethod aforgeSelectionMethod = new Aforge.AforgeSelectionMethod();
            Aforge.MyAforgePopulation myAforgePopulation = new Aforge.MyAforgePopulation(dataset.Length * 10, aforgeChromosome, aforgeFitness, new RankSelection());
            #region mt
            // Aforge.MyAforgePopulation[] AforgePopulationPool = new Aforge.MyAforgePopulation[1];
            // Thread[] aforgeThreadPool = new Thread[1];

            //for (int i = 0; i < AforgePopulationPool.Length; i++)
            //{
            //    AforgePopulationPool[i] = new Aforge.MyAforgePopulation(dataset.Length * 3, aforgeChromosome, aforgeFitness, new EliteSelection());
            //    AforgePopulationPool[i].MutationRate = 0.5;
            //    AforgePopulationPool[i].RandomSelectionPortion = 0.05;
            //}
            #endregion
            try
            {
                myAforgePopulation.MutationRate = double.Parse(tBoxMutationRate.Text);
                myAforgePopulation.RandomSelectionPortion = double.Parse(tBoxRandomPortion.Text);
                
            }
            catch(Exception ex)
            {
                myAforgePopulation.MutationRate = 0.1;
                myAforgePopulation.RandomSelectionPortion = 0.1;
                tbLog.Text += ex.ToString() + "\r\n";
                
            }
            if(chboxAutoShuffle.IsChecked == true)
            {
                myAforgePopulation.AutoShuffling = true;
            }
            else
            {
                myAforgePopulation.AutoShuffling = false;
            }

            tbLog.Text = "Population size: " + myAforgePopulation.Size + "\r\n";

            Thread trainingThread = new Thread(() => 
            {
                long generation = 0;
                while (topFitness < 0.0009)
                {
                    //ClearScreen

                    myAforgePopulation.RunEpoch(); //ThisMakesOneStepEachIIteration
                    if (generation % 1000 == 0)
                    {

                        tbLog.Dispatcher.Invoke(new Action(() =>
                    {
                            tbLog.Text += "Generation: " + generation + "\r\n";
                            tbLog.Text += myAforgePopulation.FitnessAvg + " avg\r\n";
                            tbLog.Text += myAforgePopulation.FitnessMax + " max\r\n";
                            tbLog.Text += topFitness + " Record\r\n";
                            tbLog.Text += shotrestRoad + " shortestroad\r\n";
                        
                        //tbLog.ScrollToEnd();

                    }));
                    }
                    generation++;
                    if (myAforgePopulation.FitnessMax > topFitness)
                    {
                        topFitness = myAforgePopulation.FitnessMax;
                    }
                    if (aforgeFitness.topScore < shotrestRoad)
                    {
                        shotrestRoad = aforgeFitness.topScore;
                        topOrder = aforgeFitness.topOrder;
                        CanvasMap.Dispatcher.Invoke(new Action(() =>
                        {
                            CanvasMap.Children.Clear();
                            DrawRoadsGenetic(dataset, topOrder, sweetBrush, 3);
                        }));
                    }


                    myAforgePopulation.Selection();
                    myAforgePopulation.Crossover();
                    myAforgePopulation.Mutate();
                    
                }
            });
            trainingThread.Start();
            #region commentMT
            //  for (int i = 0; i < aforgeThreadPool.Length; i++)
            //   {
            //    aforgeThreadPool[0] = new Thread(() =>
            //    {
            //        while (topFitness < 0.0009)
            //        {
            //            //ClearScreen

            //            AforgePopulationPool[0].RunEpoch(); //ThisMakesOneStepEachIIteration


            //            tbLog.Dispatcher.Invoke(new Action(() =>
            //            {
            //                tbLog.Text += AforgePopulationPool[0].FitnessAvg + " avg\r\n";
            //                tbLog.Text += AforgePopulationPool[0].FitnessMax + " max\r\n";
            //                tbLog.Text += topFitness + " Record\r\n";
            //                tbLog.Text += shotrestRoad + " shortestroad\r\n";

            //                tbLog.ScrollToEnd();

            //            }));

            //            if (AforgePopulationPool[0].FitnessMax > topFitness)
            //            {
            //                topFitness = AforgePopulationPool[0].FitnessMax;
            //            }
            //            if (aforgeFitness.topScore < shotrestRoad)
            //            {
            //                shotrestRoad = aforgeFitness.topScore;
            //                topOrder = aforgeFitness.topOrder;
            //                CanvasMap.Dispatcher.Invoke(new Action(() =>
            //                {
            //                    CanvasMap.Children.Clear();
            //                    DrawRoadsGenetic(dataset, topOrder, sweetBrush, 3);
            //                }));
            //            }


            //            AforgePopulationPool[0].Mutate();
            //            AforgePopulationPool[0].Selection();
            //            //ThenBestChromosomeShouldBeSelected
            //            //  AforgePopulation.Crossover();//NewPopulationShouldBeMade
            //            //Mutate'em
            //            //DrawWithDispacher
            //        }
            //    });
            //aforgeThreadPool[0].Start();
            //  }
            #endregion
        }

        public void DrawRoadsGenetic(Vector[] array,int[]order, SolidColorBrush brush, int thicc)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                //works almost ok
                Line line = new Line()
                {
                    X1 = array[order[i]].X,
                    X2 = array[order[i + 1]].X,
                    Y1 = array[order[i]].Y,
                    Y2 = array[order[i + 1]].Y,
                    Fill = null,
                    Stroke = brush,
                    StrokeThickness = thicc,
                };
                CanvasMap.Dispatcher.Invoke(new Action(() => { CanvasMap.Children.Add(line); }));

                if (i == array.Length - 2)
                {
                    Line line2 = new Line()
                    {
                        X1 = array[order[i + 1]].X,
                        X2 = array[order[0]].X,
                        Y1 = array[order[i + 1]].Y,
                        Y2 = array[order[0]].Y,
                        Fill = null,
                        Stroke = brush,
                        StrokeThickness = thicc,
                    };
                    CanvasMap.Dispatcher.Invoke(new Action(() => { CanvasMap.Children.Add(line2); }));

                }
            }

        }
    }
}
