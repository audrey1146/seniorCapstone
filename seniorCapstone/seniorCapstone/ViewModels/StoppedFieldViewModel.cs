using seniorCapstone.Tables;
using seniorCapstone.Views;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class StoppedFieldViewModel : PageNavViewModel
	{
		// Public Properties
		public ICommand RunPivotCommand { get; set; }

		public StoppedFieldViewModel ()
		{
			base.ChangePageCommand = new Command<string> (base.ChangePage);
			this.RunPivotCommand = new Command (this.RunPivotButton_Clicked);
		}

		public async void RunPivotButton_Clicked ()
		{
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				List<FieldTable> runPivot = dbConnection.Query<FieldTable>
				("UPDATE FieldTable SET PivotRunning=1 WHERE UID=? AND FID=? AND PivotRunning=0", App.UserID, App.FieldID);

				// If query fails then pop this page off the stack
				if (null == runPivot)
				{
					Debug.WriteLine ("Running Pivot Failed From the Field Page");
					await Application.Current.MainPage.Navigation.PopAsync ();
				}
				else
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
					//await Application.Current.MainPage.Navigation.PushAsync (new RunningFieldPage ());
				}
			}
		}
	}
}
