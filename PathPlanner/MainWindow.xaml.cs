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

				bool IsMouseDown;
				double xOffset;
				double yOffset;
				Point initial;

				private void MapCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
				{
						IsMouseDown = true;
						initial = Mouse.GetPosition(this);
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
						IsMouseDown = false;
				}

				private void MapCanvas_MouseMove(object sender, MouseEventArgs e)
				{
						if (! (Mouse.LeftButton == MouseButtonState.Pressed)) return;
						
						((MainViewModel)DataContext).XOffset = e.GetPosition(this).X - initial.X;
						((MainViewModel)DataContext).YOffset = e.GetPosition(this).Y - initial.Y;
				}
		}
}
