/****************************************************************************
 * File			AccountViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/22/2020
 * Purpose		Account Page of the mobile application
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	public class AccountViewModel : PageNavViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		public AccountViewModel()
		{
			base.ChangePageCommand = new Command<string>(base.ChangePage);
		}


	}
}
