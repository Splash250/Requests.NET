using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Requests.NET
{
    public class RequestResponse
    {

        public string Content { get; set; }
        public string Header { get; set; }
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }

        public CookieContainer ResponseCookies { get; set; }
        public RequestResponse()
        {
            Content = String.Empty;
            Header = String.Empty;
            StatusCode = 0;
            StatusDescription = String.Empty;
            ResponseCookies = new CookieContainer();
        }
        public RequestResponse(string content, string header, int statusCode, string statusDescription, CookieContainer responseCookies)
        {
            Content = content;
            Header = header;
            StatusCode = statusCode;
            StatusDescription = statusDescription;
            ResponseCookies = responseCookies;
        }
        public static RequestResponse LoadResponse(HttpWebResponse response)
        {
            CookieContainer cookies = new CookieContainer();
            cookies.Add(response.Cookies);
            RequestResponse req_response = new RequestResponse
            {
                Content = LoadResponseBody(response.GetResponseStream()),
                Header = LoadHeaders(response.Headers),
                StatusCode = (int)response.StatusCode,
                StatusDescription = response.StatusDescription,
                ResponseCookies = cookies
                
            };
            return req_response;

        }
        private static string LoadHeaders(WebHeaderCollection headers)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < headers.Count; i++)
            {
                sb.Append(headers.Keys[i] + ": " + headers[i] + EnvironmentStrings.NewLine);
            }
            return sb.ToString();
        }
        private static string LoadResponseBody( Stream responseStream)
        {
            using (responseStream)
            {
                using (StreamReader sr = new StreamReader(responseStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        public Session ToSession()
        {
            return new Session(ResponseCookies);
        }
    }
}
