using AzureML.Studio.Core.Models;
using System;

namespace AzureML.Studio.Common.Exceptions
{
    public class AmlRestApiException : Exception
    {
        public AmlRestApiException(HttpResult httpResult) : base("Error: [" + httpResult.StatusCode + " (" + httpResult.ReasonPhrase + ")]: " + httpResult.Payload) { }
    }
}
