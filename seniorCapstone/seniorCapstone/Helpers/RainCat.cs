using System;
using System.Collections.Generic;
using System.Text;
using seniorCapstone.Helpers;
using seniorCapstone.Tables;

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
			DateTime endTime = DateTime.Now.AddHours (24);
			return (String.Format ("{0:s}", endTime));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldTable"></param>
		/// <returns></returns>
		public double WaterUsage (ref FieldTable fieldTable)
		{
			
			return (0);
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
