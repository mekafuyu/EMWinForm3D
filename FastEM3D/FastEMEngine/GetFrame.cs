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
  public void GetFrame(
   (float width, float height) size,
   Graphics g,
   IEnumerable<Mesh> meshesToRender,
   (float x, float y, float z) rotation,
   (float x, float y, float z) translation,
   bool fillShape,
   bool showMesh
   )
  {
    int totalTriangles = 0;
    int[] rgb = new int[] { 128, 128, 255 };

    var rotationMatrix = MatrixMath.GetRotationMatrix(rotation.x, rotation.y, rotation.z);
    this.RefreshView();

    List<Triangle> trianglesToRaster = new();
    foreach (var mesh in meshesToRender)
    {
      transformMeshToBuffer(mesh, rotationMatrix, translation, size, trianglesToRaster);
      totalTriangles += mesh.t.Count();
    }
    trianglesToRaster = trianglesToRaster.OrderBy(t => t.ZPos).ToList();

    g.Clear(Color.Black);
    foreach (var triangle in trianglesToRaster)
    {
      RasterTriangle(triangle, g, rgb, fillShape, showMesh);
    }

    g.DrawString("EM3D v0.0.8", SystemFonts.DefaultFont, Brushes.White, new PointF(0f, 0f));
    g.DrawString("FPS: " + fpsCalculator().ToString(), SystemFonts.DefaultFont, Brushes.White, new PointF(0f, 10f));
    g.DrawString("Triangles: " + trianglesToRaster.Count.ToString() + "/" + totalTriangles, SystemFonts.DefaultFont, Brushes.White, new PointF(0f, 20f));
  }

}
