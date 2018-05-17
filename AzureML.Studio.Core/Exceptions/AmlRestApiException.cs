using AzureML.Studio.Core.Models;
using System;

namespace AzureML.Studio.Core.Exceptions
{
    internal class AmlRestApiException : Exception
    {
        internal AmlRestApiException(HttpResult httpResult) : base("Error: [" + httpResult.StatusCode + " (" + httpResult.ReasonPhrase + ")]: " + httpResult.Payload) { }
    }
}
