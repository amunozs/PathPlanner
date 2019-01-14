using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathPlanner.Model
{
		public class Point
		{
				public int X;
				public int Y;
				public Point(int x, int y)
				{
						X = x;
						Y = y;
				}
		}


		public class Force
		{
				public double Strength;
				public bool IsGoal = false;

				public Point Pos;
				private bool _frozen;
				public bool Frozen
				{
						get => _frozen;
						set
						{
								if (_frozen  && value)
								{
										ConsecutiveFrozen++;
								}
								_frozen = value;
						}
				}

				public int ConsecutiveFrozen = 0;

				public List<Point> Path;

				public Force(int posX, int posY, double strength = 0, bool isGoal = false)
				{

						Pos = new Point(posX,posY);
						IsGoal = isGoal;
						Strength = strength;
						Path = new List<Point>();
		}
				public void SavePoint()
				{
						Path.Add(new Point(Pos.X, Pos.Y));
				}

		}
}
