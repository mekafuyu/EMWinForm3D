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
  public void GetFrame(
   (float width, float height) size,
   Graphics g,
   IEnumerable<Mesh> meshesToRender,
   (float x, float y, float z) rotation,
   (float x, float y, float z) translation,
   bool fillShape,
   bool showMesh,
   Vertex p,
   Amelia amelia
   )
  {
    int totalTriangles = 0;
    int[] rgb = new int[] { 128, 128, 255 };

    var rotationMatrix = MatrixMath.GetRotationMatrix(rotation.x, rotation.y, rotation.z);
    this.VirtualCamera.RefreshView();

    List<Triangle> trianglesToRaster = new();
    foreach (var mesh in meshesToRender)
    {
      totalTriangles += transformMeshToBuffer(mesh, rotationMatrix, translation, size, trianglesToRaster);
    }

    trianglesToRaster = trianglesToRaster.OrderBy(t => t.ZPos).ToList();

    foreach (var triangle in trianglesToRaster)
    {
      var clipped = new Triangle[2];

      List<Triangle> listTr = new(){ triangle };
      int newTriangles = 1;

      for (int plane = 0; plane < 4; plane++)
      {
        int trisToAdd = 0;
        
        while (newTriangles > 0)
        {
          Triangle trToTest = listTr.First();
          listTr.RemoveAt(0);
          newTriangles--;

          switch (plane)
          {
            case 0:
              (trisToAdd, clipped) = TriangleMath.ClipAgainstPlane(
                new(0.0f, 0.0f, 0.0f),
                new(0.0f, 1.0f, 0.0f),
                trToTest);
              // trisToAdd += sum;
              break;
            case 1:
              (trisToAdd, clipped) = TriangleMath.ClipAgainstPlane(
                new(0.0f, size.height - 1, 0.0f),
                new(0.0f, -1.0f, 0.0f),
                trToTest);
              // trisToAdd += sum;
              break;
            case 2:
              (trisToAdd, clipped) = TriangleMath.ClipAgainstPlane(
                new(0.0f, 0.0f, 0.0f),
                new(1.0f, 0.0f, 0.0f),
                trToTest);
              // trisToAdd += sum;
              break;
            case 3:
              (trisToAdd, clipped) = TriangleMath.ClipAgainstPlane(
                new(size.width - 1, 0.0f, 0.0f),
                new(-1.0f, 0.0f, 0.0f),
                trToTest);
              // trisToAdd += sum;
              break;
            default:
              break;
          }

          for (int tw = 0; tw < trisToAdd; tw++)
          {
            listTr.Add(clipped[tw]);
          }
        }
        newTriangles = listTr.Count;
      }

      foreach (var clippedTr in listTr)
      {
        RasterTriangle(clippedTr, g, rgb, fillShape, showMesh);
      }
    }

    var (newP, resize) = renderPoint(p, size);
    if(resize > 0)
    {
      EMUtils.Drawing.DrawPoint(Brushes.Red, g, newP, 1000 / resize);
      amelia.X = newP.X;
      amelia.Y = newP.Y;
      amelia.Size = 10000 / resize;
      RenderAmelia(amelia, g);
    }

    

    g.DrawString("EM3D v0.0.8", SystemFonts.DefaultFont, Brushes.White, new PointF(0f, 0f));
    g.DrawString("FPS: " + fpsCalculator().ToString(), SystemFonts.DefaultFont, Brushes.White, new PointF(0f, 10f));
    g.DrawString("Triangles: " + trianglesToRaster.Count.ToString() + "/" + totalTriangles, SystemFonts.DefaultFont, Brushes.White, new PointF(0f, 20f));
  }

}
