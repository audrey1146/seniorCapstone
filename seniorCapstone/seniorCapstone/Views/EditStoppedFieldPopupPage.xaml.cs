﻿using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Rg.Plugins.Popup.Services;
using seniorCapstone.Tables;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone.Views
{
	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class EditStoppedFieldPopupPage : PopupPage
	{
		// event callback
		public event EventHandler<object> CallbackEvent;

		public EditStoppedFieldPopupPage ()
		{
			InitializeComponent ();
			this.setPlaceholder ();

			soiltype.ItemsSource = (System.Collections.IList)Helpers.CenterPivotSpecs.SoilTypes;
			pivotlength.ItemsSource = (System.Collections.IList)Helpers.CenterPivotSpecs.PivotTypes;
		}


		/// <summary>
		/// Get the current user data to display as place holders
		/// </summary>
		private async void setPlaceholder ()
		{
			// Reading from Database
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				// Query for the current user
				List<FieldTable> currentField = dbConnection.Query<FieldTable>
					("SELECT * FROM FieldTable WHERE UID=? AND FID=?", App.UserID, App.FieldID);

				// If query fails then pop this page off the stack
				if (null == currentField || currentField.Count != 1)
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
					Debug.WriteLine ("Finding Current Field Failed");
					await PopupNavigation.Instance.PopAsync (true);
				}
				else
				{
					fieldname.Placeholder = currentField[0].FieldName;
				}
			}
		}


		/// <summary>
		/// When the submit button is clicked this command will check update the 
		/// table entry according to the values they specified
		/// </summary>
		/// /// <param name="sender"></param>
		/// <param name="args"></param>
		public async void SubmitButton_Clicked (object sender, EventArgs args)
		{
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();
				bool bPopOff = true;

				// Update based on which entries got filled out
				if (false == string.IsNullOrEmpty (fieldname.Text))
				{
					// Before update check that the unique values don't already exist
					List<FieldTable> uniqueCheck = dbConnection.Query<FieldTable>
						("SELECT * FROM FieldTable WHERE FieldName=? AND UID=?",
						fieldname.Text, App.UserID);

					if (null != uniqueCheck && uniqueCheck.Count == 0)
					{
						List<FieldTable> updateFieldName = dbConnection.Query<FieldTable>
							("UPDATE FieldTable SET FieldName=? WHERE UID=? AND FID=?",
							fieldname.Text, App.UserID, App.FieldID);
					}
					else
					{
						await App.Current.MainPage.DisplayAlert ("Update Field Alert", "Field Name Already Exists", "OK");
						bPopOff = false;
					}
				}
				if (-1 != pivotlength.SelectedIndex)
				{
					List<FieldTable> updatePivotLength = dbConnection.Query<FieldTable>
						("UPDATE FieldTable SET PivotLength=? WHERE UID=? AND FID=?",
						pivotlength.ItemsSource[pivotlength.SelectedIndex], App.UserID, App.FieldID);
				}
				if (-1 != soiltype.SelectedIndex)
				{
					List<FieldTable> updateSoilType = dbConnection.Query<FieldTable>
						("UPDATE FieldTable SET SoilType=? WHERE UID=? AND FID=?",
						soiltype.ItemsSource[soiltype.SelectedIndex], App.UserID, App.FieldID);
				}

				if (true == bPopOff)
				{
					await PopupNavigation.Instance.PopAsync (true);
					CallbackEvent?.Invoke (this, EventArgs.Empty);
				}
			}
		}


		/// <summary>
		/// Pop off the popup page
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public async void CancelButton_Clicked (object sender, EventArgs args)
		{
			await PopupNavigation.Instance.PopAsync (true);
		}

	}
}