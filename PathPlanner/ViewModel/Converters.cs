using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PathPlanner.ViewModel
{
		public class StateToTextConverter : IValueConverter
		{
				public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
				{
						State state = (State)value;
						switch (state)
						{
								case State.OpenMap:
										return "State 0: Open map";

								case State.SelectStartGoal:
										return "State 1: Select start and goal points (click on map)";

								case State.GetVertices:
										return "State 2: Calculate the graph looking for the vertices";

								case State.GetNeighs:
										return "State 3: Get neighbours on the graph";

								case State.CalculatePath:
										return "State 4: Calculate path using Dijkstra";

								default:
										return "";
						}
				}

				public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
				{
						throw new NotImplementedException();
				}
		}
}
