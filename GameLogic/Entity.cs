using System.Drawing;
using EM3D;

public class Entity
{
  public float Width { get; set; }
  public float Height { get; set; }
  public float Length { get; set; }
  public float RealSize { get; set; }
  public float X { get; set; }
  public float Y { get; set; }
  public float Speed { get; set; }
  public RectangleF Hitbox { get; set; }
  public Vertex Anchor3D { get; set; }
  public virtual void Draw(Graphics g) { }

  public static Entity Scale(Entity e, float scale)
  {
    e.Width *= scale;
    e.Height *= scale;
    e.Length *= scale;
    e.RealSize *= scale;
    e.X *= scale;
    e.Y *= scale;
    e.Hitbox = new(
      e.Hitbox.X * scale,
      e.Hitbox.Y * scale,
      e.Width * scale,
      e.Height * scale);
    e.Anchor3D = e.Anchor3D.V3 * scale;
    return e;
  }
}