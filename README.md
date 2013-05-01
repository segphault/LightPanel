# LightPanel

LightPanel is a simple desktop application for controlling Philips Hue lighting systems. It is built with Xamarin.Mac and the C# programming language. It connects to a Hue base station on the local network and sends commands using the [Hue REST API](http://developers.meethue.com/).

### Some Notes:

* It comes with several custom user interface elements that were created with the assistance of [PaintCode](http://www.paintcodeapp.com/), including a custom toggle-switch control.
* It requires Mono 3.0 because async/await are used for the REST API calls.
* The settings dialog hasn't been implemented yet, so you have to paste a local network IP address and access hash for your Hue base station into the code before you compile.
* It uses the colorspace conversion library from the [Q42.HueApi](https://github.com/Q42/Q42.HueApi) project.
* I originally wanted this application to be a menubar agent, but there are bugs in NSPopover that make it work badly with NSStatusItem.

## Screenshot

![LightPanel](http://seg.phault.net/images/lightpanelscreen.png)
