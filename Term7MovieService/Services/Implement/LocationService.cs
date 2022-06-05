using Microsoft.Extensions.Options;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Options;
using Term7MovieService.Services.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Term7MovieCore.Extensions;

namespace Term7MovieService.Services.Implement
{
    public class LocationService : ILocationService
    {
        private const string RESULTS = "results";
        private const string GEOMETRY = "geometry";
        private const string LOCATION = "location";

        private readonly GoongOption goongOption;
        public LocationService(IOptions<GoongOption> options)
        {
            goongOption = options.Value;
        }

        public async Task<Location> GetLocationByAddressAsync(string address)
        {
            Location location = new Location();

            using(HttpClient client = new HttpClient())
            {
                string uri = string.Format(goongOption.GeocodeForwardUri, address, goongOption.APIKey);
                var response = await client.GetAsync(uri); 
                if (response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();

                    JObject jObj = JObject.Parse(message);

                    if (jObj[RESULTS] != null)
                    {
                        var element = jObj[RESULTS][0];

                        if (element == null) return null;

                        location = element[GEOMETRY][LOCATION].ToString().ToObject<Location>();
                    }
                }
            }

            return location;
        }
    }
}
