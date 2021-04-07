using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.Integration;
using seniorCapstone.Helpers;
using seniorCapstone.Models;

namespace seniorCapstone.Helpers
{
	public class RainCat
	{
		public RainCat ()
		{

		}

		/// <summary>
		/// Total Run time is currently set at 24 hours from start time
		/// in the format: "2008-03-09T16:05:07" 
		/// </summary>
		/// <param name="fieldTable"></param>
		/// <returns> The time that the pivot will shut off </returns>
		public string TotalRunTime (ref FieldTable fieldTable)
		{
			DateTime endTime = DateTime.Now.AddHours (CenterPivotModel.RevolutionTime);
			return (String.Format ("{0:s}", endTime));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldTable"></param>
		/// <returns></returns>
		public double WaterUsage (ref FieldTable fieldTable)
		{
			double constA = SoilModel.InfiltrationConstants[fieldTable.SoilType].Item1;
			double constB = SoilModel.InfiltrationConstants[fieldTable.SoilType].Item2;

			double totalWater = 0;
			double t_a = 0;
			double da_dt_max = 0;
			double da_dt = 0;
			double di_dt = 0;

			double intersect1 = 0;
			double intersect2 = 0;

			bool bInt1Found = false;
			bool bInt2Found = false;

			double circumference = 0;
			double totalSlice = 0;
			double integration = 0;


			for (int i = CenterPivotModel.Spacing; i < fieldTable.PivotLength; i *= CenterPivotModel.Spacing)
			{
				t_a = (CenterPivotModel.RevolutionTime) * (CenterPivotModel.WettedDiameter / (2 * Math.PI * i));

				da_dt_max = (4 / Math.PI) * (CenterPivotModel.WettedSoilDepth / t_a);

				// Find intersection 1
				for (double t = .005; t < t_a && bInt1Found == false; t += .002)
				{
					da_dt = (da_dt_max) * Math.Sqrt (1 - (Math.Pow ((t - (t_a / 2)), 2) / Math.Pow (t_a / 2, 2)));
					di_dt = 600 * constA * constB * Math.Pow (t, constB - 1);

					if (di_dt - da_dt < 1 && di_dt - da_dt > -1)
					{
						intersect1 = t;
						bInt1Found = true;
					}
				}

				if (true == bInt1Found)
				{
					// Find intersection 2
					for (double t = t_a; t > intersect1 && bInt2Found == false; t -= .002)
					{
						da_dt = (da_dt_max) * Math.Sqrt (1 - (Math.Pow ((t - (t_a / 2)), 2) / Math.Pow (t_a / 2, 2)));
						di_dt = 600 * constA * constB * Math.Pow (t, constB - 1);

						if (di_dt - da_dt < 1 && di_dt - da_dt > -1)
						{
							intersect2 = t;
							bInt2Found = true;
						}
					}


					if (true == bInt2Found)
					{
						// Get distance traveled then integrate to get amount of water used from t_0 to t_1
						circumference = 2 * Math.PI * i;
						totalSlice = (circumference / CenterPivotModel.RevolutionTime) * (intersect2 - intersect1);

						integration = SimpsonRule.IntegrateComposite (x => (da_dt_max) * Math.Sqrt (1 - (Math.Pow ((x - (t_a / 2)), 2) / Math.Pow (t_a / 2, 2))), 0, intersect1, 4);
						totalWater += integration * (circumference / totalSlice);
					}
					// If no second intersection then ????
					else
					{

					}


				}
				// If no intersection then da_dt over entire field
				else
				{

				}


				// Reset Values
				intersect1 = 0;
				intersect2 = 0;
				bInt1Found = false;
				bInt2Found = false;

				circumference = 0;
				totalSlice = 0;
			}



			return (totalWater);
		}


		/// <summary>
		/// 
		/// </summary>
		public double InstantaneousInfiltrationRate (string soil)
		{
			double IR = 0;

			

			return (IR);
		}
	}
}
