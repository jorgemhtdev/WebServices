namespace WebServices.Utils
{
    using System;
    using Microsoft.AppCenter.Crashes;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Xamarin.Essentials;

    public class PreferencesHelpers
    {
        public static T Get<T>(string key, T @default) where T : class
        {
            var serialized = Preferences.Get(key, string.Empty);
            var result = @default;

            try
            {
                var serializeSettings = GetSerializerSettings();
                result = JsonConvert.DeserializeObject<T>(serialized);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return result;
        }


        public static void Set<T>(string key, T obj) where T : class
        {
            try
            {
                var serializeSettings = GetSerializerSettings();
                var serialized = JsonConvert.SerializeObject(obj, serializeSettings);

                Preferences.Set(key, serialized);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        static JsonSerializerSettings GetSerializerSettings() => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}
