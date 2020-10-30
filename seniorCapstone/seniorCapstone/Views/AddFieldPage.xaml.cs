using seniorCapstone.Tables;
using SQLite;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddFieldPage : ContentPage
	{
		public AddFieldPage()
		{
			InitializeComponent();
		}

		/// <summary>
		/// When the user presses the button to register their data 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void AddFieldButton_Clicked(object sender, System.EventArgs e)
		{
			int returnValue = 0;

			if (true == areEntiresFilledOut())
			{
				FieldTable newField = new FieldTable()
				{
					UID = App.UserID,
					FieldName = fieldname.Text,
					PivotLength = pivotlength.SelectedIndex,
					Longitude = longitude.Text,
					Latitude = latitude.Text,
					PivotAngle = 0,
					PivotRunning = 0
				};

				// Insert into DB (using closes the DB for me)
				using (SQLiteConnection dbConnection = new SQLiteConnection(App.DatabasePath))
				{
					dbConnection.CreateTable<FieldTable>();

					returnValue = dbConnection.Insert(newField);

					if (1 == returnValue)
					{
						await Application.Current.MainPage.Navigation.PopModalAsync();
					}
					else
					{
						await DisplayAlert ("Alert", "Field Not Inserted Correctly", "OK");
					}
				}
			}
			else
			{
				await DisplayAlert("Alert", "Please Fill Out Entire Form", "OK");
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void CancelButton_Clicked(object sender, System.EventArgs e)
		{
			await Application.Current.MainPage.Navigation.PopModalAsync();
		}

		/// <summary>
		/// Verfies that the user input data for all entries
		/// </summary>
		/// <returns>
		/// True if all of the entries have some text in them, otherwise false 
		/// if any are empty or null
		/// </returns>
		private bool areEntiresFilledOut()
		{
			return (false	== string.IsNullOrEmpty(fieldname.Text) &&
					false	== string.IsNullOrEmpty(longitude.Text) &&
					false	== string.IsNullOrEmpty(latitude.Text) &&
					-1		== pivotlength.SelectedIndex);
		}
	}
}