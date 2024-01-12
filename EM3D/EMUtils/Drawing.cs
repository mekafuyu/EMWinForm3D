using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EM3D.EMUtils;

public static class Drawing
{
   public static void FillTriangleWithGraphics(Brush b, Graphics g, Triangle tr)
  {
    var path = new GraphicsPath();
    PointF[] pts = new PointF[]
    {
      new(tr.P[0].X, tr.P[0].Y),
      new(tr.P[1].X, tr.P[1].Y),
      new(tr.P[2].X, tr.P[2].Y),
    };

    path.AddLines(pts);
    path.CloseFigure();

    g.FillPath(b, path);
  }

  public static void DrawTriangleWithGraphics(Pen p, Graphics g, Triangle tr)
  {
    var path = new GraphicsPath();
    PointF[] pts = new PointF[]
    {
      new(tr.P[0].X, tr.P[0].Y),
      new(tr.P[1].X, tr.P[1].Y),
      new(tr.P[2].X, tr.P[2].Y),
    };

    path.AddLines(pts);
    path.CloseFigure();

    g.DrawPath(p, path);
  }
}