using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using static FastEM3D.EMUtils.Utils;

namespace FastEM3D.EMUtils;

public static class Drawing
{
  public static void FillTriangleWithTexture(Brush b, Graphics g, Triangle tr, Image img)
  {
    var path = new GraphicsPath();

    PointF[] pts = TriangleToPointFs(tr);
    path.AddLines(pts);
    path.CloseFigure();

    g.SetClip(path);

    RectangleF imageBounds = path.GetBounds();
    
    float scale = Math.Min(imageBounds.Width / img.Width, imageBounds.Height / img.Height);

    PointF imageLocation = new PointF(imageBounds.X, imageBounds.Y);
    SizeF imageSize = new SizeF(img.Width * scale, img.Height * scale);

    // Draw the clipped image inside the triangle
    g.DrawImage(img, new RectangleF(imageLocation, imageSize));
    g.ResetClip();
  }
  public static void FillTriangleWithGraphics(Brush b, Graphics g, Triangle tr)
  {
    var path = new GraphicsPath();

    PointF[] pts = TriangleToPointFs(tr);
    path.AddLines(pts);
    path.CloseFigure();

    g.FillPath(b, path);
  }
  public static void FillTriangleWithGraphics(Brush b, Graphics g, PointF[] points)
  {
    var path = new GraphicsPath();

    path.AddLines(points);
    path.CloseFigure();

    g.FillPath(b, path);
  }
  public static void DrawTriangleWithGraphics(Pen p, Graphics g, Triangle tr)
  {
    var path = new GraphicsPath();

    PointF[] pts = TriangleToPointFs(tr);
    path.AddLines(pts);
    path.CloseFigure();

    g.DrawPath(p, path);
  }
  public static void DrawTriangleWithGraphics(Pen p, Graphics g, PointF[] points)
  {
    var path = new GraphicsPath();
    path.AddLines(points);
    path.CloseFigure();

    g.DrawPath(p, path);
  }
}