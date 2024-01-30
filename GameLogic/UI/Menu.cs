using System.Drawing;

namespace GameLogic;

public class Menu
{
  public Image Background;
  public Button ButtonStart;
  public Title TitleGame;
  public MenuAmelia Amelia;
  public SizeF Size;
  private bool btnPressed = false;

  public Menu(SizeF screenSize)
  {
    this.Size = screenSize;

    this.Background = Image.FromFile("./assets/imgs/menuBackgroundFinal.png");
    float scaleBt = 4f;
    this.ButtonStart =new(
      Image.FromFile("./assets/imgs/menuButton.png"),
      new RectangleF(500f, 500f, 52f * scaleBt, 46f * scaleBt)
    );

    float scaleTitle = 4f;

    this.TitleGame = new(
      Image.FromFile("./assets/imgs/menuTitle.png"),
      new RectangleF(475f, 250f, 67f * scaleTitle, 37f * scaleTitle)
    );

    float scaleAmelia = 4.5f;
    this.Amelia = new(
      Image.FromFile("./assets/imgs/menuAmeliaSprite.png"),
      new RectangleF(
        Size.Width / 2 - 96f * scaleAmelia / 2,
        Size.Height / 2 - 146f * scaleAmelia / 2,
        96f * scaleAmelia,
        146f * scaleAmelia)
    );
  }

  public void Draw(Graphics g, float width, float height)
  {
    g.DrawImage(Background, 0, 0, width, height);
    TitleGame.Draw(g);
    ButtonStart.Draw(g);
    Amelia.Draw(g);
  }

  public void ClickMenu()
  {

  }
}