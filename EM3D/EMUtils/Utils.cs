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
      new(tr.P.v1.X, tr.P.v1.Y),
      new(tr.P.v2.X, tr.P.v2.Y),
      new(tr.P.v3.X, tr.P.v3.Y),
    };
    return points;
  }
}