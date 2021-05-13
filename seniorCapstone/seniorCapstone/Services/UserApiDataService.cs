/****************************************************************************
 * File			UserApiDataService.cs
 * Author		Audrey Lincoln + 'Mastering Xamarin.Forms' Book
 * Date			4/3/2021
 * Purpose		Implementation of IUserDataService, with access  
 *				to the BaseHttpService
 * Note         Defined in Chapter 6 page 116 in the book
 ****************************************************************************/

using seniorCapstone.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace seniorCapstone.Services
{
	public class UserApiDataService : BaseHttpService, IUserDataService
	{
		readonly Uri baseUri;
		readonly IDictionary<string, string> headers;

		//**************************************************************************
		// Constructor:	UserApiDataService
		//
		// Description:	Set up for the data servicer
		//
		// Parameters:	paramBaseUri	-	Base URL for the API
		//
		// Returns:		None
		//**************************************************************************
		public UserApiDataService (Uri paramBaseUri)
		{
			baseUri = paramBaseUri;
			headers = new Dictionary<string, string> ();

			// TODO:  Header with auth-based token
		}

		//**************************************************************************
		// Function:	GetEntriesAsync
		//
		// Description:	Get all of the entries
		//
		// Parameters:	None
		//
		// Returns:		List of the user entries
		//**************************************************************************
		public async Task<IList<UserTable>> GetEntriesAsync ()
		{
			var url = new Uri (baseUri, "/api/userEntry");
			var response = await SendRequestAsync<UserTable[]> (url, HttpMethod.Get, headers);

			return response;
		}

		//**************************************************************************
		// Function:	AddEntryAsync
		//
		// Description:	Add a user entry to the database
		//
		// Parameters:	entry	-	Entry to be added
		//
		// Returns:		Response of the API
		//**************************************************************************
		public async Task<UserTable> AddEntryAsync (UserTable entry)
		{
			var url = new Uri (baseUri, "/api/userEntry");
			var response = await SendRequestAsync<UserTable> (url, HttpMethod.Post, headers, entry);

			return response;
		}

		//**************************************************************************
		// Function:	DeleteEntryAsync
		//
		// Description:	Delete a user entry from the database
		//
		// Parameters:	entry	-	Entry to be deleted
		//
		// Returns:		Response of the API
		//**************************************************************************
		public async Task<UserTable> DeleteEntryAsync (UserTable entry)
		{
			var url = new Uri (baseUri, "/api/userEntry");
			var response = await SendRequestAsync<UserTable> (url, HttpMethod.Delete, headers, entry);

			return response;
		}

		//**************************************************************************
		// Function:	EditEntryAsync
		//
		// Description:	Edit a user entry from the database
		//
		// Parameters:	entry	-	Entry to be edited
		//
		// Returns:		Response of the API
		//**************************************************************************
		public async Task<UserTable> EditEntryAsync (UserTable entry)
		{
			var url = new Uri (baseUri, "/api/userEntry");
			var response = await SendRequestAsync<UserTable> (url, HttpMethod.Put, headers, entry);

			return response;
		}
	}
}
