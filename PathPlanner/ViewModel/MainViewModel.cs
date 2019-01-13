using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PathPlanner.ViewModel
{
		public enum State
		{
				OpenMap,
				SelectStartGoal,
				GetVertices,
				GetNeighs,
				CalculatePath
		}

		public partial class MainViewModel : INotifyPropertyChanged
		{
				public State state;
				public string ImgPath { get; set; }
				public Canvas MapCanvas {get;set;}
				public Model.Planner planner;
				private DispatcherTimer _timer;
				private bool _running;
				private Bitmap _bm;

				private double _zoom = 1;
				public double Zoom
				{
						get => _zoom;
						set
						{
								_zoom = value;
								OnPropertyChanged("Zoom");
						}
				}

				private double _xoffset = 0;
				public double XOffset
				{
						get => _xoffset;
						set
						{
								_xoffset = value;
								OnPropertyChanged("XOffset");
						}
				}

				private double _yoffset = 0;
				public double YOffset
				{
						get => _yoffset;
						set
						{
								_yoffset = value;
								OnPropertyChanged("YOffset");
						}
				}

				private bool _plannerCreated = false;
				public bool PlannerCreated
				{
						get => _plannerCreated;
						set
						{
								_plannerCreated = value;
								OnPropertyChanged("PlannerCreated");
						}
				}

				public void CreatePlanner ()
				{
						planner = new Model.Planner(_bm, Start, End);
						PlannerCreated = true;
				}

				public string GoalAttraction
				{
						get
						{
								if (PlannerCreated)
								{
										return planner.GoalAttraction.ToString("0.##");
								}
								else return "100";
						}
						set
						{
								bool ok = double.TryParse(value, out double goalAttraction);
								if (ok && PlannerCreated)
								{
										planner.GoalAttraction = goalAttraction;
								}
								OnPropertyChanged("GoalAttraction");
						}
				}

				public string PreviousRepulsion
				{
						get
						{
								if (PlannerCreated)
								{
										return planner.PointRepulsion.ToString("0.##");
								}
								else return "-1";
						}
						set
						{
								bool ok = double.TryParse(value, out double previousRepulsion);
								if (ok && PlannerCreated)
								{
										planner.PointRepulsion = previousRepulsion;
								}
								OnPropertyChanged("PreviousRepulsion");
						}
				}

				public Model.Force Start { get; set; }
				public Model.Force End { get; set; }

				public MainViewModel(Canvas mapCanvas)
				{
						CreateCommands();
						ImgPath = @"C:\MUIA\RobotsAutonomos\practica3\Test\PathPlanner\Maps\TestMap.png";

						MapCanvas = mapCanvas;
						_bm = new Bitmap(ImgPath);
						Model.Force start = Start;
						Model.Force end = End;
						
						_timer = new DispatcherTimer();
						WaitTimeMs = 50;
						_running = false;
						GoalAttraction = "100";
						PreviousRepulsion = "1";
				}

				public event PropertyChangedEventHandler PropertyChanged;
				protected void OnPropertyChanged(string name)
				{
						PropertyChangedEventHandler handler = PropertyChanged;
						if (handler != null)
						{
								handler(this, new PropertyChangedEventArgs(name));
						}
				}

				private int _waitTime;

				public int WaitTimeMs
				{
						get => _waitTime;
						set
						{
								_waitTime = value;
								_timer.Interval = TimeSpan.FromMilliseconds(100 - _waitTime);
								OnPropertyChanged("WaitTimeMs");
						}
				}
		}
}
