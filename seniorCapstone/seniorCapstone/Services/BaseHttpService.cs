/****************************************************************************
 * File			BaseHttpService.cs
 * Author		Audrey Lincoln + 'Mastering Xamarin.Forms' Book
 * Date			4/3/2021
 * Purpose		HTTP Service to send and recieve data. This provides 
 *				a building block for any domain-specific data services; 
 *				for example, a service responsible for log entries in the API. 
 *				Any class that will inherit from this class will be 
 *				able to send HTTP request messages using standard HTTP methods
 *				(such as GET, POST, PATCH, and DELETE) and get HTTP response
 *				messages back without having to deal with HttpClientdirectly.
 * Note         Defined in Chapter 6 page 113 in the book
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace seniorCapstone.Services
{
	public abstract class BaseHttpService
	{

        //**************************************************************************
        // Function:	SendRequestAsync
        //
        // Description:	Sends requests to an API, and gets the response
        //
        // Parameters:	url	        -	URL to send request to
        //              httpMethod  -   Specific HTTP request
        //              headers     -   Headers to add to the request
        //              requestData -   Data in the request
        //
        // Returns:		The response
        //**************************************************************************
        protected async Task<T> SendRequestAsync<T> (Uri url, HttpMethod httpMethod = null, 
            IDictionary<string, string> headers = null, object requestData = null)
        {
            var result = default (T);

            // Default to GET
            var method = httpMethod ?? HttpMethod.Get;

            // Serialize request data
            var data = requestData == null
                ? null
                : JsonConvert.SerializeObject (requestData);

            using (var request = new HttpRequestMessage (method, url))
            {
                // Add request data to request
                if (data != null)
                {
                    request.Content = new StringContent (data, Encoding.UTF8, "application/json");
                }

                // Add headers to request
                if (headers != null)
                {
                    foreach (var h in headers)
                    {
                        request.Headers.Add (h.Key, h.Value);
                    }
                }

                // Get response
                using (var client = new HttpClient ())
                using (var response = await client.SendAsync (request, HttpCompletionOption.ResponseContentRead))
                {
                    var content = response.Content == null
                        ? null
                        : await response.Content.ReadAsStringAsync ();

                    if (response.IsSuccessStatusCode)
                    {
                        result = JsonConvert.DeserializeObject<T> (content);
                    }
                }
            }

            return result;
        }
    }
}
