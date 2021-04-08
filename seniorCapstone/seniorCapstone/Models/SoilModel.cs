using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace seniorCapstone.Models
{
	public class SoilModel
	{
		/// <summary>
		/// Tuple that represents table 3.10 and 3.9 (page 46/47) in the book
		/// 'Irrigation and Drainage Engineering, P.Waller and M. Yitayew, Springer'
		/// Item1 is 'a' constant and Item2 is the 'b' constant for each soil type
		/// </summary>
		public static Dictionary<string, Tuple<double, double>> InfiltrationConstants = new Dictionary<string, Tuple<double, double>> ()
		{
			{ "Clay", new Tuple<double, double>(.0620, .661) },
			{ "Silty Clay", new Tuple<double, double>(.0701, .683) },
			{ "Silty Clay Loam", new Tuple<double, double>(.0925, .720) },
			{ "Sandy Clay", new Tuple<double, double>(.0996, .729) },
			{ "Clay Loam", new Tuple<double, double>(.1064, .736) },
			{ "Sandy Clay Loam", new Tuple<double, double>(.1130, .742) },
			{ "Fine Sandy Loam", new Tuple<double, double>(.1196, .748) },
			{ "Loam", new Tuple<double, double>(.1321, .757) },
			{ "Fine Sand", new Tuple<double, double>(.1443, .766) },
			{ "Loamy Fine Sand", new Tuple<double, double>(.1560, .773) },
			{ "Loamy Sand", new Tuple<double, double>(.1674, .779) },
			{ "Course Sand", new Tuple<double, double>(.1768, .785) },
			{ "Sand", new Tuple<double, double>(.2283, .799) },

		};

		/// <summary>
		/// Collection of the soil names that are offered.
		/// Value corresponds to the key of the Dictionary
		/// </summary>
		public static ObservableCollection<string> SoilNames = new ObservableCollection<string> ()
		{
			"Clay",
			"Silty Clay",
			"Silty Clay Loam",
			"Sandy Clay",
			"Clay Loam",
			"Sandy Clay Loam",
			"Fine Sandy Loam",
			"Loam",
			"Fine Sand",
			"Loamy Fine Sand",
			"Loamy Sand",
			"Course Sand",
			"Sand"
		};
	}

	
}
