using System;
using System.Drawing;

namespace GameLogic;

public class MenuAmelia
{
  public Image MenuAmeliaSprite;
  public RectangleF MenuAmeliaPosition;
  public RectangleF RecDefault;
  private float aceleration = 0.1f;
  private float speed = 0;
  private float displace = 1;
  private int blink = 0;


  public MenuAmelia(Image i, RectangleF position)
  {
    this.MenuAmeliaSprite = i;
    this.MenuAmeliaPosition = position;
    this.RecDefault = new(0, 0, i.Width / 4, i.Height);
  }

  public void Draw(Graphics g)
  {
    if(blink >= 0)
    {
      this.RecDefault = new(MenuAmeliaSprite.Width / 4 * blink, 0, MenuAmeliaSprite.Width / 4, MenuAmeliaSprite.Height);
      blink--;
    }
    else
    {
      if(Random.Shared.Next(100) > 98)
        blink = 3;
    }


    if(Math.Abs(speed) > 3)
      aceleration = -aceleration;

    var recToDraw = new RectangleF(
      MenuAmeliaPosition.X,
      MenuAmeliaPosition.Y - displace,
      MenuAmeliaPosition.Width,
      MenuAmeliaPosition.Height
    );

    speed += aceleration;
    displace += speed;

    g.DrawImage(
      MenuAmeliaSprite,
      recToDraw,
      RecDefault,
      GraphicsUnit.Pixel
    );
  }
}