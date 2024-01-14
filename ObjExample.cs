using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using EM3D;
using static EM3D.EMUtils.Drawing;
using static EM3D.EMUtils.Geometry;
using static EM3D.EMUtils.Utils;

public class ObjExample
{
  public EMEngine Eng;
  public int[] rgb;
  private int fps = 0;
  private DateTime lastCheckTime;
  private Matrix4x4 mrx, mry, mrz;

  public void renderFrame(
    PictureBox pb,
    Form form,
    Graphics g,
    IEnumerable<Mesh> meshesToRender,
    (float rx, float ry, float rz) rotationAngles,
    (float tx, float ty, float tz) translationOffset
    )
  {
    double secondsElapsed = (DateTime.Now - lastCheckTime).TotalSeconds;
    lastCheckTime = DateTime.Now;
    fps = (int) (1 / secondsElapsed);
    g.Clear(Color.Black);
    g.DrawString("EM3D v0.0.6", SystemFonts.DefaultFont, Brushes.White, new PointF(0f, 0f));
    g.DrawString(fps.ToString(), SystemFonts.DefaultFont, Brushes.White, new PointF(0f, 10f));

    mrx = GetRotateInXMatrix(rotationAngles.rx);
    mry = GetRotateInYMatrix(rotationAngles.ry);
    mrz = GetRotateInZMatrix(rotationAngles.rz);
    mrx *= mry * mrz;

    Pen p = new Pen(Color.FromArgb(255, 0, 0, 0), 2 * form.Width / form.Height);

    List<Triangle> trianglesToRaster = new();
    foreach (var mesh in meshesToRender)
    {
      foreach (var meshTri in mesh.t)
      {
        if (meshTri is null)
          continue;
        var moddedTri = (Triangle)meshTri.Clone();
        moddedTri = RotateTriangle3D(moddedTri, mrx);
        moddedTri = TranslateTriangle3D(moddedTri, translationOffset.tx, translationOffset.ty, translationOffset.tz);

        var triProj = ProjectTriangle(
          moddedTri,
          Eng.LightDirection,
          Eng.VCamera,
          Eng.matProj,
          (form.Width, form.Height)
        );
        if (triProj is null)
          continue;
        trianglesToRaster.Add(triProj);
      }

      trianglesToRaster = trianglesToRaster.OrderBy(t => t.zPos).ToList();

      foreach (var triangle in trianglesToRaster)
      {
        SolidBrush b =
          new(
            Color.FromArgb(
              (int)(rgb[0] * triangle.lightIntensity),
              (int)(rgb[1] * triangle.lightIntensity),
              (int)(rgb[2] * triangle.lightIntensity)
            )
          );
        FillTriangleWithGraphics(b, g, triangle);
        // DrawTriangleWithGraphics(p, g, triangle);
      }
    }
    pb.Refresh();
  }
}