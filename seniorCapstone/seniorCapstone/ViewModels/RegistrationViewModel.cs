using seniorCapstone.Tables;
using seniorCapstone.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	public class RegistrationViewModel
	{
		// Properties
		public ICommand RegisterAccountCommand { get; set; }
		public ICommand CancelRegistrationCommand { get; set; }

		public RegistrationViewModel ()
		{
			RegisterAccountCommand = new Command(RegisterButton_Clicked);
			CancelRegistrationCommand = new Command(CancelButton_Clicked);
		}


		public void RegisterButton_Clicked()
		{
			UserTable userTable = new UserTable()
			{
				
			};
		}

		public async void CancelButton_Clicked()
		{
			await Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync();
		}
	}
}
