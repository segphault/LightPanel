using System;
using System.Drawing;
using System.Collections.Generic;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace LightPanel
{
	[Register("ToggleSwitch")]
	public class ToggleSwitch : NSButtonCell
	{
		public ToggleSwitch (IntPtr handle) : base(handle) {}
		
		[Export("initWithCoder:")]
		public ToggleSwitch (NSCoder coder) : base(coder) {}

		public override void DrawImage (NSImage image, System.Drawing.RectangleF frame, NSView controlView)
		{
			var context = NSGraphicsContext.CurrentContext.GraphicsPort;

			var darkHighlight = NSColor.FromCalibratedRgba (0.931f, 0.931f, 0.931f, 1.0f);
			var innerBoxShadowColor = NSColor.FromCalibratedRgba (0.896f, 0.896f, 0.896f, 1.0f);
			var switchInnerFillEnabledColor1 = NSColor.FromCalibratedRgba (0.215f, 0.447f, 0.668f, 1.0f);
			var switchInnerFillEnabledColor2 = NSColor.FromCalibratedHsba (switchInnerFillEnabledColor1.HueComponent,
			                                                               switchInnerFillEnabledColor1.SaturationComponent, 1,
			                                                               switchInnerFillEnabledColor1.AlphaComponent);

			var switchInnerFillGradient = new NSGradient (NSColor.DarkGray, NSColor.Gray);
			var buttonFillGradientColors = new NSColor[] { innerBoxShadowColor, NSColor.FromCalibratedRgba(0.948f, 0.948f, 0.948f, 1.0f), NSColor.White };
			var buttonFillGradientPositions = new float[] { 0.0f, 0.25f, 0.51f };
			var switchButtonFillGradient = new NSGradient (buttonFillGradientColors, buttonFillGradientPositions);
			var switchInnerFillEnabledGradient = new NSGradient (new NSColor[] { switchInnerFillEnabledColor1, switchInnerFillEnabledColor2 }, new float[] { 0.0f, 1.0f});

			var switchBoxInnerShadow = new NSShadow ();
			switchBoxInnerShadow.ShadowColor = NSColor.Black;
			switchBoxInnerShadow.ShadowOffset = new SizeF (0.1f, 0.1f);
			switchBoxInnerShadow.ShadowBlurRadius = 5;

			var switchButtonShadow = new NSShadow ();
			switchButtonShadow.ShadowColor = NSColor.DarkGray;
			switchButtonShadow.ShadowOffset = new SizeF (0.1f, 0.1f);
			switchButtonShadow.ShadowBlurRadius = 2;

			var switchFrame =  new RectangleF(frame.X, frame.Y, 69, 28);

			var switchBoxPath = NSBezierPath.FromRoundedRect (new RectangleF (switchFrame.X + 12.5f, switchFrame.Y + switchFrame.Height - 23.5f, 49, 20), 9, 9);

			var gradientToUse = IntValue > 0 ? switchInnerFillEnabledGradient : switchInnerFillGradient;
			gradientToUse.DrawInBezierPath (switchBoxPath, -90);

			var switchBoxBorderRect = switchBoxPath.Bounds;
			switchBoxBorderRect.Inflate (switchBoxInnerShadow.ShadowBlurRadius, switchBoxInnerShadow.ShadowBlurRadius);
			switchBoxBorderRect.Offset (-switchBoxInnerShadow.ShadowOffset.Width, -switchBoxInnerShadow.ShadowOffset.Height);
			switchBoxBorderRect = RectangleF.Union (switchBoxBorderRect, switchBoxPath.Bounds);
			switchBoxBorderRect.Inflate (1, 1);

			var switchBoxNegativePath = NSBezierPath.FromRoundedRect (switchBoxBorderRect, 0, 0);
			switchBoxNegativePath.AppendPath (switchBoxPath);
			switchBoxNegativePath.WindingRule = NSWindingRule.EvenOdd;

			context.SaveState ();

			var switchBoxInnerShadowOffset = new SizeF(0.1f, -1.1f);
			var xOffset = switchBoxInnerShadowOffset.Width + (float)Math.Round(switchBoxBorderRect.Width);
			var yOffset = switchBoxInnerShadowOffset.Height;
			NSShadow switchBoxInnerShadowWithOffset = (NSShadow)switchBoxInnerShadow.Copy ();
			switchBoxInnerShadowWithOffset.ShadowOffset = new SizeF (xOffset + (xOffset >= 0 ? 0.1f : -0.1f), yOffset + (yOffset >= 0 ? 0.1f : -0.1f));
			switchBoxInnerShadowWithOffset.Set ();

			NSColor.Gray.SetFill ();
			switchBoxPath.AddClip ();

			var transform = new NSAffineTransform ();
			transform.Translate (-(float)Math.Round (switchBoxBorderRect.Width), 0);
			transform.TransformBezierPath (switchBoxNegativePath).Fill ();

			context.RestoreState ();

			NSColor.WindowFrame.SetStroke ();
			switchBoxPath.LineWidth = 0.5f;
			switchBoxPath.Stroke ();

			// Switch label drawing

			var labelPosition = IntValue > 0 ? 19 : 32;
			var switchLabelRect = new RectangleF (switchFrame.X + labelPosition, switchFrame.Y + switchFrame.Height - 20, 22, 15);
			NSMutableParagraphStyle switchLabelStyle = (NSMutableParagraphStyle)NSMutableParagraphStyle.DefaultParagraphStyle.MutableCopy ();
			switchLabelStyle.Alignment = NSTextAlignment.Center;
			var switchLabelFontAttributes = NSDictionary.FromObjectsAndKeys (
				new NSObject[] { NSFont.FromFontName("Helvetica-Bold", NSFont.SmallSystemFontSize), darkHighlight, switchLabelStyle },
				new NSObject[] { NSAttributedString.FontAttributeName, NSAttributedString.ForegroundColorAttributeName, NSAttributedString.ParagraphStyleAttributeName });

			var labelText = IntValue > 0 ? "On" : "Off";
			new NSString (labelText).DrawString (switchLabelRect, switchLabelFontAttributes);
			
		
			// Switch Button
			var position = this.IntValue > 0 ? 43.0f : 10.0f;
			var switchButtonPath = NSBezierPath.FromOvalInRect (new RectangleF (switchFrame.X + position, switchFrame.Y + switchFrame.Height - 24, 21, 21));

			context.SaveState ();
			switchButtonShadow.Set ();
			context.BeginTransparencyLayer (null);
			switchButtonFillGradient.DrawInBezierPath (switchButtonPath, 135.0f);
			context.EndTransparencyLayer ();
			context.RestoreState ();

			context.SaveState ();
			switchButtonShadow.Set ();
			NSColor.DarkGray.SetStroke ();
			switchButtonPath.LineWidth = 0.5f;
			switchButtonPath.Stroke ();
			context.RestoreState ();
		}
	}
}

