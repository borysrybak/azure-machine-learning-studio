using System.IO;

namespace AzureML.Studio.Core.Models
{
    internal class HttpResult
    {
        internal bool IsSuccess { get; set; }
        internal int StatusCode { get; set; }
        internal string ReasonPhrase { get; set; }
        internal string Payload { get; set; }
        internal Stream PayloadStream { get; set; }
    }
}
