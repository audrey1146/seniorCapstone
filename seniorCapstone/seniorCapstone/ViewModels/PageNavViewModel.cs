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

		/// <summary>
		/// When the buttons are pressed, the name of the button will be passed, 
		/// and will correspond with a view.
		/// </summary>
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
			}
		}

		protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke (this,
			new PropertyChangedEventArgs (propertyName));
		}
	}
}