using seniorCapstone.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace seniorCapstone.Interfaces
{
	public interface IFieldPage : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// Public Properties
		public ICommand AddFieldCommand { get; set; }
		public ICommand CancelCommand { get; set; }
		public IList<int> PivotOptions { get; set; }
		public string FieldName { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
	}
}
