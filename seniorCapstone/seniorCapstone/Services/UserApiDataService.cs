/****************************************************************************
 * File			UserApiDataService.cs
 * Author		Audrey Lincoln + 'Mastering Xamarin.Forms' Book
 * Date			4/3/2021
 * Purpose		Implementation of IUserDataService, with access  
 *				to the BaseHttpService
 * Note         Defined in Chapter 6 page 116 in the book
 ****************************************************************************/

using seniorCapstone.Tables;
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

		public UserApiDataService (Uri paramBaseUri)
		{
			baseUri = paramBaseUri;
			headers = new Dictionary<string, string> ();

			// TODO:  Header with auth-based token
		}

		public async Task<IList<UserTable>> GetEntriesAsync ()
		{
			var url = new Uri (baseUri, "/api/userEntry");
			var response = await SendRequestAsync<UserTable[]> (url, HttpMethod.Get, headers);

			return response;
		}

		public async Task<UserTable> AddEntryAsync (UserTable entry)
		{
			var url = new Uri (baseUri, "/api/userEntry");
			var response = await SendRequestAsync<UserTable> (url, HttpMethod.Post, headers, entry);

			return response;
		}

		public async Task<UserTable> DeleteEntryAsync (UserTable entry)
		{
			var url = new Uri (baseUri, "/api/userEntry");
			var response = await SendRequestAsync<UserTable> (url, HttpMethod.Delete, headers, entry);

			return response;
		}

		public async Task<UserTable> EditEntryAsync (UserTable entry)
		{
			var url = new Uri (baseUri, "/api/userEntry");
			var response = await SendRequestAsync<UserTable> (url, HttpMethod.Put, headers, entry);

			return response;
		}
	}
}
