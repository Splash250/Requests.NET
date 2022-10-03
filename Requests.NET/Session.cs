using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Requests.NET
{
    public class Session
    {
        public CookieContainer SessionCookies { get; set; }
        public RequestResponse Response { get; set; }
        public Session(CookieContainer SessionCookies)
        {
            this.SessionCookies = SessionCookies;
        }
        public Session()
        {
            SessionCookies = new CookieContainer();
        }
        public RequestResponse Post(string URL, string data, Dictionary<string, string> headerParameters = null)
        {
            Utils.CheckHeaderParams(ref headerParameters);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = EnvironmentStrings.PostMethod;
            request.CookieContainer = SessionCookies;
            byte[] byteArray = Encoding.UTF8.GetBytes(data);

            Utils.AddRequestParams(request, headerParameters);

            Response = TrySendRequestAndGetResponse(request, byteArray);
            return Response;
        }
        private RequestResponse TrySendRequestAndGetResponse(WebRequest request, byte[] dataBytes)
        {
            try
            {
                return SendRequestAndGetResponse(request, dataBytes);
            }
            catch (WebException ex)
            {
                RequestResponse webErrorResponse = new RequestResponse();
                if (TryLoadWebException(ex, out webErrorResponse))
                {
                    return webErrorResponse;
                }
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        private RequestResponse SendRequestAndGetResponse(WebRequest request, byte[] dataBytes)
        {
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(dataBytes, 0, dataBytes.Length);
            }

            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            SessionCookies.Add(resp.Cookies);
            return RequestResponse.LoadResponse(resp);
        }
        private bool TryLoadWebException(WebException ex, out RequestResponse webErrorResponse)
        {
            webErrorResponse = null;
            if (ex.Status == WebExceptionStatus.ProtocolError)
                return TryLoadProtocolError(ex, out webErrorResponse);
            else
                return false;
        }
        private bool TryLoadProtocolError(WebException ex, out RequestResponse webErrorResponse)
        {
            webErrorResponse = null;
            if (ex.Response is HttpWebResponse resp)
            {
                try
                {
                    SessionCookies.Add(resp.Cookies);
                    webErrorResponse = RequestResponse.LoadResponse(resp);
                    return true;
                }
                catch (Exception) { return false; }
            }
            else
            {
                return false;
            }
        }

        public RequestResponse Get(string URL,Dictionary<string, string> headerParameters = null)
        {
            try
            {
                Utils.CheckHeaderParams(ref headerParameters);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = EnvironmentStrings.GetMethod;
                request.CookieContainer = SessionCookies;

                return GetPageWithHeader(request, headerParameters);
            }
            catch (WebException ex)
            {
                RequestResponse webErrorResponse = new RequestResponse();
                if (TryLoadWebException(ex, out webErrorResponse))
                {
                    return webErrorResponse;
                }
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        private RequestResponse GetPageWithHeader(HttpWebRequest request, Dictionary<string, string> headerParameters = null)
        {
            Utils.AddRequestParams(request, headerParameters);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                SessionCookies.Add(response.Cookies);
                Response = RequestResponse.LoadResponse(response);
                return Response;
            }

        }
        public void ResetCookies()
        {
            SessionCookies = new CookieContainer();
        }
    }
}
