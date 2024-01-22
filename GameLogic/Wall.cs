using System.Drawing;

//tenho que fazer um singleton
public class Wall : Entity
{
  Image spritesheet = null;
  public float Size { get; set; } 
  public float X { get; set; } 
  public float Y { get; set; } 
  public float Speed { get; set; }


  public void Draw(Graphics g, PointF p)
  {
    g.DrawImage( spritesheet, p);
  }
}