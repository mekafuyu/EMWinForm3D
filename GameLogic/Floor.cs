using System.Drawing;
using EM3D;
public class Floor : Entity
{
  Image spritesheet = null;
  public Floor(float x, float y, float z, float length, float width)
  {
    this.Anchor3D = new(x, y, z);
    this.Hitbox = new(x, z, width, length);
  }

  public void Draw(Graphics g, PointF p)
  {
    g.DrawImage(spritesheet, p);
  }
}