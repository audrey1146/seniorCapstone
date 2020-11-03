using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using seniorCapstone.Tables;
using SQLite;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class RunPivotViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// Private Varibales
		private int fieldIndex = -1;

		// Public Properties
		public ICommand RunPivotCommand { get; set; }
		public ICommand CancelCommand { get; set; }
		public IList<string> FieldOptions { get; set; }
		public int FieldIndex
		{
			get => this.fieldIndex;
			set
			{
				if (this.fieldIndex != value)
				{
					this.fieldIndex = value;
					OnPropertyChanged (nameof (this.FieldIndex));
				}
			}
		}


		/// <summary>
		/// Constructor for the AddField VM. Sets the list of 
		/// viable pivot lengths
		/// </summary>
		public RunPivotViewModel ()
		{
			FieldOptions = new ObservableCollection<string> ();
			this.GetPivots ();

			this.CancelCommand = new Command (this.CancelButton_Clicked);
			this.RunPivotCommand = new Command (this.RunPivotButton_Clicked);
		}

		/// <summary>
		/// Gets a list of all of the non-running fields
		/// </summary>
		public async void GetPivots ()
		{
			// Reading from Database
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				// Query for the fields
				List <FieldTable> FieldList = dbConnection.Query<FieldTable>
					("SELECT FieldName FROM FieldTable WHERE UID=? AND PivotRunning=0", App.UserID);

				// If query fails then pop this page off the stack
				if (null == FieldList)
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
					Debug.WriteLine ("Finding User Fields Failed");
				}

				for (int i = 0; i < FieldList.Count; i++)
				{
					FieldOptions.Add (FieldList[i].FieldName);
				}
			}
		}


		/// <summary>
		/// Verfies that the user input data for all entries
		/// </summary>
		/// <returns>
		/// True if all of the entries have some text in them, otherwise false 
		/// if any are empty or null
		/// </returns>
		public bool AreEntiresFilledOut ()
		{
			return (-1 != this.FieldIndex);
		}

		/// <summary>
		/// Command that will pop the current page off of the stack
		/// </summary>
		public async void CancelButton_Clicked ()
		{
			await Application.Current.MainPage.Navigation.PopModalAsync ();
		}

		
		public async void RunPivotButton_Clicked ()
		{
			if (true == this.AreEntiresFilledOut ())
			{ 
				using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
				{
					dbConnection.CreateTable<FieldTable> ();

					List<FieldTable> runPivot = dbConnection.Query<FieldTable>
					("UPDATE FieldTable SET PivotRunning=1 WHERE UID=? AND FieldName=? AND PivotRunning=0", App.UserID, this.FieldOptions[FieldIndex]);

					// If query fails then pop this page off the stack
					if (null == runPivot)
					{
						Debug.WriteLine ("Running Pivot Failed");
					}
				}
				await Application.Current.MainPage.Navigation.PopModalAsync ();
			}
			else
			{
				await App.Current.MainPage.DisplayAlert ("Run Pivot Alert", "Choose a Field", "Ok");
			}
		}


		protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
		}

	}
}
