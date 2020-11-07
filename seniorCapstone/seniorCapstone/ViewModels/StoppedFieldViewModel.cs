using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class StoppedFieldViewModel : PageNavViewModel
	{

		public StoppedFieldViewModel ()
		{
			base.ChangePageCommand = new Command<string> (base.ChangePage);
		}
	}
}
