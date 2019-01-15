using System;
using System.Collections.Generic;

namespace PathPlanner.Model
{
		public class Planner
		{

				const int _maxConsecUnfozen = 1;

				public List<Point> Path;

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

				private int _maxUnfrozen;
				public int MaxUnfrozen
				{
						get => _maxUnfrozen;
						set
						{
								_maxUnfrozen = value;
						}
				}

				//public List<Force> Path { get; set; }

				private Force _end;
				public Force End
				{
						get => _end;
						set
						{
								if (value.Pos.X != _end.Pos.X || value.Pos.Y != _end.Pos.Y)
								{								
										_end.Pos.X = value.Pos.X;
										_end.Pos.Y = value.Pos.Y;
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
						double maxForce = GetForce(Forces[index], Forces[index]);
						if (Forces[index].ConsecutiveFrozen > _maxConsecUnfozen)
						{
								return false;
								maxForce = double.NegativeInfinity;
						}
						Force maxForceNeigh = null;
						bool freezeActualPoint = true;
						foreach (Force neigh in neighs)
						{
								double force = GetForce(neigh, Forces[index]);
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
								Forces[index].SavePoint();
								Forces[index].Pos.X = maxForceNeigh.Pos.X;
								Forces[index].Pos.Y = maxForceNeigh.Pos.Y;
								//Forces[index].Frozen = false;
						}

						else
						{
								//Force force = new Force() { Point = Actual[index], -PointRepulsion };
								//Forces[0].Strength++;
								//Forces.Add(force);
								Forces[index].Frozen = true;
								if (Forces[index].Pos.X == End.Pos.X && Forces[index].Pos.Y == End.Pos.Y)
								{
										Path = Forces[index].Path;
										return true;
								}
						}

						return false;

				}

				public bool NextStep ()
				{
						int notFrozen = 0;
						int count = 0;
						foreach(Force f in Forces)
						{
								count++;

								if (f.ConsecutiveFrozen < _maxConsecUnfozen)
								{
										notFrozen++;
								}
								/*
								if (!freeze)
								{
										f.Frozen = false;
								}*/
								if (!f.Frozen)
								{
										notFrozen++;
								}
						}

						if(notFrozen < _maxUnfrozen)
						{
								Forces.Add(new Force(Start.Pos.X, Start.Pos.Y,-PointRepulsion));
								count++;
								//Path = new List<Point>();
						}
						for (int i = 0; i < count; i ++)
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

				private double GetForce (Force point, Force original)
				{
						double force = 0;
						if (_grid[point.Pos.X, point.Pos.Y] == 0)
						{
								return double.NegativeInfinity;
						}

						double dist = Math.Sqrt((point.Pos.X - End.Pos.X)*(point.Pos.X - End.Pos.X) + (point.Pos.Y - End.Pos.Y)* (point.Pos.Y - End.Pos.Y));
						if (dist < 0)
						{
								dist = -dist;
						}
						force += End.Strength / dist;

						foreach (Force f in Forces)
						{
								if (f == original)
								{
										continue;
								}
								dist = Math.Sqrt(Math.Pow(point.Pos.X - f.Pos.X, 2) + Math.Pow(point.Pos.Y - f.Pos.Y, 2));
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

						possibleNeighs.Add(new Force(point.Pos.X - 1, point.Pos.Y - 1));
						possibleNeighs.Add(new Force(point.Pos.X - 1, point.Pos.Y));
						possibleNeighs.Add(new Force(point.Pos.X - 1, point.Pos.Y + 1));
						possibleNeighs.Add(new Force(point.Pos.X, point.Pos.Y -1));
						possibleNeighs.Add(new Force(point.Pos.X, point.Pos.Y + 1));
						possibleNeighs.Add(new Force(point.Pos.X + 1, point.Pos.Y - 1));
						possibleNeighs.Add(new Force(point.Pos.X + 1, point.Pos.Y));
						possibleNeighs.Add(new Force(point.Pos.X + 1, point.Pos.Y + 1));

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
						if (point.Pos.X < 0 || point.Pos.X >= _width)
						{
								return false;
						}
						if (point.Pos.Y < 0 || point.Pos.Y >= _height)
						{
								return false;
						}

						foreach (Force f in Forces)
						{
								if (f.IsGoal)
								{
										continue;
								}
								if (f.Pos.X == point.Pos.X && f.Pos.Y == point.Pos.Y)
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
