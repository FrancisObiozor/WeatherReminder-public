using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeatherReminder.Models.ApiModel.ApiKeys;

namespace WeatherReminder.Models.ApiModel.IpAddressGeocodeApi
{
    public class IpAddressGeocode : IIpAddressGeocode
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IKeyVault _keyVault;
        private readonly IHttpContextAccessor _accessor;

        public IpAddressGeocode(IHttpClientFactory clientFactory,
                                 IKeyVault keyVault,
                                 IHttpContextAccessor accessor)
        {
            _clientFactory = clientFactory;
            _keyVault = keyVault;
            _accessor = accessor;
        }

        private string GetIpAddress()
        {
            var ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ipAddress == "::1")
            {
                try
                {
                    WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                    using (WebResponse response = request.GetResponse())
                    using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                    {
                        ipAddress = stream.ReadToEnd();
                    }

                    int first = ipAddress.IndexOf("Address: ") + 9;
                    int last = ipAddress.LastIndexOf("</body>");
                    ipAddress = ipAddress.Substring(first, last - first);
                }
                catch (Exception)
                {
                    ipAddress = null;
                }
            }

            return ipAddress;
        }

        public async Task<IpAddressGeocodeModel> GetLocationFromIp()
        {

            var ip = GetIpAddress();
            var geocode = new IpAddressGeocodeModel();

            if (ip != null)
            {
                var ipAddressGeocodeKey = _keyVault.ApiKeys.IpAddressGeocodeKey;
                var request = new HttpRequestMessage(HttpMethod.Get, $"http://api.ipstack.com/{ip}?access_key={ipAddressGeocodeKey}");
                var client = _clientFactory.CreateClient();

                try
                {
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        geocode.Location = await response.Content.ReadFromJsonAsync<IpAddressGeocodeResults>();
                    }
                    else
                    {
                        geocode.Error = $"There was an error getting the address: {response.ReasonPhrase}";
                    }
                    return geocode;
                }
                catch (Exception)
                {
                    geocode.Error = "There was an error getting the address";
                    return geocode;
                }
            }
            else
            {
                geocode.Location = null;
                geocode.Error = "There was an error getting your ip address";
            }
            return geocode;
        }


    }

}
