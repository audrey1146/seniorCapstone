using seniorCapstone.Tables;
using seniorCapstone.Views;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class RunningFieldViewModel : PageNavViewModel
	{
		// Public Properties
		public ICommand StopPivotCommand { get; set; }

		public RunningFieldViewModel ()
		{
			base.ChangePageCommand = new Command<string> (base.ChangePage);
			this.StopPivotCommand = new Command (this.StopPivotButton_Clicked);
		}

		public async void StopPivotButton_Clicked ()
		{
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				List<FieldTable> runPivot = dbConnection.Query<FieldTable>
				("UPDATE FieldTable SET PivotRunning=0 WHERE UID=? AND FID=? AND PivotRunning=1", App.UserID, App.FieldID);

				// If query fails then pop this page off the stack
				if (null == runPivot)
				{
					Debug.WriteLine ("Stopping Pivot Failed From the Field Page");
					await Application.Current.MainPage.Navigation.PopAsync ();
				}
				else
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
					//await Application.Current.MainPage.Navigation.PushAsync (new StoppedFieldPage ());
				}
			}
		}
	}
}
