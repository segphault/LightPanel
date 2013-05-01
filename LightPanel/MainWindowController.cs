
using System;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace LightPanel
{
	public partial class MainWindowController : MonoMac.AppKit.NSWindowController
	{
		#region Constructors
		
		// Called when created from unmanaged code
		public MainWindowController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Call to load from the XIB/NIB file
		public MainWindowController () : base ("MainWindow")
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
		}
		
		#endregion

		HueDelegate Hue = new HueDelegate();
		
		//strongly typed window accessor
		public new MainWindow Window {
			get {
				return (MainWindow)base.Window;
			}
		}
	
		public override void WindowDidLoad ()
		{
			base.WindowDidLoad ();

			Hue.Controller = controllerLights;
			Hue.Refresh();

			buttonRefresh.Activated += (sender, e) => Hue.Refresh();
		}
	}
}

