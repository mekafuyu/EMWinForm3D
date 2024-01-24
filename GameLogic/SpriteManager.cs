using System;
using System.Drawing;

public class SpriteManager
{
  Image spritesheet = null;
  DateTime dt = DateTime.Now;
  public float Speed { get; set; }
  public int TotalSprites { get; set; }
  public int SpriteIndex = 0;
  public int StartIndex = 0;
  public int QuantSprite = 0;
  public SpriteManager(string filePath, float speed, int totalSprites)
  {
    spritesheet = Bitmap.FromFile(filePath);
    Speed = speed;
    TotalSprites = totalSprites;
  }

  public void Draw(Graphics g, PointF centerPoint, float sx = 1f, float sy = 1f)
  {
    var size = new SizeF(91 * sx, 91 * sy);
    var left = new PointF(centerPoint.X - size.Width / 2, centerPoint.Y - size.Height);
    var rec = new RectangleF(left, size);
    g.DrawImage(spritesheet,
      rec,
      getRectanglesInterval(this.StartIndex, this.QuantSprite),
      GraphicsUnit.Pixel
    );
    g.DrawRectangle(Pens.Blue, rec);
  }

  private Rectangle getRectanglesInterval(int index, int quantitySprites)
  {
    this.SpriteIndex++;
    if (this.SpriteIndex + 1 > quantitySprites * this.Speed)
      this.SpriteIndex = 0;
    return getRectangleByIndex((int)Math.Floor(this.SpriteIndex / this.Speed) + index);
  }

  private Rectangle getCurrentRectangle()
  {
    var time = DateTime.Now - dt;
    var totalsecs = time.TotalSeconds;
    int index = (int)(totalsecs * Speed % TotalSprites);

    Rectangle sprite = getRectangleByIndex(index);

    return sprite;
  }

  private Rectangle getRectangleByIndex(int index)
  {
    int width = spritesheet.Width / 91;

    int x = index % width;
    int y = index / width;
    Rectangle rec = new Rectangle(x * 91, y * 91, 91, 91);

    return rec;
  }
}