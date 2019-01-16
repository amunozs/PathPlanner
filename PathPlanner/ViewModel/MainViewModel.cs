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
				private MainWindow _mainWindow;

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

				private int _maxUnfrozen = 100;
				public string MaxUnfrozen
				{
						get => _maxUnfrozen.ToString();
						set
						{
								bool ok = int.TryParse(value, out int result);
								if (ok)
								{
										_maxUnfrozen = result;
										if (PlannerCreated)
										{
												planner.MaxUnfrozen = _maxUnfrozen;
										}
										OnPropertyChanged("MaxUnfrozen");
								}	
						}
				}

				private int _stepsToFreeze = 100;
				public string StepsToFreeze
				{
						get => _stepsToFreeze.ToString();
						set
						{
								bool ok = int.TryParse(value, out int result);
								if (ok)
								{
										_stepsToFreeze = result;
										if (PlannerCreated)
										{
												planner.StepsToFreeze = _stepsToFreeze;
										}
										OnPropertyChanged("StepsToFreeze");
								}
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

				private bool _mapVisible = false;
				public bool MapVisible
				{
						get => _mapVisible;
						set
						{

								_mapVisible = value;
								OnPropertyChanged("MapVisible");
								OnPropertyChanged("MapNotVisible");
								OnPropertyChanged("StartNotAdded");
								OnPropertyChanged("GoalNotAdded");
						}
				}

				public bool MapNotVisible
				{
						get => !_mapVisible;
				}

				private bool _startNotAdded = true;
				public bool StartNotAdded
				{
						get => _startNotAdded && MapVisible;
						set
						{
								_startNotAdded = value;
								OnPropertyChanged("StartNotAdded");
								OnPropertyChanged("GoalNotAdded");
						}
				}

				private bool _goalNotAdded = true;
				public bool GoalNotAdded
				{
						get => _goalNotAdded && !StartNotAdded && MapVisible;
						set
						{
								_goalNotAdded = value;
								OnPropertyChanged("GoalNotAdded");
						}
				}

				public void CreatePlanner ()
				{
						planner = new Model.Planner(_bm, Start, End);
						planner.GoalAttraction = _goalAttraction;
						planner.PointRepulsion = _previousRepulsion;
						planner.MaxUnfrozen = _maxUnfrozen;
						planner.StepsToFreeze = _stepsToFreeze;
						PlannerCreated = true;
				}

				private double _goalAttraction;
				public string GoalAttraction
				{
						get => _goalAttraction.ToString("0.##");
						set
						{
								bool ok = double.TryParse(value, out double goalAttraction);
								if (ok)
								{
										_goalAttraction = goalAttraction;
										if (PlannerCreated)
										{
												planner.GoalAttraction = goalAttraction;
										}
									
								}
								OnPropertyChanged("GoalAttraction");
						}
				}

				private double _previousRepulsion;
				public string PreviousRepulsion
				{
						get => _previousRepulsion.ToString("0.##");
						set
						{
								bool ok = double.TryParse(value, out double previousRepulsion);
								if (ok)
								{
										_previousRepulsion = previousRepulsion;
										if (PlannerCreated)
										{
												planner.GoalAttraction = previousRepulsion;
										}

								}
								OnPropertyChanged("PreviousRepulsion");
						}
				}

				public Model.Force Start { get; set; }
				public Model.Force End { get; set; }

				public MainViewModel(Canvas mapCanvas, MainWindow mainWindow)
				{
						_mainWindow = mainWindow;
						CreateCommands();				
						MapCanvas = mapCanvas;			
						Model.Force start = Start;
						Model.Force end = End;
						
						_timer = new DispatcherTimer();
						WaitTimeMs = 50;
						_running = false;
						GoalAttraction = "1000";
						PreviousRepulsion = "1";
						MaxUnfrozen = "100";
						StepsToFreeze = "10";
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
