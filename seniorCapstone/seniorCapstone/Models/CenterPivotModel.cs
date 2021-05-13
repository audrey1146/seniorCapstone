/****************************************************************************
 * File		CenterPivotModel.cs
 * Author	Audrey Lincoln
 * Date		3/20/2021
 * Purpose	Assumptions of the RainCat center pivot that were decided 
 *			in the Pacific University 2021 mathematics capstone 'RainCat'
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace seniorCapstone.Models
{
	class CenterPivotModel
	{
		public static ObservableCollection<int> PivotTypes = new ObservableCollection<int> ()
		{
			400, 300, 150
		};

		public static int WettedDiameter
		{
			get => 6;
		}

		public static int Spacing
		{
			get => 5;
		}

		public static int RevolutionTime
		{
			get => 24;
		}

		public static double WettedSoilDepth
		{
			get => 25.4;
		}
	}
}
