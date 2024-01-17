using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

using FastEM3D.EMUtils;
using static FastEM3D.EMUtils.EMGeometry;

namespace FastEM3D;

public partial class FastEMEngine
{
  private void RasterTriangle(Triangle tr, Graphics g, int[] rgb, bool fillPoly, bool drawPolly)
  {
    Pen p = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
    SolidBrush b =
      new(
        Color.FromArgb(
          (int)(rgb[0] * tr.lightIntensity),
          (int)(rgb[1] * tr.lightIntensity),
          (int)(rgb[2] * tr.lightIntensity)
        )
      );

    var trPoints = Utils.TriangleToPointFs(tr);

    if (fillPoly)
      Drawing.FillTriangleWithGraphics(b, g, trPoints);
    if (drawPolly)
      Drawing.DrawTriangleWithGraphics(p, g, trPoints);
  }
}
