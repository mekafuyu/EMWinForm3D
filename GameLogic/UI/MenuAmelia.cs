using System;
using System.Drawing;

namespace GameLogic;

public class MenuAmelia : Component
{
  private float countMove = 0f;
  private int blink = 0;


  public MenuAmelia(Image i, PointF position)
  {
    this.Sprite = i;
    this.Position = position;
    this.SpriteRec = new(0, 0, i.Width / 4, i.Height);
  }

  public void Draw(Graphics g, SizeF size)
  {
    if(blink >= 0)
    {
      this.SpriteRec = new(Sprite.Width / 4 * blink, 0, Sprite.Width / 4, Sprite.Height);
      blink--;
    }
    else
    {
      if(Random.Shared.Next(100) > 98)
        blink = 3;
    }
    float ratio = size.Width / size.Height;

    float displace = MathF.Cos(countMove) * 0.0375f - 0.05f;
    countMove += 0.025f;
    
    // proporcao de 2/3, diminuido metade para centralizar
    float newAmeliaX = size.Width * (Position.X - 0.1f);
    float newAmeliaY = size.Height * (Position.Y + displace - (0.15f * ratio));

    Rec = new RectangleF(
      newAmeliaX,
      newAmeliaY,
      size.Width * 0.2f,
      size.Height * 0.3f * ratio
    );

    g.DrawImage(
      Sprite,
      Rec,
      SpriteRec,
      GraphicsUnit.Pixel
    );
  }
}