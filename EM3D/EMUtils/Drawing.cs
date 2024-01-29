using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using static EM3D.EMUtils.Utils;

namespace EM3D.EMUtils;

public static class Drawing
{
  public static void FillTriangleWithTexture(Graphics g, Triangle tr, Image img)
  {
    var path = new GraphicsPath();

    PointF[] pts = TriangleToPointFs(tr);
    path.AddLines(pts);

    path.CloseFigure();

    g.SetClip(path);

    RectangleF imageBounds = path.GetBounds();
    
    float scale = Math.Min(imageBounds.Width / img.Width, imageBounds.Height / img.Height);

    PointF imageLocation = new PointF(imageBounds.X, imageBounds.Y);
    // SizeF imageSize = new SizeF(img.Width * scale, img.Height * scale);
    SizeF imageSize = new SizeF(imageBounds.Width, imageBounds.Height);

    // Draw the clipped image inside the triangle
    if(tr.lightIntensity < 0.3f)
      tr.lightIntensity = 0.3f;
    Brush b = new SolidBrush(Color.FromArgb(255 - (int) (255 * tr.lightIntensity),0,0,0));
    g.DrawImage(img, new RectangleF(imageLocation, imageSize));
    g.FillPath(b, path);
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

  public static void DrawPoint(Brush b, Graphics g, Vertex p, float size)
  {
    g.FillEllipse(b, new RectangleF(p.X - size / 2, p.Y, size, size));
  }
}