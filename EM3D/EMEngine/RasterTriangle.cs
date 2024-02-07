using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

using EM3D.EMUtils;
using static EM3D.EMUtils.EMGeometry;

namespace EM3D;

public partial class EMEngine
{
  // public Image Txt = Image.FromFile("./assets/imgs/bg/monke.png");
  public Pen PenLine = new Pen(Color.FromArgb(255, 255, 255, 255), 1);
  
  private void RasterTriangle(Triangle tr, Graphics g, bool fillPoly, bool drawPolly)
  {
    // Pen p = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
    SolidBrush b =
      new(
        Color.FromArgb(
          (byte)(tr.Color[0] * tr.lightIntensity),
          (byte)(tr.Color[1] * tr.lightIntensity),
          (byte)(tr.Color[2] * tr.lightIntensity)
        )
      );

    var trPoints = Utils.TriangleToPointFs(tr);

    if (fillPoly)
      // Drawing.FillTriangleWithTexture(g, tr, Txt);
      Drawing.FillTriangleWithGraphics(b, g, trPoints);
    if (drawPolly)
      Drawing.DrawTriangleWithGraphics(PenLine, g, trPoints);
  }
}
