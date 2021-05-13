/****************************************************************************
 * File			PageNavViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/22/2020
 * Purpose		Handles the navigation functionality when a user attempts
 *				to change to another page.
 ****************************************************************************/

using seniorCapstone.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	public abstract class PageNavViewModel : INotifyPropertyChanged
	{
		// Properties
		public ICommand ChangePageCommand { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;

		//**************************************************************************
		// Function:	ChangePage
		//
		// Description:	When the buttons are pressed, the name of the button will be passed, 
		//				and will correspond with a view.
		//
		// Parameters:	PageName	-	name of the page to change to
		//
		// Returns:		None
		//**************************************************************************
		public async void ChangePage (string PageName)
		{
			switch (PageName)
			{
				case "MainPage":
					await Application.Current.MainPage.Navigation.PopToRootAsync();
					break;
				case "AccountPage":
					await Application.Current.MainPage.Navigation.PushAsync (new AccountPage ());
					break;
				case "FieldListPage":
					await Application.Current.MainPage.Navigation.PushAsync (new FieldListPage ());
					break;
				case "AddFieldPage":
					await Application.Current.MainPage.Navigation.PushModalAsync (new AddFieldPage ());
					break;
				case "RunPivotPage":
					await Application.Current.MainPage.Navigation.PushModalAsync (new RunPivotPage ());
					break;
				case "Logout":
					App.IsUserLoggedIn = false;
					App.UserID = string.Empty;
					Application.Current.MainPage = new NavigationPage (new LoginPage ());
					break;
			}
		}


		//**************************************************************************
		// Function:	OnPropertyChanged
		//
		// Description:	Invoked when a property changes to notify the view and viewmodel
		//
		// Parameters:	propertyName	-	Name of the property changed
		//
		// Returns:		None
		//**************************************************************************
		protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
		}
	}
}