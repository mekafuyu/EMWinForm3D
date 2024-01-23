using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;

using static EM3D.EMUtils.EMGeometry;

namespace EM3D;

public partial class EMEngine
{
  private (Vertex, float) renderPoint(
      Vertex p,
      (float width, float height) size
    )
  {
    var pClone = p;
    // Move farther
    pClone.Z += 8;

    // Convert World Space to view Space
    pClone = Vector4.Transform(pClone.V4, this.VirtualCamera.ViewMatrix);

    // The point to test is zNear, the plane is the Z axis, 
    float distance = TriangleMath.distancePointPlane(pClone.V3, new(0f, 0f, this.fNear), new(0f, 0f, 1f));
    if (distance <= 0)
      return (p, 0);

    pClone.V4 = Vector4.Transform(pClone.V4, this.VirtualCamera.ProjectionMatrix);
    pClone.V3 /= pClone.W;

    // Scale triangle
    pClone.X += 1f;
    pClone.Y += 1f;
    pClone.X *= 0.5f * size.width;
    pClone.Y *= 0.5f * size.height;

    // trProjected.lightIntensity =
    // trianglesToRasterBuffer.Add(trProjected);      

    return (pClone, distance);
  }

  public (PointF[], bool) project2Points(Vertex[] figureVertexes, (float width, float height) size)
  {
    PointF[] points2D = new PointF[figureVertexes.Length];
    bool notClipping = true;

    for (int i = 0; i < figureVertexes.Length; i++)
    {
      var (vProj, r) = renderPoint(figureVertexes[i], size);
      points2D[i] = new(vProj.X, vProj.Y);
      if(TriangleMath.distancePointPlane(figureVertexes[i].V3, VirtualCamera.VCamera, VirtualCamera.VLookDirection) <= 0)
        notClipping = false;
    };

    return (points2D, notClipping);
  }

  public void RenderAmelia(Amelia amelia, Graphics g, (float h, float w) size)
  {
    var (newPAmelia, ameliaResize) = renderPoint(amelia.Pos3D, size);
    if(ameliaResize > 0)
    {
      amelia.X = newPAmelia.X;
      amelia.Y = newPAmelia.Y;
      var d = Vector3.Distance(VirtualCamera.VCamera, amelia.Pos3D.V3);
      // var d = TriangleMath.distancePointPlane(am)
      amelia.Draw(g, d, fAspectRatio);
    }

    var ameliaHibox = new Vertex[]{
      new(amelia.Pos3D.X - 2, amelia.Pos3D.Y - 2.5f, amelia.Pos3D.Z - 2),
      new(amelia.Pos3D.X + 2, amelia.Pos3D.Y - 2.5f, amelia.Pos3D.Z - 2),
      new(amelia.Pos3D.X + 2, amelia.Pos3D.Y - 2.5f, amelia.Pos3D.Z + 2),
      new(amelia.Pos3D.X - 2, amelia.Pos3D.Y - 2.5f, amelia.Pos3D.Z + 2)
    };

    var (points2D, draw) = project2Points(ameliaHibox, size);
    if(draw)
    {
      var path = new GraphicsPath();

      path.AddLines(points2D);
      path.CloseFigure();

      g.DrawPath(Pens.Red, path);
    }
  }

  public void RenderWall(Wall wall, Graphics g, (float h, float w) size)
  {
    var (points2D, draw) = project2Points(wall.vRec3D, size);
    if(draw)
    {
      var path = new GraphicsPath();

      path.AddLines(points2D);
      path.CloseFigure();

      g.DrawPath(Pens.Red, path);
    }
  }
}
