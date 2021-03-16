using System.Collections.ObjectModel;

namespace seniorCapstone.Helpers
{
	class CenterPivotSpecs
	{
		public static ObservableCollection<string> SoilTypes = new ObservableCollection<string> ()
		{
			"Sand", "Sandy Loam", "Loam", "Clay Loam", "Silty Clay", "Clay"
		};

		public static ObservableCollection<double> SoilIIR = new ObservableCollection<double> ()
		{
			50, 25, 13, 8, 2.5, 0.5
		};

		public static ObservableCollection<int> PivotTypes = new ObservableCollection<int> ()
		{
			400, 300, 150
		};
	}
}
