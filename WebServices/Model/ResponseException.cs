namespace WebServices.Model
{
    using Newtonsoft.Json;

    public class ResponseException
    {
        [JsonProperty("Version")]
        public string Version { get; set; }

        [JsonProperty("StatusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        //[JsonProperty("ResponseException")]
        //public ExceptionResponse Exception { get; set; }
    }
}
