/****************************************************************************
 * File		FieldListViewModel.cs
 * Author	Audrey Lincoln
 * Date		11/15/2020
 * Purpose	VM for the FieldList page
 ****************************************************************************/

using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class FieldListViewModel : PageNavViewModel
	{
		public FieldListViewModel()
		{
			base.ChangePageCommand = new Command<string> (base.ChangePage);
		}
	}
}
