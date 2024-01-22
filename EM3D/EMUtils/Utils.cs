using System.Drawing;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;

namespace EM3D.EMUtils;

public static class Utils
{
  public static PointF[] TriangleToPointFs(Triangle tr)
  {
    PointF[] points = new PointF[]
    {
      new(tr.P.l1.X, tr.P.l1.Y),
      new(tr.P.l2.X, tr.P.l2.Y),
      new(tr.P.l3.X, tr.P.l3.Y),
    };
    return points;
  }
}