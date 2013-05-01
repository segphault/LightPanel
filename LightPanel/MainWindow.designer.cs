// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;

namespace LightPanel
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSCollectionView viewLights { get; set; }

		[Outlet]
		MonoMac.AppKit.NSArrayController controllerLights { get; set; }

		[Outlet]
		MonoMac.AppKit.NSToolbarItem buttonRefresh { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (viewLights != null) {
				viewLights.Dispose ();
				viewLights = null;
			}

			if (controllerLights != null) {
				controllerLights.Dispose ();
				controllerLights = null;
			}

			if (buttonRefresh != null) {
				buttonRefresh.Dispose ();
				buttonRefresh = null;
			}
		}
	}

	[Register ("MainWindow")]
	partial class MainWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
