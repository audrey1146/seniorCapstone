
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
