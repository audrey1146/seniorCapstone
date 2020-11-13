using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone.Views
{
	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class SyncPopupPage : PopupPage
	{
		public SyncPopupPage ()
		{
			InitializeComponent ();
		}

		public EventHandler<bool> Action;  //you can change "string" to any parameter you want to pass back.

		public async void Sync_ButtonClickedEvent (object sender, EventArgs e)
		{
			bool parameter = true;

			Action?.Invoke (this, parameter); // don't forget to invoke the method before close the popup. (only invoke when you want to pass the value back).

			await PopupNavigation.Instance.PopAsync ();

		}

		public async void Cancel_ButtonClickedEvent (object sender, EventArgs e)
		{
			bool parameter = false;

			Action?.Invoke (this, parameter); // don't forget to invoke the method before close the popup. (only invoke when you want to pass the value back).

			await PopupNavigation.Instance.PopAsync ();

		}
	}
}