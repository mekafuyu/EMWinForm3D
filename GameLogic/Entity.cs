using System.Drawing;
using EM3D;

public class Entity
{
  public float Size { get; set; }
  public float X { get; set; }
  public float Y { get; set; }
  public float Speed { get; set; }
  public RectangleF Hitbox { get; set; }
  public Vertex Anchor3D { get; set; }
  public virtual void Draw(Graphics g)
  {

  }
}