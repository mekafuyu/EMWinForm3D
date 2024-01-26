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
  private (Vertex, float) projectPoint(
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

  public void RenderPoint(Brush b, Graphics g, Vertex point, (float h, float w) size)
  {
    var (newPoint, d) = projectPoint(point, size);
    g.FillEllipse(b, new RectangleF(newPoint.X, newPoint.Y, 5, 5));
  }
  public void RenderPoint(Brush b, Graphics g, PointF point, (float h, float w) size)
  {
    g.FillEllipse(b, new RectangleF(point.X, point.Y, 5, 5));
  }

  public (PointF[], bool) ProjectHitbox(Entity e, (float h, float w) size)
  {
    PointF[] points2D = new PointF[4];
    Vertex[] points3D = new Vertex[]{
      new(e.Hitbox.Right, e.Anchor3D.Y, e.Hitbox.Top),
      new(e.Hitbox.Left, e.Anchor3D.Y, e.Hitbox.Top),
      new(e.Hitbox.Left, e.Anchor3D.Y, e.Hitbox.Bottom),
      new(e.Hitbox.Right, e.Anchor3D.Y, e.Hitbox.Bottom),
    };

    bool notClipping = true;

    for (int i = 0; i < 4; i++)
    {
      var (vProj, r) = projectPoint(points3D[i], size);
      points2D[i] = new(vProj.X, vProj.Y);

      if(TriangleMath.distancePointPlane(
        points3D[i].V3,
        VirtualCamera.VCamera,
        VirtualCamera.VLookDirection) <= 0
      )
        notClipping = false;
    };

    return (points2D, notClipping);
  }

  public void RenderAmelia(Amelia amelia, Graphics g, (float h, float w) size)
  {
    // Vertex ameliaNP = new(amelia.Anchor3D.X, amelia.Anchor3D.Y + amelia.RealSize / 2, amelia.Anchor3D.Z);
    var (newPAmelia, ameliaResize) = projectPoint(amelia.Anchor3D, size);

    if(ameliaResize > 0)
    {
      amelia.X = newPAmelia.X;
      amelia.Y = newPAmelia.Y;
      var d = Vector3.Distance(VirtualCamera.VCamera, amelia.Anchor3D.V3);
      amelia.Draw(g, d, fAspectRatio);
    }
    RenderPoint(Brushes.Yellow, g, new PointF(newPAmelia.X, newPAmelia.Y), size);
    
    var (pointsHitbox, draw) = ProjectHitbox(amelia, size);
    if(draw)
    {
      var path = new GraphicsPath();

      path.AddLines(pointsHitbox);
      path.CloseFigure();

      g.DrawPath(Pens.Red, path);
    }
  }

  public void RenderWall(Wall wall, Graphics g, (float h, float w) size)
  {
    var (pointsHitbox, draw) = ProjectHitbox(wall, size);
    if(draw)
    {
      var path = new GraphicsPath();

      path.AddLines(pointsHitbox);
      path.CloseFigure();

      g.DrawPath(Pens.Red, path);
    }
  }

  public void RenderDoor(Door door, Graphics g, (float h, float w) size)
  {
    var (pointsHitbox, draw) = ProjectHitbox(door, size);
    if(draw)
    {
      var path = new GraphicsPath();

      path.AddLines(pointsHitbox);
      path.CloseFigure();

      if(door.IsOpen)
        g.DrawPath(Pens.Green, path);
      else
        g.DrawPath(Pens.Yellow, path);

    }
  }
}
