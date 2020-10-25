/****************************************************************************
 * File			MainViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/22/2020
 * Purpose		Home Page of the mobile application
 ****************************************************************************/

using seniorCapstone.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	/// <summary>
	/// Viewmodel class that implements the properties and commands for the
	/// main page of the app.
	/// </summary>
	public class MainViewModel : PageNavViewModel
	{
		/// <summary>
		/// Constructor method
		/// </summary>
		public MainViewModel ()
		{
			base.ChangePageCommand = new Command<string>(base.ChangePage);
		}
	}
}
