using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;

namespace IF.Lastfm.Core.Api.Helpers
{
    public interface ILastResponse
    {
        bool Success { get; }

        LastResponseStatus Status { get; }

        string ErrorMessage { get; }

        HttpStatusCode? HttpStatusCode { get; }
    }

    public class LastResponse : ILastResponse
    {
        public virtual bool Success
        {
            get { return Status == LastResponseStatus.Successful; }
        }

        public LastResponseStatus Status { get; internal set; }

        [Obsolete("This property has been renamed to Status and will be removed soon.")]
        public LastResponseStatus Error { get { return Status; } }

        public string ErrorMessage { get; internal set; }

        public HttpStatusCode? HttpStatusCode { get; internal set; }

        public Exception Exception { get; internal set; }

        public static LastResponse CreateSuccessResponse()
        {
            var r = new LastResponse
            {
                Status = LastResponseStatus.Successful
            };

            return r;
        }

        public static T CreateErrorResponse<T>(LastResponseStatus status) where T : LastResponse, new()
        {
            var r = new T
            {
                Status = status
            };

            return r;
        }

        internal static T CreateErrorResponse<T>(string json, HttpResponseMessage httpResponse)
            where T : LastResponse, new()
        {
            LastResponseStatus status;
            string message;
            LastFm.IsResponseValid(json, out status, out message);
            return new T
            {
                Status = status,
                ErrorMessage = message,
                HttpStatusCode = httpResponse.StatusCode
            };
        }

        public static async Task<LastResponse> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                return LastResponse.CreateSuccessResponse();
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse>(json, response);
            }
        }
    }
}