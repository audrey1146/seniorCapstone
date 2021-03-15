using System;
using System.Collections.Generic;
using System.Text;
using seniorCapstone.Helpers;

namespace seniorCapstone.Helpers
{
	public class RainCat
	{
		private SoilType mcSoilType;

		public RainCat ()
		{

		}

		/// <summary>
		/// 
		/// </summary>
		public float InstantaneousInfiltrationRate (string soil)
		{
			float IR = 0;

			switch (soil)
			{
				case (SoilType.SAND):
					break;
				case (SoilType.SANDY_LOAM):
					break;
				case (SoilType.CLAY):
					break;
				case (SoilType.CLAY_LOAM):
					break;
				case (SoilType.LOAM):
					break;
			}

			return (IR);
		}
	}
}
