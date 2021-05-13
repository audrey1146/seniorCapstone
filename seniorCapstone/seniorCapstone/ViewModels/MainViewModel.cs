/****************************************************************************
 * File			MainViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/22/2020
 * Purpose		Home Page of the mobile application
 ****************************************************************************/

using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	/// <summary>
	/// Viewmodel class that implements the properties and commands for the
	/// main page of the app.
	/// </summary>
	public class MainViewModel : PageNavViewModel
	{
		//**************************************************************************
		// Constructor:	MainViewModel
		//
		// Description:	Sets the change page command
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public MainViewModel ()
		{
			base.ChangePageCommand = new Command<string>(base.ChangePage);
		}
	}
}
