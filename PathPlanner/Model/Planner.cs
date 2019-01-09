using System;
using System.Collections.Generic;
using System.Drawing;

namespace PathPlanner.Model
{
		public class Planner
		{
				private int[,] _grid;
				private int _width;
				private int _height;

				public List<Force> Forces { get; set; }
				public Point Actual { get; set; }
				public Point Start { get; set; }
				public double GoalAttraction { get; set; }
				public double PointRepulsion { get; set; }

				public List<Point> Path { get; set; }

				private bool _actual_frozen = true;

				private Point _end = new Point (0,0);
				public Point End
				{
						get => _end;
						set
						{
								if (value.PosX != _end.PosX || value.PosY != _end.PosY)
								{
										for (int i = 0; i < Forces.Count; i++)
										{
												if (Forces[i].Point.PosX != _end.PosX || Forces[i].Point.PosY != _end.PosY )
												{
														Forces.RemoveAt(i);
												}
										}
										Force f = new Force();
										f.Point = value;
										f.Strength = GoalAttraction;
										Forces.Add(f);
										_end = value;
								}
						}
				}

				public Planner(Bitmap map, Point start, Point end)
				{
						GetGrid(map);
						Forces = new List<Force>();
						Start = start;
						End = end;
						GoalAttraction = 100;
						PointRepulsion = 1;
				}

				public bool NextStep ()
				{
						if (_actual_frozen)
						{
								Actual = new Point(Start.PosX, Start.PosY);
								Path = new List<Point>();
						}

						List<Point> neighs = GetValidNeighs(Actual);
						double maxForce = GetForce (Actual);			
						if (_actual_frozen)
						{
								maxForce = double.NegativeInfinity;
						}
						Point maxForceNeigh = null;
						bool freezeActualPoint = true;
						foreach (Point neigh in neighs)
						{
								double force = GetForce(neigh);
								if (force > maxForce)
								{
										maxForce = force;
										maxForceNeigh = neigh;
										freezeActualPoint = false;
								}
						}

						if (!freezeActualPoint)
						{
								Path.Add(Actual);
								Actual = maxForceNeigh;
								_actual_frozen = false;							
						}

						else
						{
								Force force = new Force() { Point = Actual, Strength = - PointRepulsion };
								//Forces[0].Strength++;
								Forces.Add(force);
								_actual_frozen = true;
								if (Actual.PosX == End.PosX && Actual.PosY == End.PosY)
								{
										return true;
								}
						}

						return false;

				}

				public void Reset ()
				{
						Forces = new List<Force>();
						Force f = new Force();
						f.Point = End;
						f.Strength = 100;
						Forces.Add(f);
						Actual = null;
						_actual_frozen = true;
				}

				private double GetForce (Point point)
				{
						double force = 0;
						if (_grid[point.PosX, point.PosY] == 0)
						{
								return 0;
						}
						foreach (Force f in Forces)
						{
								double dist = (Math.Pow(point.PosX - f.Point.PosX, 2) + Math.Pow(point.PosY - f.Point.PosY, 2));
								if (dist < 0)
								{
										dist = -dist;
								}
								force += f.Strength / dist;
						}
						
						return force;
				}

				private List<Point> GetValidNeighs (Point point)
				{
						List<Point> possibleNeighs = new List<Point>();

						possibleNeighs.Add(new Point(point.PosX - 1, point.PosY - 1));
						possibleNeighs.Add(new Point(point.PosX - 1, point.PosY));
						possibleNeighs.Add(new Point(point.PosX - 1, point.PosY + 1));
						possibleNeighs.Add(new Point(point.PosX, point.PosY -1));
						possibleNeighs.Add(new Point(point.PosX, point.PosY + 1));
						possibleNeighs.Add(new Point(point.PosX + 1, point.PosY - 1));
						possibleNeighs.Add(new Point(point.PosX + 1, point.PosY));
						possibleNeighs.Add(new Point(point.PosX + 1, point.PosY + 1));

						List<Point> validNeighs = new List<Point>();
						foreach (Point p in possibleNeighs)
						{
								if (IsValid(p))
								{
										validNeighs.Add(p);
								}
						}
						return validNeighs;
				}

				private bool IsValid (Point point)
				{
						if (point.PosX < 0 || point.PosX >= _width)
						{
								return false;
						}
						if (point.PosY < 0 || point.PosY >= _height)
						{
								return false;
						}

						foreach (Force f in Forces)
						{
								if (f.Point.PosX == End.PosX && f.Point.PosY == End.PosY)
								{
										continue;
								}
								if (f.Point.PosX == point.PosX && f.Point.PosY == point.PosY)
								{
										return false;
								}
						}

						return true;
				}

				private void GetGrid(Bitmap bitmap)
				{
						_width = bitmap.Size.Width;
						_height = bitmap.Size.Height;
						_grid = new int[_width, _height];

						for (int x = 0; x < bitmap.Width; x++)
						{
								for (int y = 0; y < bitmap.Height; y++)
								{
										var pixel = bitmap.GetPixel(x, y);
										if (pixel.R > 100)
										{
												_grid[x, y] = 1;
										}
										else
										{
												_grid[x, y] = 0; 
										}
								}
						}
				}
		}
}
