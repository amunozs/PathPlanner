using PathPlanner.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PathPlanner.ViewModel
{
		public partial class MainViewModel
		{
				public ICommand DrawPointsCommand { get; set; }

				public ICommand NextCommand { get; set; }

				public ICommand AutoCommand { get; set; }

				private void CreateCommands()
				{
						DrawPointsCommand = new RelayCommand(DrawPointsExecute, DrawPointsCanExecute);
						NextCommand = new RelayCommand(NextExecute, NextCanExecute);
						AutoCommand = new RelayCommand(AutoExecute, AutoCanExecute);
				}



				private bool DrawPointsCanExecute(object parameter)
				{
						return true;
				}

				public void DrawPointsExecute(object parameter)
				{
						MapCanvas.Children.Clear();
						foreach (Force force in planner.Forces)
						{
								if (force.Point.PosX == planner.End.PosX && force.Point.PosY == planner.End.PosY)
								{
										continue;
								}
								Ellipse ellipse = new Ellipse();
								ellipse.Fill = Brushes.Green;
								ellipse.Width = 3;
								ellipse.Height = 3;
								ellipse.StrokeThickness = 2;
								MapCanvas.Children.Add(ellipse);
								Canvas.SetLeft(ellipse, force.Point.PosX);
								Canvas.SetTop(ellipse, force.Point.PosY);
						}

						Ellipse start = new Ellipse();
						start.Fill = Brushes.Blue;
						start.Width = 3;
						start.Height = 3;
						start.StrokeThickness = 2;
						MapCanvas.Children.Add(start);
						Canvas.SetLeft(start, planner.Start.PosX);
						Canvas.SetTop(start, planner.Start.PosY);

						Ellipse goal = new Ellipse();
						goal.Fill = Brushes.Red;
						goal.Width = 3;
						goal.Height = 3;
						goal.StrokeThickness = 2;
						MapCanvas.Children.Add(goal);
						Canvas.SetLeft(goal, planner.End.PosX);
						Canvas.SetTop(goal, planner.End.PosY);

						Ellipse actual = new Ellipse();
						actual.Fill = Brushes.Green;
						actual.Width = 5;
						actual.Height = 5;
						actual.StrokeThickness = 4;
						MapCanvas.Children.Add(actual);
						Canvas.SetLeft(actual, planner.Actual.PosX);
						Canvas.SetTop(actual, planner.Actual.PosY);

						/*
						foreach (Model.Point point in planner.Path)
						{
								if (point.PosX == planner.End.PosX && point.PosY == planner.End.PosY)
								{
										continue;
								}

								if (point.PosX == planner.Start.PosX && point.PosY == planner.Start.PosY)
								{
										continue;
								}

								Ellipse ellipse = new Ellipse();
								ellipse.Fill = Brushes.Green;
								ellipse.Width = 3;
								ellipse.Height = 3;
								ellipse.StrokeThickness = 2;
								MapCanvas.Children.Add(ellipse);
								Canvas.SetLeft(ellipse, point.PosX);
								Canvas.SetTop(ellipse, point.PosY);
						}
						*/
				}

				private bool NextCanExecute(object parameter)
				{
						return true;
				}

				public void NextExecute(object parameter)
				{
						planner.NextStep();
						DrawPointsExecute(parameter);

				}

				private bool DrawPathCanExecute(object parameter)
				{
						return true;
				}

				public void DrawPathExecute (object parameter)
				{
						foreach (Model.Point point in planner.Path)
						{
								if (point.PosX == planner.End.PosX && point.PosY == planner.End.PosY)
								{
										continue;
								}

								if (point.PosX == planner.Start.PosX && point.PosY == planner.Start.PosY)
								{
										continue;
								}

								Ellipse ellipse = new Ellipse();
								ellipse.Fill = Brushes.Green;
								ellipse.Width = 3;
								ellipse.Height = 3;
								ellipse.StrokeThickness = 2;
								MapCanvas.Children.Add(ellipse);
								Canvas.SetLeft(ellipse, point.PosX);
								Canvas.SetTop(ellipse, point.PosY);
						}
				}
				
				private bool AutoCanExecute(object parameter)
				{
						return true;
				}


				
				public void AutoExecute(object parameter)
				{
						_timer.Tick += new EventHandler(async (object s, EventArgs a) =>
						{
								if (planner.NextStep())
								{
										_timer.Stop();
										DrawPathExecute(parameter);
								}
								DrawPointsExecute(parameter);
						});
						_timer.Start();
				}


		}
}
