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


		public string TotalRunTime (ref FieldTable fieldTable)
		{
			/*
				YYYY MM DD hh:mm:ss
				DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7, 123);
				String.Format("{0:s}", dt);  // "2008-03-09T16:05:07"  SortableDateTime
			 */

			DateTime tempTime = new DateTime (2021, 4, 15, 10, 52, 123);

			return (String.Format ("{0:s}", tempTime));
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
