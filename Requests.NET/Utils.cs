using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace Requests.NET
{
    static class Utils
    {

        public static readonly Dictionary<string, string> defaultHeaderParams = new Dictionary<string, string>()
        {
            { "Accept", "application/x-www-form-urlencoded" },
            { "User-Agent", "C-Sharp"},
        };
        public static WebRequest AddRequestParams(WebRequest request, Dictionary<string, string> headerParameters)
        {
            HttpWebRequest req = (HttpWebRequest)request;
            if (headerParameters != null && headerParameters.Count > 0)
            {
                for (int i = 0; i < headerParameters.Count; i++)
                {
                    LoadParam(req, headerParameters.ElementAt(i));
                }
            }
            return req;
        }
        private static HttpWebRequest LoadParam(HttpWebRequest req,KeyValuePair<string, string> reqParam)
        {
            //loading all the header parameters that we provided with the arguments
            if (reqParam.Key == EnvironmentStrings.AcceptHeaderString)
            {
                req.Accept = reqParam.Value;
            }
            else if (reqParam.Key == EnvironmentStrings.ConnectionHeaderString)
            {
                req.Connection = reqParam.Value;
            }
            else if (reqParam.Key == EnvironmentStrings.ContentLengthHeaderString)
            {
                TrySetContentLength(reqParam.Value, req);
            }
            else if (reqParam.Key == EnvironmentStrings.ContentTypeHeaderString)
            {
                req.ContentType = reqParam.Value;
            }
            else if (reqParam.Key == EnvironmentStrings.DateHeaderString)
            {
                TrySetDate(reqParam.Value, req);
            }
            else if (reqParam.Key == EnvironmentStrings.ExpectHeaderString)
            {
                req.Expect = reqParam.Value;
            }
            else if (reqParam.Key == EnvironmentStrings.HostHeaderString)
            {
                req.Host = reqParam.Value;
            }
            else if (reqParam.Key == EnvironmentStrings.RefererHeaderString)
            {
                req.Referer = reqParam.Value;
            }
            else if (reqParam.Key == EnvironmentStrings.UserAgentHeaderString)
            {
                req.UserAgent = reqParam.Value;
            }
            else if (reqParam.Key == EnvironmentStrings.ProtocolVersionHeaderString)
            {
                TrySetProtocolVersion(reqParam.Value, req);
            }
            else if (reqParam.Key == EnvironmentStrings.TransferEncodingHeaderString)
            {
                req.TransferEncoding = reqParam.Value;
            }
            else if (reqParam.Key == EnvironmentStrings.TimeoutHeaderString)
            {
                TrySetTimeout(reqParam.Value, req);
            }
            else
            {
                //if we come to an unknown header we just add it to the request's custom header list
                req.Headers[reqParam.Key] = reqParam.Value;
            }
            return req;
        }
        private static void TrySetContentLength(string contentLength, HttpWebRequest req)
        {
            if (int.TryParse(contentLength, out int length))
            {
                req.ContentLength = length;
            }
        }
        private static void TrySetDate(string dateValue, HttpWebRequest req)
        {
            if (DateTime.TryParseExact(dateValue,
                EnvironmentStrings.DefaultDateTimeFormat,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out DateTime parsedDate)
                )
            {
                req.Date = parsedDate;
            }
        }
        private static void TrySetProtocolVersion(string protocolVersion, HttpWebRequest req)
        {
            if (Version.TryParse(protocolVersion, out Version parsedVersion))
            {
                req.ProtocolVersion = parsedVersion;
            }
        }
        private static void TrySetTimeout(string timeout, HttpWebRequest req)
        {
            if (int.TryParse(timeout, out int parsedTimeout))
            {
                req.Timeout = parsedTimeout;
            }
        }
        public static Dictionary<string, string> CheckHeaderParams(ref Dictionary<string, string> headerParameters)
        {

            if (headerParameters == null || headerParameters.Count <= 0)
                return Utils.defaultHeaderParams;
            return headerParameters;
        }
    }
}
