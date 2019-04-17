namespace WebServices.Services.Request
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using WebServices.Exceptions;
    using Xamarin.Essentials;

    public class RequestService : IRequestService
    {
        readonly JsonSerializerSettings serializerSettings;

        public RequestService()
        {
            serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };

            serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string token = "")
        {
            var httpClient = CreateHttpClient(token);
            var response = await httpClient.GetAsync(uri);

            await HandleResponse(response);

            var serialized = await response.Content.ReadAsStringAsync();
            var result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, serializerSettings));

            return result;
        }

        public Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "") => PostAsync<TResult, TResult>(uri, data, token);

        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "")
        {
            var httpClient = CreateHttpClient(token);
            var serialized = await Task.Run(() => JsonConvert.SerializeObject(data, serializerSettings));
            var response = await httpClient.PostAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            var responseData = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, serializerSettings));
        }

        public Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "") => PutAsync<TResult, TResult>(uri, data, token);

        public async Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data, string token = "")
        {
            var httpClient = CreateHttpClient(token);
            var serialized = await Task.Run(() => JsonConvert.SerializeObject(data, serializerSettings));
            var response = await httpClient.PutAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            var responseData = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, serializerSettings));
        }

        HttpClient CreateHttpClient(string token = "")
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                throw new ConnectivityException();

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                if (IsEmail(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Email", token);
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return httpClient;
        }

        bool IsEmail(string email) => new EmailAddressAttribute().IsValid(email);

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden 
                    || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception(content);
                }

                throw new HttpRequestException(content);
            }

            /*
            try
            {   
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                    {
                        var result = await Task.Run(() => JsonConvert.DeserializeObject<Response<TResult>>(serialized, serializerSettings));
                        return result.Result;
                    }
                    else if (response.StatusCode == HttpStatusCode.Accepted) {}
                    }
                    var error = await Task.Run(() => JsonConvert.DeserializeObject<ResponseException>(serialized, serializerSettings));

                    throw new ApiException($"{error.Exception.ExceptionMessage}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error en la consulta", ex);
            }*/
        }
    }
}