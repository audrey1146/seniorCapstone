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
