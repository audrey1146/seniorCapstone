using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Rg.Plugins.Popup.Services;
using seniorCapstone.Tables;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using seniorCapstone.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace seniorCapstone.Views
{
	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class EditStoppedFieldPopupPage : PopupPage
	{
		// event callback
		public event EventHandler<object> CallbackEvent;

		// Private Variables
		readonly IFieldDataService fieldDataService;
		ObservableCollection<FieldTable> fieldEntries;
		FieldTable singleField;

		// Public Properties
		public ObservableCollection<FieldTable> FieldEntries
		{
			get => this.fieldEntries;
			set
			{
				this.fieldEntries = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public EditStoppedFieldPopupPage ()
		{
			InitializeComponent ();

			this.fieldDataService = new FieldApiDataService (new Uri ("https://evenstreaminfunctionapp.azurewebsites.net"));
			this.FieldEntries = new ObservableCollection<FieldTable> ();
			this.setPlaceholder ();

			soiltype.ItemsSource = (System.Collections.IList)Helpers.CenterPivotSpecs.SoilTypes;
			pivotlength.ItemsSource = (System.Collections.IList)Helpers.CenterPivotSpecs.PivotTypes;
		}


		/// <summary>
		/// Get the current user data to display as place holders
		/// </summary>
		private async void setPlaceholder ()
		{
			int count = 0;

			await this.LoadEntries ();

			foreach (FieldTable field in this.FieldEntries)
			{
				if (field.FID == App.FieldID && field.UID == App.UserID)
				{
					this.singleField = field;
					fieldname.Placeholder = field.FieldName;
					count++;
				}
			}

			// If query fails then pop this page off the stack
			if (count != 1)
			{
				Debug.WriteLine ("Finding Current Field Failed");
				await PopupNavigation.Instance.PopAsync (true);


				//await Application.Current.MainPage.Navigation.PopAsync ();
				//Debug.WriteLine ("Finding Current Field Failed");
				//await PopupNavigation.Instance.PopAsync (true);
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
			FieldTable updatedField = new FieldTable (ref singleField);
			bool bPopOff = true;

			if (false == this.areEntriesFilled ())
			{
				await App.Current.MainPage.DisplayAlert ("Update Field Alert", "No New Entries", "OK");
				bPopOff = false;
			}
			else
			{
				// Update based on which entries got filled out
				if (false == string.IsNullOrEmpty (fieldname.Text))
				{
					// Verify Unique field name
					if (true == doesFieldNameExist ())
					{
						await App.Current.MainPage.DisplayAlert ("Update Field Alert", "Field Name Already Exists", "OK");
						bPopOff = false;
					}
					else
					{
						updatedField.FieldName = fieldname.Text;
					}
				}
				if (-1 != pivotlength.SelectedIndex)
				{
					updatedField.PivotLength = (int)pivotlength.ItemsSource[pivotlength.SelectedIndex];
				}
				if (-1 != soiltype.SelectedIndex)
				{
					updatedField.SoilType = (string)soiltype.ItemsSource[soiltype.SelectedIndex];
				}
			}
			

			// If valid new field then edit existing
			if (true == bPopOff)
			{
				await this.fieldDataService.EditEntryAsync (updatedField);

				await PopupNavigation.Instance.PopAsync (true);
				CallbackEvent?.Invoke (this, EventArgs.Empty);
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



		/// <summary>
		/// Calls the API and loads the returned data into a member variable
		/// </summary>
		private async Task LoadEntries ()
		{
			try
			{
				var entries = await fieldDataService.GetEntriesAsync ();
				this.FieldEntries = new ObservableCollection<FieldTable> (entries);
			}
			catch (Exception ex)
			{
				Debug.WriteLine ("Loading Fields Failed");
				Debug.WriteLine (ex.Message);
				await PopupNavigation.Instance.PopAsync (true);
			}
		}


		/// <summary>
		/// Query the database to check whether the field name already exists for the 
		/// current user
		/// </summary>
		private bool doesFieldNameExist ()
		{
			foreach (FieldTable field in this.FieldEntries)
			{
				if (field.FieldName == fieldname.Text && field.UID == App.UserID)
				{
					return (true);
				}
			}

			return (false);
		}


		/// <summary>
		/// Check that at least one field is filled
		/// </summary>
		private bool areEntriesFilled ()
		{
			return (false == string.IsNullOrEmpty (fieldname.Text)
				|| -1 != pivotlength.SelectedIndex
				|| -1 != soiltype.SelectedIndex);
		}
	}
}