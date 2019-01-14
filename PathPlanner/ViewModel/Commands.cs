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

				public ICommand DrawPathCommand { get; set; }

				public ICommand NextCommand { get; set; }

				public ICommand PlayCommand { get; set; }

				public ICommand PauseCommand { get; set; }

				public ICommand StopCommand { get; set; }

				private void CreateCommands()
				{
						DrawPointsCommand = new RelayCommand(DrawPointsExecute, DrawPointsCanExecute);
						DrawPathCommand = new RelayCommand(DrawPathExecute, DrawPathCanExecute);
						NextCommand = new RelayCommand(NextExecute, NextCanExecute);
						PlayCommand = new RelayCommand(PlayExecute, PlayCanExecute);
						PauseCommand = new RelayCommand(PauseExecute, PauseCanExecute);
						StopCommand = new RelayCommand(StopExecute, StopCanExecute);
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
								if (force.Pos.X == planner.End.Pos.X && force.Pos.Y == planner.End.Pos.Y)
								{
										continue;
								}

								Ellipse ellipse = new Ellipse();

								if (force.Frozen)
								{
										ellipse.Width = 3;
										ellipse.Height = 3;
										ellipse.StrokeThickness = 2;
								}
								else
								{
										ellipse.Width = 5;
										ellipse.Height = 5;
										ellipse.StrokeThickness = 4;
								}

								ellipse.Fill = Brushes.Blue;
								MapCanvas.Children.Add(ellipse);
								Canvas.SetLeft(ellipse, force.Pos.X);
								Canvas.SetTop(ellipse, force.Pos.Y);
						}

						Ellipse start = new Ellipse();
						start.Fill = Brushes.Green;
						start.Width = 5;
						start.Height = 5;
						start.StrokeThickness = 4;
						MapCanvas.Children.Add(start);
						Canvas.SetLeft(start, planner.Start.Pos.X);
						Canvas.SetTop(start, planner.Start.Pos.Y);

						Ellipse goal = new Ellipse();
						goal.Fill = Brushes.Red;
						goal.Width = 5;
						goal.Height = 5;
						goal.StrokeThickness = 4;
						MapCanvas.Children.Add(goal);
						Canvas.SetLeft(goal, planner.End.Pos.X);
						Canvas.SetTop(goal, planner.End.Pos.Y);
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
						MapCanvas.Children.Clear();
						foreach (Model.Point point in planner.Path)
						{
								if (point.X == planner.End.Pos.X && point.Y == planner.End.Pos.Y)
								{
										continue;
								}

								if (point.X == planner.Start.Pos.X && point.Y == planner.Start.Pos.Y)
								{
										continue;
								}

								Ellipse ellipse = new Ellipse();
								ellipse.Fill = Brushes.Blue;
								ellipse.Width = 3;
								ellipse.Height = 3;
								ellipse.StrokeThickness = 2;
								MapCanvas.Children.Add(ellipse);
								Canvas.SetLeft(ellipse, point.X);
								Canvas.SetTop(ellipse, point.Y);
						}
						
				}

				private bool DrawStartEndCanExecute(object parameter)
				{
						return true;
				}

				public void DrawStartEndExecute(object parameter)
				{
						int[] point = parameter as int[];

						Ellipse start = new Ellipse();
						start.Fill = Brushes.Green;
						start.Width = 5;
						start.Height = 5;
						start.StrokeThickness = 4;
						MapCanvas.Children.Add(start);
						Canvas.SetLeft(start, planner.Start.Pos.X);
						Canvas.SetTop(start, planner.Start.Pos.Y);

				}

				private bool PlayCanExecute(object parameter)
				{
						return !_running && PlannerCreated;
				}

				public void PlayExecute(object parameter)
				{
						_timer.Tick += new EventHandler(async (object s, EventArgs a) =>
						{
								if (planner.NextStep())
								{
										DrawPathCommand.Execute(parameter);
										_timer.Stop();
										
										_running = false; 
								}
								else
								{
										DrawPointsExecute(parameter);
								}
								
						});
						_timer.Start();
						_running = true;
				}

				private bool PauseCanExecute(object parameter)
				{
						return _running;
				}

				public void PauseExecute(object parameter)
				{
						_timer.Stop();
						_running = false;
				}

				private bool StopCanExecute(object parameter)
				{
						return true;
				}

				public void StopExecute(object parameter)
				{
						_timer.Stop();
						planner.Reset();
						/*
						Model.Point start = new Model.Point(200, 40);
						Model.Point end = new Model.Point(400, 200);
						planner = new Model.Planner(_bm, start, end);
						*/
						DrawPointsExecute(parameter);
						_running = false;
				}

		}
}
