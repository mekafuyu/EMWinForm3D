using System.Drawing;

namespace GameLogic;

public class Button
{
  public Image ButtonSprite;
  public RectangleF ButtonPosition;
  public RectangleF RecDefault = new(0, 0, 52, 46);
  public RectangleF RecClick = new(0, 46, 52, 46);
  public bool Click = false;

  public Button(Image i, RectangleF position)
  {
    this.ButtonSprite = i;
    this.ButtonPosition = position;
  }

  public void Draw(Graphics g)
  {
    if(Click)
      g.DrawImage(
        ButtonSprite,
        ButtonPosition,
        RecClick,
        GraphicsUnit.Pixel
      );
    else
      g.DrawImage(
        ButtonSprite,
        ButtonPosition,
        RecDefault,
        GraphicsUnit.Pixel
      );
  }
}