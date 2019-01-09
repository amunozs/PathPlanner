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

				public MainViewModel(Canvas mapCanvas)
				{
						CreateCommands();
						ImgPath = @"C:\MUIA\RobotsAutonomos\practica3\Test\PathPlanner\Maps\TestMap.png";

						MapCanvas = mapCanvas;
						Bitmap bm = new Bitmap(ImgPath);
						Model.Point start = new Model.Point (200,40);
						Model.Point end = new Model.Point (400,200);
						planner = new Model.Planner(bm, start,end);
						_timer = new DispatcherTimer();
						WaitTimeMs = 50;
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
