/****************************************************************************
 * File			IUserDataService.cs
 * Author		Audrey Lincoln + 'Mastering Xamarin.Forms' Book
 * Date			4/3/2021
 * Purpose		Interface for the data service that can be injected into 
 *				the ViewModels.
 * Note         Defined in Chapter 6 page 115 in the book
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using seniorCapstone.Models;

namespace seniorCapstone.Services
{
	public interface IUserDataService
	{
		Task<IList<UserTable>> GetEntriesAsync ();
		Task<UserTable> AddEntryAsync (UserTable entry);
		Task<UserTable> DeleteEntryAsync (UserTable entry);
		Task<UserTable> EditEntryAsync (UserTable entry);
	}
}
