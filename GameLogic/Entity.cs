using System.Drawing;

public class Entity
{
  public float Size { get; set; }
  public float X { get; set; }
  public float Y { get; set; }
  public float Speed { get; set; }
  public Rectangle rec = new Rectangle();
  private SpriteManager manager;

  public void Draw(Graphics g)
  {

  }
}