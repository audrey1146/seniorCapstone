/****************************************************************************
 * File			MainViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/22/2020
 * Purpose		Home Page of the mobile application
 ****************************************************************************/

using seniorCapstone.Interfaces;
using System.Windows.Input;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	/// <summary>
	/// Viewmodel class that implements the properties and commands for the
	/// main page of the app.
	/// </summary>
	public class MainViewModel: IPageNav
	{
		// Properties
		public ICommand ChangePageCommand { get; }


		/// <summary>
		/// Constructor method
		/// </summary>
		public MainViewModel ()
		{
			ChangePageCommand = new Command<string> (ChangePage);
		}


		/// <summary>
		/// When the buttons are pressed, the name of the button will be passed, 
		/// and will correspond with a view.
		/// </summary>
		public void ChangePage (string PageName)
		{

		}
	}
}
