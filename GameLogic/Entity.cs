using System.Drawing;
using EM3D;

public class Entity
{
  public float Size { get; set; }
  public float X { get; set; }
  public float Y { get; set; }
  public float Speed { get; set; }
  public Rectangle rec = new Rectangle();
  public Vertex Pos3D { get; set; }
  private SpriteManager manager;

  public virtual void Draw(Graphics g)
  {

  }
}