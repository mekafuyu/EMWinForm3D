using System.Drawing;
using EM3D;
public class Wall : Entity
{
  Image spritesheet = null;
  public float Size { get; set; }
  public float X { get; set; }
  public float Y { get; set; }
  public Vertex[] vRec3D { get; set; }

  public RectangleF hitbox {
    get
      => new RectangleF(
        vRec3D[0].X,
        vRec3D[0].Z,
        vRec3D[0].X - vRec3D[1].X,
        vRec3D[0].Z - vRec3D[1].Z
      );
  }

  public void Draw(Graphics g, PointF p)
  {
    g.DrawImage(spritesheet, p);
  }
}