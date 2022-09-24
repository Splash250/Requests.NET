using System.Collections.Generic;

namespace Requests.NET
{
    public static class Requests
    {
        public static RequestResponse Post(string url, string data, Dictionary<string, string> headerParameters = null)
        {
            Utils.CheckHeaderParams(ref headerParameters);
            Session session = new Session();
            session.Post(url, data, headerParameters);
            return session.Response;
        }
        public static RequestResponse Post(string url, string data, Session session, Dictionary<string, string> headerParameters = null)
        {
            Utils.CheckHeaderParams(ref headerParameters);
            session.Post(url, data, headerParameters);
            return session.Response;
        }
        public static RequestResponse Get(string url, Dictionary<string, string> headerParameters = null)
        {
            Utils.CheckHeaderParams(ref headerParameters);
            Session session = new Session();
            RequestResponse response = session.Get(url, headerParameters);
            return response;
        }
        public static RequestResponse Get(string url,Session session, Dictionary<string, string> headerParameters = null)
        {
            Utils.CheckHeaderParams(ref headerParameters);
            RequestResponse response = session.Get(url, headerParameters);
            return response;
        }
    }
}
