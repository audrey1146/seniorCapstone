/****************************************************************************
 * File			IFieldDataService.cs
 * Author		Audrey Lincoln + 'Mastering Xamarin.Forms' Book
 * Date			4/3/2021
 * Purpose		Interface for the data service that can be injected into 
 *				the ViewModels.
 * Note         Defined in Chapter 6 page 115 in the book
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using seniorCapstone.Tables;

namespace seniorCapstone.Services
{
	public interface IFieldDataService
	{
		Task<IList<FieldTable>> GetEntriesAsync ();
		Task<FieldTable> AddEntryAsync (FieldTable entry);
		Task<FieldTable> DeleteEntryAsync (FieldTable entry);
		Task<FieldTable> EditEntryAsync (FieldTable entry);
	}
}
