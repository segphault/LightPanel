using System;
using System.Drawing;
using System.Collections.Generic;
using MonoMac.Foundation;
using MonoMac.AppKit;


namespace LightPanel
{
	[Register("BoxFrame")]
	public class BoxFrame : NSView
	{
		public BoxFrame (IntPtr handle) : base(handle) {}
		
		[Export("initWithCoder:")]
		public BoxFrame (NSCoder coder) : base(coder) {}

		public override void DrawRect (RectangleF dirtyRect)
		{
			var context = NSGraphicsContext.CurrentContext.GraphicsPort;

			var innerBoxShadowColor = NSColor.FromCalibratedRgba (0.896f, 0.896f, 0.896f, 1.0f);
			var darkHighlight = NSColor.FromCalibratedRgba (0.931f, 0.931f, 0.931f, 1.0f);
			var boxBackgroundColor = NSColor.FromCalibratedRgba (1.0f, 1.0f, 1.0f, 0.5f);

			var headingGradient = new NSGradient (
				new NSColor[] { NSColor.White, darkHighlight },
				new float[] { 0.50f, 1.0f });

			var shadow = new NSShadow ();
			shadow.ShadowColor = innerBoxShadowColor;
			shadow.ShadowOffset = new SizeF (0.1f, 0.1f);
			shadow.ShadowBlurRadius = 8.0f;

			var headerShadow = new NSShadow ();
			headerShadow.ShadowColor = innerBoxShadowColor;
			headerShadow.ShadowOffset = new SizeF(1.1f, -1.1f);
			headerShadow.ShadowBlurRadius = 5.0f;

			var drawFrame = Bounds;

			// Main Box Drawing
			var mainBoxRect = new RectangleF(drawFrame.X + 4.5f, (float)(drawFrame.Y + Math.Floor((drawFrame.Height - 36.0f) * 0.02976f) + 0.5f),
			                             	 drawFrame.Width - 9.0f, (float)(drawFrame.Height - 36.5f - Math.Floor((drawFrame.Height - 36) * 0.02976f)));
			
			var mainBoxCornerRadius = 4.0f;
			var mainBoxInnerRect = new RectangleF(mainBoxRect.X, mainBoxRect.Y, mainBoxRect.Width, mainBoxRect.Height);
			mainBoxInnerRect.Offset (mainBoxCornerRadius, mainBoxCornerRadius);


			var mainBoxPath = new NSBezierPath ();
			mainBoxPath.AppendPathWithArc (new PointF (mainBoxInnerRect.X, mainBoxInnerRect.Y), mainBoxCornerRadius, 180.0f, 270.0f);
			mainBoxPath.AppendPathWithArc (new PointF (mainBoxInnerRect.Width, mainBoxInnerRect.Y), mainBoxCornerRadius, 270.0f, 360.0f);
			mainBoxPath.LineTo (new PointF (mainBoxRect.Right, mainBoxRect.Bottom));
			mainBoxPath.LineTo (new PointF (mainBoxRect.X, mainBoxRect.Bottom));
			mainBoxPath.ClosePath ();

			boxBackgroundColor.SetFill ();
			mainBoxPath.Fill ();

			// Main Box Inner Shadow
			var mainBoxBorderRect = mainBoxPath.Bounds;
			mainBoxBorderRect.Inflate(shadow.ShadowBlurRadius, shadow.ShadowBlurRadius);
			mainBoxBorderRect.Offset(-shadow.ShadowOffset.Width, -shadow.ShadowOffset.Height);
			mainBoxBorderRect = RectangleF.Union(mainBoxBorderRect, mainBoxPath.Bounds);
			mainBoxBorderRect.Inflate(1, 1);


			var mainBoxNegativePath = NSBezierPath.FromRect (mainBoxBorderRect);
			mainBoxNegativePath.AppendPath (mainBoxPath);
			mainBoxNegativePath.WindingRule = NSWindingRule.EvenOdd;

			context.SaveState ();
			NSShadow shadowWithOffset = (NSShadow)shadow.Copy ();
			var xOffset = shadowWithOffset.ShadowOffset.Width + Math.Round (mainBoxBorderRect.Size.Width);
			var yOffset = shadowWithOffset.ShadowOffset.Height;
			shadowWithOffset.ShadowOffset = new SizeF ((float)(xOffset + (xOffset >= 0 ? 0.1f : -0.1f)), (float)(yOffset + (yOffset >= 0 ? 0.1f : -0.1f)));
			shadowWithOffset.Set ();

			NSColor.Gray.SetFill ();
			mainBoxPath.AddClip ();

			var transform = new NSAffineTransform ();
			transform.Translate (-(float)Math.Round (mainBoxBorderRect.Size.Width), 0);
			transform.TransformBezierPath (mainBoxNegativePath).Fill ();
			context.RestoreState ();

			NSColor.Gray.SetStroke ();
			mainBoxPath.LineWidth = 0.5f;
			mainBoxPath.Stroke ();

			// Box Head Drawing
			var headingBoxCornerRadius = 4.0f;
			var headingBoxRect = new RectangleF (drawFrame.X + 4.5f, drawFrame.Y + drawFrame.Height - 36.0f, drawFrame.Width - 9.0f, 25.0f);
			var headingBoxInnerRect = new RectangleF (headingBoxRect.X, headingBoxRect.Y, headingBoxRect.Width, headingBoxRect.Height);
			headingBoxInnerRect.Offset (headingBoxCornerRadius, headingBoxCornerRadius);

			var headingBoxPath = new NSBezierPath ();
			headingBoxPath.MoveTo (new PointF (headingBoxRect.X, headingBoxRect.Y));
			headingBoxPath.LineTo (new PointF (headingBoxRect.Right, headingBoxRect.Y));
			headingBoxPath.AppendPathWithArc (new PointF (headingBoxInnerRect.Width, headingBoxInnerRect.Bottom), headingBoxCornerRadius, 0.0f, 90.0f);
			headingBoxPath.AppendPathWithArc (new PointF (headingBoxInnerRect.X, headingBoxInnerRect.Bottom), headingBoxCornerRadius, 90.0f, 180.0f);
			headingBoxPath.ClosePath ();

			context.SaveState ();
			headerShadow.Set ();
			context.BeginTransparencyLayer (null);
			headingGradient.DrawInBezierPath (headingBoxPath, -90);
			context.EndTransparencyLayer ();
			context.RestoreState ();

			NSColor.Gray.SetStroke ();
			headingBoxPath.LineWidth = 0.5f;
			headingBoxPath.Stroke ();
		}
	}
}

