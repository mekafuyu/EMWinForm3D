using System.Collections.Generic;
using System.Drawing;

public class Sprite
{
  public int CurrentCode { get; set; }
  private Dictionary<int, SpriteManager> spriteCollection = new();

  public void Draw(Graphics g, PointF leftUpCornerPoint, float sx = 1f, float sy = 1f)
  {
    var manager = spriteCollection[CurrentCode];
    manager.Draw(g, leftUpCornerPoint, sx, sy);
  }

  public void Add(int code, SpriteManager spriteManager)
    => this.spriteCollection.Add(code, spriteManager);
}