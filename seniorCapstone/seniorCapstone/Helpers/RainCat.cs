﻿/****************************************************************************
 * File			RainCat.cs
 * Author		Audrey Lincoln
 * Date			3/20/2021
 * Purpose		Helper class that gets the watering information
 ****************************************************************************/

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

		//**************************************************************************
		// Function:	TotalRunTime
		//
		// Description:	Total Run time is currently set at 24 hours from start time
		//				in the format: "2008-03-09T16:05:07"
		//
		// Parameters:	fieldTable	-	FieldTable object to calculate end time of
		//
		// Returns:		The time that the pivot will shut off
		//**************************************************************************
		public static string TotalRunTime (ref FieldTable fieldTable)
		{
			DateTime runTime = DateTime.Now.AddHours (CenterPivotModel.RevolutionTime);
			return (String.Format ("{0:s}", runTime));
		}


		//**************************************************************************
		// Function:	WaterUsage
		//
		// Description:	Calculate the water usage of a specific pivot. Based on
		//				the Pacific University 2021 mathematics capstone 'RainCat'
		//
		// Parameters:	fieldTable	-	FieldTable object to calculate water usage of
		//
		// Returns:		Total water that the pivot uses.
		//**************************************************************************
		public static double WaterUsage (ref FieldTable fieldTable)
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

			// For each sprinkler in the pivot
			for (int i = CenterPivotModel.Spacing; i < fieldTable.PivotLength; i += CenterPivotModel.Spacing)
			{
				// Get sprinkler specific data
				t_a = (CenterPivotModel.RevolutionTime) * (CenterPivotModel.WettedDiameter / (2 * Math.PI * i));
				da_dt_max = (4 / Math.PI) * (CenterPivotModel.WettedSoilDepth / t_a);

				// Find first intersection by incrementing through small time increments starting at zero
				for (double t = .001; t < t_a && bInt1Found == false; t += .001)
				{
					da_dt = (da_dt_max) * Math.Sqrt (1 - (Math.Pow ((t - (t_a / 2)), 2) / Math.Pow (t_a / 2, 2)));
					di_dt = 600 * constA * constB * Math.Pow (t, constB - 1);

					if (di_dt - da_dt < 1 && di_dt - da_dt > -1)
					{
						intersect1 = t;
						bInt1Found = true;
					}
				}

				// If first intersection is found then we need to search for the second
				if (true == bInt1Found)
				{
					// Find second intersection by incrementing through small time increments starting at the end of the interval
					for (double t = t_a; t > intersect1 + .005 && bInt2Found == false; t -= .001)
					{
						da_dt = (da_dt_max) * Math.Sqrt (1 - (Math.Pow ((t - (t_a / 2)), 2) / Math.Pow (t_a / 2, 2)));
						di_dt = 600 * constA * constB * Math.Pow (t, constB - 1);

						if (di_dt - da_dt < 1 && di_dt - da_dt > -1)
						{
							intersect2 = t;
							bInt2Found = true;
						}
					}
				}

				// If both intersections are found then we have enough information to get the water usage stats
				if (true == bInt1Found && true == bInt2Found)
				{
					// Get distance traveled then integrate to get amount of water used from t_0 to t_1
					circumference = 2 * Math.PI * i;
					totalSlice = (circumference / CenterPivotModel.RevolutionTime) * (intersect2 - intersect1);
					integration = SimpsonRule.IntegrateComposite (x => (da_dt_max) *
						Math.Sqrt (1 - (Math.Pow ((x - (t_a / 2)), 2) / Math.Pow (t_a / 2, 2))), 0, intersect1, 4);
				}
				// If first intersection was not found, OR if second intersection was not found then assume pivot will run entire time
				else if (false == bInt1Found || false == bInt2Found)
				{
					circumference = 2 * Math.PI * i;
					totalSlice = (circumference / CenterPivotModel.RevolutionTime) * (t_a - 0);
					integration = SimpsonRule.IntegrateComposite (x => (da_dt_max) *
						Math.Sqrt (1 - (Math.Pow ((x - (t_a / 2)), 2) / Math.Pow (t_a / 2, 2))), 0, t_a, 4);
				}


				totalWater += integration * (circumference / totalSlice);


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
	}
}
