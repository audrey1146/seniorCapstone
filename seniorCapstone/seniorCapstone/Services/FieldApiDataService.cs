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
	public class FieldApiDataService : BaseHttpService, IFieldDataService
	{
		readonly Uri baseUri;
		readonly IDictionary<string, string> headers;

		public FieldApiDataService (Uri paramBaseUri)
		{
			baseUri = paramBaseUri;
			headers = new Dictionary<string, string> ();

			// TODO:  Header with auth-based token
		}

		public async Task<IList<FieldTable>> GetEntriesAsync ()
		{
			var url = new Uri (baseUri, "/api/fieldEntry");
			var response = await SendRequestAsync<FieldTable[]> (url, HttpMethod.Get, headers);

			return response;
		}

		public async Task<FieldTable> AddEntryAsync (FieldTable entry)
		{
			var url = new Uri (baseUri, "/api/fieldEntry");
			var response = await SendRequestAsync<FieldTable> (url, HttpMethod.Post, headers, entry);

			return response;
		}

		public async Task<FieldTable> DeleteEntryAsync (FieldTable entry)
		{
			var url = new Uri (baseUri, "/api/fieldEntry");
			var response = await SendRequestAsync<FieldTable> (url, HttpMethod.Delete, headers, entry);

			return response;
		}

		public async Task<FieldTable> EditEntryAsync (FieldTable entry)
		{
			var url = new Uri (baseUri, "/api/fieldEntry");
			var response = await SendRequestAsync<FieldTable> (url, HttpMethod.Put, headers, entry);

			return response;
		}
	}
}
