using PathPlanner.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PathPlanner
{
		/// <summary>
		/// Interaction logic for MainWindow.xaml
		/// </summary>
		public partial class MainWindow : Window
		{
				public MainWindow()
				{
						InitializeComponent();
						InitializeComponent();
						MainViewModel vm = new MainViewModel(MapCanvas);
						this.DataContext = vm;
				}

				bool first = false;
				bool entering = false;
				public double xOffset;
				public double yOffset;
				Point initial;

				private void MapCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
				{
						entering = false;
						first = true;
						initial = new Point(Mouse.GetPosition(this).X - xOffset, Mouse.GetPosition(this).Y - yOffset);
				}

				const double ScaleRate = 1.1;
				private void MapCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
				{
						if (e.Delta > 0)
						{
								((MainViewModel)DataContext).Zoom *= ScaleRate;
						}
						else
						{
								((MainViewModel)DataContext).Zoom /= ScaleRate;
						}
				}

				private void MapCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
				{
						
				}

				private void MapCanvas_MouseMove(object sender, MouseEventArgs e)
				{
						if (! (Mouse.LeftButton == MouseButtonState.Pressed) || entering) return;
						if (first)
						{
								xOffset = e.GetPosition(this).X - initial.X;
								yOffset = e.GetPosition(this).Y - initial.Y;
								first = false;
						}
						else
						{
								xOffset = e.GetPosition(this).X - initial.X;
								yOffset = e.GetPosition(this).Y - initial.Y;
						}
						
						((MainViewModel)DataContext).XOffset = xOffset;
						((MainViewModel)DataContext).YOffset = yOffset;
				}

				bool start = false;
				bool end = false;
				private void MapCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
				{
						if (! start)
						{
								int x = (int)e.GetPosition(MapCanvas).X;
								int y = (int)e.GetPosition(MapCanvas).Y;
								Model.Force f = new Model.Force(x, y);
								((MainViewModel)DataContext).Start = f;
								start = true;
								((MainViewModel)DataContext).DrawStartEndExecute(true);
						}
						else if (!end)
						{
								Model.Force f = new Model.Force((int)e.GetPosition(MapCanvas).X, (int)e.GetPosition(MapCanvas).Y);
								((MainViewModel)DataContext).End = f;
								end = true;
								((MainViewModel)DataContext).DrawStartEndExecute(false);
								((MainViewModel)DataContext).CreatePlanner();
						}
				}

				private void MapCanvas_MouseLeave(object sender, MouseEventArgs e)
				{
						
				}

				private void MapCanvas_MouseEnter(object sender, MouseEventArgs e)
				{
						entering = true;
				}
		}
}
