namespace WebServices.Model
{
    using Newtonsoft.Json;

    [JsonObject]
    public class Response<T>
    {
        [JsonProperty("Version")]
        public string Version { get; set; }

        [JsonProperty("StatusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Result")]
        public T Result { get; set; }
    }
}
