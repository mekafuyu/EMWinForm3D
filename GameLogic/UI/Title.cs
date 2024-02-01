using System;
using System.Drawing;

namespace GameLogic;

public class Title
{
  public Image TitleSprite;
  public RectangleF TitlePosition;
  public RectangleF RecDefault;
  private float increase = 0.2f;
  private float displace = 1;

  public Title(Image i, RectangleF position)
  {
    this.TitleSprite = i;
    this.TitlePosition = position;
    this.RecDefault = new(0, 0, i.Width, i.Height);
  }

  public void Draw(Graphics g, SizeF size)
  {
    if(Math.Abs(displace) > 10)
      increase = -increase;

    var recToDraw = new RectangleF(
      TitlePosition.X - displace,
      TitlePosition.Y - displace,
      TitlePosition.Width + displace * 2,
      TitlePosition.Height + displace * 2
    );

    displace += increase;

    g.DrawImage(
      TitleSprite,
      recToDraw,
      RecDefault,
      GraphicsUnit.Pixel
    );
  }
}