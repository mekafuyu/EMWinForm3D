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
    this.ButtonStart = new(
      Image.FromFile("./assets/imgs/menuButton.png"),
      new PointF(.25f, .45f),
      .125f,
      .1875f
    );

    this.TitleGame = new(
      Image.FromFile("./assets/imgs/menuTitle.png"),
      new PointF(.325f, .3f),
      .15f,
      .15f
    );

    this.Amelia = new(
      Image.FromFile("./assets/imgs/menuAmeliaSprite.png"),
      new PointF(0.5f, 0.5f)
    );
  }

  public void Draw(Graphics g, SizeF size)
  {
    g.DrawImage(Background, 0, 0, size.Width, size.Height);
    TitleGame.Draw(g, size);
    ButtonStart.Draw(g, size);
    Amelia.Draw(g, size);
  }
}