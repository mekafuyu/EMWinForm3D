using System.Drawing;

namespace GameLogic;

public class Button : Component
{
  public RectangleF ButtonHitbox;
  public RectangleF RecDefault = new(0, 0, 52, 46);
  public RectangleF RecClick = new(0, 46, 52, 46);
  public bool Click = false;

  public Button(Image i, PointF position, float width, float height)
  {
    this.Sprite = i;
    this.Position = position;
    this.Width = width;
    this.Height = height;
  }

  public void Draw(Graphics g, SizeF size)
  {
    Rec = new(
      Position.X * size.Width,
      Position.Y * size.Height,
      Width * size.Width,
      Height * size.Height
    );

    if(Click)
      g.DrawImage(
        Sprite,
        Rec,
        RecClick,
        GraphicsUnit.Pixel
      );
    else
      g.DrawImage(
        Sprite,
        Rec,
        RecDefault,
        GraphicsUnit.Pixel
      );
  }
}