using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using MonoMac.Foundation;
using MonoMac.AppKit;

namespace LightPanel
{

	public partial class LightModel : NSObject, INotifyPropertyChanged
	{
		private int brightness;
		private bool enabled;
		private NSColor color;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		[Export("id")]
		public string Id { get; set; }
		
		[Export("name")]
		public string Name { get; set; }
		
		[Export("brightness")]
		public int Brightness {
			get { return brightness; }
			set { brightness = value; OnPropertyChanged("Brightness"); }
		}
		
		[Export("enabled")]
		public bool Enabled {
			get { return enabled; }
			set { enabled = value; OnPropertyChanged ("Enabled"); }
		}

		[Export("color")]
		public NSColor Color {
			get { return color; }
			set { color = value; OnPropertyChanged ("Color"); }
		}
		
		protected void OnPropertyChanged(string name)
		{
			var handler = PropertyChanged;
			
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(name));
		}
	}

	public partial class HueDelegate : NSObject
	{
		private bool Refreshing = false;
		public LightModel Master;

		public HueClient Client { get; set; }
		public NSArrayController Controller { get; set; }
		
		protected Dictionary<string, NSObject> Storage = new Dictionary<string, NSObject> ();

		public HueDelegate()
		{
			Client = new HueClient {
				ClientIP = "192.168.1.XX",
				ClientID = "XXXXXXXXXXXXXXXXXXXXXXXXXX",
			};
		}

		protected NSColor GetLightColor (Light light)
		{
			var color = ColorService.RGBFromXY (light.state.xy);
			return NSColor.FromDeviceRgba (color [0] / 255.0f,
			                               color [1] / 255.0f,
			                               color [2] / 255.0f, 1.0f);
		}

		protected List<double> NSColorToXY (NSColor source)
		{
			var color = ColorService.XyFromColor ((int)(source.RedComponent * 255),
			                                      (int)(source.GreenComponent * 255),
			                                      (int)(source.BlueComponent * 255));

			return new List<double> { color.x, color.y };
		}

		protected void CreateLight(string id, Light light)
		{
			var newLight = new LightModel {
				Id = id,
				Name = light.name,
				Brightness = light.state.bri,
				Enabled = light.state.on,
				Color = GetLightColor(light),
			};
			
			newLight.PropertyChanged += OnLightChanged;
			
			Controller.AddObject (newLight);
			Storage[id] = newLight;
		}

		public void OnLightChanged (object sender, PropertyChangedEventArgs e)
		{
			if (Refreshing)
				return;

			var light = (LightModel)sender;

			Console.WriteLine ("Property {0} of {1} changed", e.PropertyName, light.Name);

			if (e.PropertyName == "Brightness")
				Client.SetLightStateAsync(light.Id, new { bri = light.Brightness });

			if (e.PropertyName == "Color")
				Client.SetLightStateAsync(light.Id, new { xy = NSColorToXY(light.Color) });

			if (e.PropertyName == "Enabled")
				Client.SetLightStateAsync (light.Id, new { on = light.Enabled });
		}
		
		public async Task Refresh ()
		{
			var lights = await Client.FetchLightsAsync ();
			
			Refreshing = true;
			
			foreach (var entry in lights) {
				if (Storage.ContainsKey(entry.Key)) {
					Storage[entry.Key].SetValueForKey((NSNumber)entry.Value.state.bri, (NSString)"brightness");
					Storage[entry.Key].SetValueForKey(GetLightColor(entry.Value), (NSString)"color");
					Storage[entry.Key].SetValueForKey(NSNumber.FromBoolean(entry.Value.state.on), (NSString)"enabled");
				}
				else CreateLight(entry.Key, entry.Value);
			}
			
			Refreshing = false;
		}
	}
}

