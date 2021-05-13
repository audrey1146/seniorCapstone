﻿/****************************************************************************
 * File			ApiCaller.cs
 * Author		Audrey Lincoln
 * Date			2/20/2021
 * Purpose		Helper class to get the response of the Api
 ****************************************************************************/

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace seniorCapstone.Services
{
    public class ApiCaller
    {

        //**************************************************************************
        // Function:	Get
        //
        // Description:	Sends a Get request to a specific API
        //
        // Parameters:	url	    -	Name of the specific property
        //              authId  -   Authorization to the API
        //
        // Returns:		Response of the API
        //**************************************************************************
        public static async Task<ApiResponse> Get (string url, string authId = null)
        {
            using (var client = new HttpClient ())
            {
                if (!string.IsNullOrWhiteSpace (authId))
				{
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Authorization", authId);
                }
                    
                var request = await client.GetAsync (url);
                if (request.IsSuccessStatusCode)
                {
                    return new ApiResponse { Response = await request.Content.ReadAsStringAsync () };
                }
                else
				{
                    return new ApiResponse { ErrorMessage = request.ReasonPhrase };
                }
                    
            }
        }
    }

    public class ApiResponse
    {
        public bool Successful => ErrorMessage == null;
        public string ErrorMessage { get; set; }
        public string Response { get; set; }
    }
}
