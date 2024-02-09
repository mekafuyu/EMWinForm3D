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
  public bool HideHitboxes = true;
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

      if (TriangleMath.distancePointPlane(
        points3D[i].V3,
        VirtualCamera.VCamera,
        VirtualCamera.VLookDirection) <= 0
      )
        notClipping = false;
    };

    return (points2D, notClipping);
  }

  public void RenderEntitity(Graphics g, Entity entity, (float h, float w) size)
  {
    if (entity is Amelia amelia)
    {
      RenderAmelia(amelia, g, size);
      return;
    }
    if (entity is Book book)
    {
      RenderBook(book, g, size);
    }
    if(HideHitboxes)
      return;

    if (entity is Wall wall)
    {
      RenderWall(wall, g, size);
      return;
    }
    if (entity is Door door)
    {
      RenderDoor(door, g, size);
      return;
    }
    if (entity is Portal portal)
    {
      RenderPortal(portal, g, size);
    }
    if (entity is Floor floor)
    {
      RenderFloor(floor, g, size);
    }
    if (entity is PerspectiveObstacle persp)
    {
      RenderPersp(persp, g, size);
    }
    if (entity is PerspectivePortal perspPor)
    {
      RenderPerspPortal(perspPor, g, size);
    }

  }

  public void RenderAmelia(Amelia amelia, Graphics g, (float h, float w) size)
  {
    // Vertex ameliaNP = new(amelia.Anchor3D.X, amelia.Anchor3D.Y + amelia.RealSize / 2, amelia.Anchor3D.Z);
    var (newPAmelia, ameliaResize) = projectPoint(amelia.Anchor3D, size);

    
    // RenderPoint(Brushes.Yellow, g, new PointF(newPAmelia.X, newPAmelia.Y), size);

    var (pointsHitbox, draw) = ProjectHitbox(amelia, size);
    if (draw)
    {
      var path = new GraphicsPath();

      path.AddLines(pointsHitbox);
      path.CloseFigure();

      RectangleF imageBounds = path.GetBounds();
      PointF imageLocation = new PointF(imageBounds.X, imageBounds.Y);
      SizeF imageSize = new SizeF(imageBounds.Width, imageBounds.Height);


      Brush b = new SolidBrush(Color.FromArgb(200, 0, 0, 0));

      g.FillEllipse(b,
        new RectangleF(imageLocation, imageSize));
      if(!HideHitboxes)
        g.DrawPath(Pens.Red, path);
    }
    if (ameliaResize > 0)
    {
      amelia.X = newPAmelia.X;
      amelia.Y = newPAmelia.Y;
      var d = Vector3.Distance(VirtualCamera.VCamera, amelia.Anchor3D.V3);
      if (d > 5)
        amelia.Draw(g, d, fAspectRatio);
    }
  }

  public void RenderWall(Wall wall, Graphics g, (float h, float w) size)
  {
    var (pointsHitbox, draw) = ProjectHitbox(wall, size);
    if (draw)
    {
      var path = new GraphicsPath();

      path.AddLines(pointsHitbox);
      path.CloseFigure();

      g.DrawPath(Pens.Red, path);
    }
  }
  public void RenderFloor(Floor floor, Graphics g, (float h, float w) size)
  {
    var (pointsHitbox, draw) = ProjectHitbox(floor, size);
    if (draw)
    {
      var path = new GraphicsPath();

      path.AddLines(pointsHitbox);
      path.CloseFigure();

      g.DrawPath(Pens.Cyan, path);
    }
  }

  public void RenderDoor(Door door, Graphics g, (float h, float w) size)
  {
    var (pointsHitbox, draw) = ProjectHitbox(door, size);
    if (draw)
    {
      var path = new GraphicsPath();

      path.AddLines(pointsHitbox);
      path.CloseFigure();

      if (door.IsOpen)
        g.DrawPath(Pens.Green, path);
      else
        g.DrawPath(Pens.Yellow, path);

    }
  }
  public void RenderPersp(PerspectiveObstacle perspObs, Graphics g, (float h, float w) size)
  {
    var (pointsHitbox, draw) = ProjectHitbox(perspObs, size);
    if (draw)
    {
      var path = new GraphicsPath();

      path.AddLines(pointsHitbox);
      path.CloseFigure();

      if (perspObs.IsOpen(VirtualCamera.VCamera))
        g.DrawPath(Pens.Red, path);
      else
        g.DrawPath(Pens.Green, path);

    }
  }
  public void RenderPerspPortal(PerspectivePortal perspObs, Graphics g, (float h, float w) size)
  {
    var (pointsHitbox, draw) = ProjectHitbox(perspObs, size);
    if (draw)
    {
      var path = new GraphicsPath();

      path.AddLines(pointsHitbox);
      path.CloseFigure();

      if (perspObs.IsOpen(VirtualCamera.VCamera))
        g.DrawPath(Pens.Pink, path);
      else
        g.DrawPath(Pens.Purple, path);

    }
  }

  public void RenderPortal(Portal portal, Graphics g, (float h, float w) size)
  {
    var (pointsHitbox, draw) = ProjectHitbox(portal, size);
    if (draw)
    {
      var path = new GraphicsPath();

      path.AddLines(pointsHitbox);
      path.CloseFigure();

      if (portal.IsOpen)
        g.DrawPath(Pens.Purple, path);
      else
        g.DrawPath(Pens.Pink, path);

    }
  }

  public void RenderBook(Book book, Graphics g, (float h, float w) size)
  {
    var (newPBook, bookResize) = projectPoint(book.Anchor3D, size);

    var (pointsHitbox, draw) = ProjectHitbox(book, size);
    if (draw)
    {
      var path = new GraphicsPath();

      path.AddLines(pointsHitbox);
      path.CloseFigure();

      RectangleF imageBounds = path.GetBounds();
      PointF imageLocation = new PointF(imageBounds.X, imageBounds.Y);
      SizeF imageSize = new SizeF(imageBounds.Width, imageBounds.Height);

      Brush b = new SolidBrush(Color.FromArgb(0, 0, 0, 0));

    }
    if (bookResize > 0)
    {
      book.X = newPBook.X;
      book.Y = newPBook.Y;
      var d = Vector3.Distance(VirtualCamera.VCamera, book.Anchor3D.V3);
      if (d > 5)
        book.Draw(g, d, fAspectRatio);
    }
  }
}