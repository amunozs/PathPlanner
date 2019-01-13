using System;
using System.Collections.Generic;

namespace PathPlanner.Model
{
		public class Planner
		{
				private int[,] _grid;
				private int _width;
				private int _height;

				public List<Force> Forces { get; set; }
				//public List<Point> Actual { get; set; }
				public Force Start { get; set; }

				private double _goalAttraction;
				public double GoalAttraction
				{
						get => _goalAttraction;
						set
						{
								_goalAttraction = value;
								End.Strength = _goalAttraction;
						}
				}

				private double _pointRepulsion;
				public double PointRepulsion

				{
						get => _pointRepulsion;
						set
						{
								_pointRepulsion = value;
								for (int i = 1; i < Forces.Count; i++)
								{
										Forces[i].Strength = - _pointRepulsion;
								}
						}
				}

				//public List<Force> Path { get; set; }

				private Force _end;
				public Force End
				{
						get => _end;
						set
						{
								if (value.PosX != _end.PosX || value.PosY != _end.PosY)
								{								
										_end.PosX = value.PosX;
										_end.PosY = value.PosY;
								}
						}
				}

				public Planner(System.Drawing.Bitmap map, Force start, Force end)
				{
						GetGrid(map);
						Forces = new List<Force>();
						Start = start;
						_end = end;
						End = end;
						End.IsGoal = true;
						GoalAttraction = 100;
						PointRepulsion = 1;
						//Actual = new List<Point>();
				}

				private bool NextStepInternal (int index)
				{

						List<Force> neighs = GetValidNeighs(Forces[index]);
						double maxForce = double.NegativeInfinity;
						if (Forces[index].Frozen)
						{
								maxForce = double.NegativeInfinity;
						}
						Force maxForceNeigh = null;
						bool freezeActualPoint = true;
						foreach (Force neigh in neighs)
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
								//Path.Add(Actual[index]);
								Forces[index].PosX = maxForceNeigh.PosX;
								Forces[index].PosY = maxForceNeigh.PosY;
								//Forces[index].Frozen = false;
						}

						else
						{
								//Force force = new Force() { Point = Actual[index], -PointRepulsion };
								//Forces[0].Strength++;
								//Forces.Add(force);
								//Forces[index].Frozen = true;
								if (Forces[index].PosX == End.PosX && Forces[index].PosY == End.PosY)
								{
										return true;
								}
						}

						return false;

				}

				public bool NextStep ()
				{
						if (Forces.Count < 300)
						{
								Forces.Add(new Force(Start.PosX, Start.PosY,-PointRepulsion));
								//Path = new List<Point>();
						}
						for (int i = 0; i < Forces.Count; i ++)
						{
								if (NextStepInternal(i))
								{
										return true;
								}
						}
						return false;
				}

				public void Reset ()
				{
						Forces = new List<Force>();
						Forces.Add(_end);
						//Actual = new List<Point>();
				}

				private double GetForce (Force point)
				{
						double force = 0;
						if (_grid[point.PosX, point.PosY] == 0)
						{
								return double.NegativeInfinity;
						}

						double dist = (Math.Pow(point.PosX - End.PosX, 2) + Math.Pow(point.PosY - End.PosY, 2));
						if (dist < 0)
						{
								dist = -dist;
						}
						force += End.Strength / dist;

						foreach (Force f in Forces)
						{
								if (f == point)
								{
										continue;
								}
								dist = (Math.Pow(point.PosX - f.PosX, 2) + Math.Pow(point.PosY - f.PosY, 2));
								if (dist < 0)
								{
										dist = -dist;
								}
								force += f.Strength / dist;
						}

						return force;
				}

				private List<Force> GetValidNeighs (Force point)
				{
						List<Force> possibleNeighs = new List<Force>();

						possibleNeighs.Add(new Force(point.PosX - 1, point.PosY - 1));
						possibleNeighs.Add(new Force(point.PosX - 1, point.PosY));
						possibleNeighs.Add(new Force(point.PosX - 1, point.PosY + 1));
						possibleNeighs.Add(new Force(point.PosX, point.PosY -1));
						possibleNeighs.Add(new Force(point.PosX, point.PosY + 1));
						possibleNeighs.Add(new Force(point.PosX + 1, point.PosY - 1));
						possibleNeighs.Add(new Force(point.PosX + 1, point.PosY));
						possibleNeighs.Add(new Force(point.PosX + 1, point.PosY + 1));

						List<Force> validNeighs = new List<Force>();
						foreach (Force p in possibleNeighs)
						{
								if (IsValid(p))
								{
										validNeighs.Add(p);
								}
						}
						return validNeighs;
				}

				private bool IsValid (Force point)
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
								if (f.IsGoal)
								{
										continue;
								}
								if (f.PosX == point.PosX && f.PosY == point.PosY)
								{
										return false;
								}
						}

						return true;
				}

				private void GetGrid(System.Drawing.Bitmap bitmap)
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
