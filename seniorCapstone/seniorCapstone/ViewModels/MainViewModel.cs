using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	/// <summary>
	/// Viewmodel class that implements the properties and commands for the
	/// main page of the app.
	/// </summary>
	public class MainViewModel : INotifyPropertyChanged
	{
		// Variables
		public event PropertyChangedEventHandler PropertyChanged;


		// Properties
		public ICommand ChangePageCommand { get; }



		/// <summary>
		/// Constructor method
		/// </summary>
		public MainViewModel ()
		{
			ChangePageCommand = new Command (ChangePage);
		}

		/// <summary>
		/// 
		/// </summary>
		public void OnPropertyChanged ()
		{

		}


		/// <summary>
		/// When the buttons are pressed, the name of the button will be passed, 
		/// and will correspond with a view.
		/// </summary>
		public void ChangePage ()
		{

		}
	}
}
