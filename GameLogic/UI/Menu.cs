using System.Drawing;

namespace GameLogic;

public class Menu
{
  public Image Background;
  public Button ButtonStart;
  public Title TitleGame;
  public MenuAmelia Amelia;
  public SizeF Size;

  public Menu(SizeF screenSize)
  {
    this.Size = screenSize;

    this.Background = Image.FromFile("./assets/imgs/menuBackgroundFinal.png");
    float scaleBt = 4f;
    this.ButtonStart =new(
      Image.FromFile("./assets/imgs/menuButton.png"),
      new PointF(
        0.25f,
        0.45f
      )
    );

    float scaleTitle = 4f;

    this.TitleGame = new(
      Image.FromFile("./assets/imgs/menuTitle.png"),
      new RectangleF(475f, 250f, 67f * scaleTitle, 37f * scaleTitle)
    );

    this.Amelia = new(
      Image.FromFile("./assets/imgs/menuAmeliaSprite.png"),
      new PointF(
        0.5f,
        0.5f
      )
    );
  }

  public void Draw(Graphics g, SizeF size)
  {
    g.DrawImage(Background, 0, 0, size.Width, size.Height);
    TitleGame.Draw(g, size);
    ButtonStart.Draw(g, size);
    Amelia.Draw(g, size);
  }

  public void ClickMenu()
  {

  }
}