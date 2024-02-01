using System.Drawing;

namespace GameLogic;

public class Button : Component
{
  public RectangleF ButtonHitbox;
  public RectangleF RecDefault = new(0, 0, 52, 46);
  public RectangleF RecClick = new(0, 46, 52, 46);
  public bool Click = false;

  public Button(Image i, PointF position)
  {
    this.Sprite = i;
    this.Position = position;
  }

  public void Draw(Graphics g, SizeF size)
  {
    float ratio = size.Width / size.Height;
    
    Rec = new(
      Position.X * size.Width,
      Position.Y * size.Height,
      size.Width * 0.125f,
      size.Height * 0.1f
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