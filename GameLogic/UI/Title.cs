using System;
using System.Drawing;

namespace GameLogic;

public class Title : Component
{
  private float countScale = 0.2f;

  public Title(Image i, PointF position, float width, float height)
  {
    this.Sprite = i;
    this.Position = position;
    this.SpriteRec = new(0, 0, i.Width, i.Height);
    this.Width = width;
    this.Height = height;
  }

  public void Draw(Graphics g, SizeF size)
  {
    float scale = MathF.Cos(countScale) * 0.01f;
    countScale += 0.025f;

    Rec = new RectangleF(
      size.Width * (Position.X + (- Width - scale) / 2),
      size.Height * (Position.Y + ( - Height - scale) / 2),
      size.Width * (Width + scale),
      size.Height * (Height + scale)
    );

    g.DrawImage(
      Sprite,
      Rec,
      SpriteRec,
      GraphicsUnit.Pixel
    );
  }
}