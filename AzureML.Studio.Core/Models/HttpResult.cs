using System.IO;

namespace AzureML.Studio.Core.Models
{
    public class HttpResult
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public string Payload { get; set; }
        public Stream PayloadStream { get; set; }
    }
}
