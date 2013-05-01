using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LightPanel
{
	public class LightState
	{
		public string alert { get; set; }
		public int bri { get; set; }
		public string colormode { get; set; }
		public int ct { get; set; }
		public string effect { get; set; }
		public int hue { get; set; }
		public bool on { get; set; }
		public bool reachable { get; set; }
		public int sat { get; set; }
		public List<double> xy { get; set; }
	}
	
	public class Light
	{
		public string lightid { get; set; }
		public string modelid { get; set; }
		public string name { get; set; }
		public LightState state { get; set; }
		public string swversion { get; set; }
		public string type { get; set; }
	}
	
	public class HueClient
	{
		protected WebClient client;
		
		public string ClientID { get; set; }
		public string ClientIP { get; set; }
		
		public HueClient()
		{
			client = new WebClient ();
		}
		
		private string GenerateURL (string endpoint)
		{
			return String.Format ("http://{0}/api/{1}/{2}", ClientIP, ClientID, endpoint);
		}
		
		public async Task<Dictionary<string, Light>> FetchLightsAsync ()
		{
			var output = JObject.Parse (await SendRequestAsync (""));
			return JsonConvert.DeserializeObject<Dictionary<string, Light>> (output ["lights"].ToString());
		}

		public Task<string> SetLightStateAsync (string light, object obj)
		{
			return SendRequestAsync (String.Format ("lights/{0}/state", light), obj);
		}
		
		public Task<string> SendRequestAsync (string endpoint)
		{
			var url = GenerateURL (endpoint);
			return client.DownloadStringTaskAsync (url);
		}
		
		public Task<string> SendRequestAsync (string endpoint, object obj)
		{
			return SendRequestAsync (endpoint, JsonConvert.SerializeObject (obj));
		}
		
		public Task<string> SendRequestAsync (string endpoint, string value)
		{
			var url = GenerateURL (endpoint);
			return client.UploadStringTaskAsync (url, "PUT", value);
		}
	}
}
