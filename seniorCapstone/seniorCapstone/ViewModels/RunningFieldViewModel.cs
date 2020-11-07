using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class RunningFieldViewModel : PageNavViewModel
	{

		public RunningFieldViewModel ()
		{
			base.ChangePageCommand = new Command<string> (base.ChangePage);
		}
	}
}
