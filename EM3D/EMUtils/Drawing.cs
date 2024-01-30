using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using static EM3D.EMUtils.Utils;

namespace EM3D.EMUtils;

public static class Drawing
{
  public static int gifIndex = 0;
  public static void FillTriangleWithTexture(Graphics g, Triangle tr, Image img)
  {
    if(gifIndex > 69)
      gifIndex = 0;

    var path = new GraphicsPath();

    PointF[] pts = TriangleToPointFs(tr);
    path.AddLines(pts);

    path.CloseFigure();

    g.SetClip(path);

    RectangleF imageBounds = path.GetBounds();
    PointF imageLocation = new PointF(imageBounds.X, imageBounds.Y);
    SizeF imageSize = new SizeF(imageBounds.Width, imageBounds.Height);
    if(tr.lightIntensity < 0.3f)
      tr.lightIntensity = 0.3f;

    Brush b = new SolidBrush(Color.FromArgb(255 - (int) (255 * tr.lightIntensity),0,0,0));

    g.DrawImage(img,
      new RectangleF(imageLocation, imageSize),
      new RectangleF(220 * gifIndex, 0, 220, 220),
      GraphicsUnit.Pixel);

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