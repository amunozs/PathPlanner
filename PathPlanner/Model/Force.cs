using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathPlanner.Model
{
		public class Force
		{
				public double Strength;
				public bool IsGoal = false;

				public int PosX;
				public int PosY;
				public bool Frozen = false;
				public Force(int posX, int posY, double strength = 0, bool isGoal = false)
				{
						PosX = posX;
						PosY = posY;
						IsGoal = isGoal;
						Strength = strength;
		}

		}
}
