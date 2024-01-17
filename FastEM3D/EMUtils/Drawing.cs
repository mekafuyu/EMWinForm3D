using System.Drawing;
using System.Drawing.Drawing2D;
using static FastEM3D.EMUtils.Utils;

namespace FastEM3D.EMUtils;

public static class Drawing
{
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