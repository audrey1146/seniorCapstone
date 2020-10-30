using seniorCapstone.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	public abstract class PageNavViewModel
	{
		// Properties
		public ICommand ChangePageCommand { get; set; }

		/// <summary>
		/// When the buttons are pressed, the name of the button will be passed, 
		/// and will correspond with a view.
		/// </summary>
		public async void ChangePage(string PageName)
		{
			switch (PageName)
			{
				case "MainPage":
					await Application.Current.MainPage.Navigation.PopToRootAsync();
					break;
				case "AccountPage":
					await Application.Current.MainPage.Navigation.PushAsync(new AccountPage());
					break;
				case "FieldListPage":
					await Application.Current.MainPage.Navigation.PushAsync(new FieldListPage());
					break;
				case "AddFieldPage":
					await Application.Current.MainPage.Navigation.PushModalAsync(new AddFieldPage());
					break;

			}
		}
	}
}
/*case "RunPivotPage":
	await Application.Current.MainPage.Navigation.PushModalAsync(new RunPivotPage());
	break;*/