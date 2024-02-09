
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EM3D;
using EM3D.EMUtils;
using GameLogic;

public partial class Game
{
  public List<Level> Levels;
  private Level currLevel;
  private int levelNumber = -1;
  public EMEngine Engine;
  public Graphics GameGraphics;
  public Bitmap Bmp;
  public PictureBox Pb;
  public Timer timer;
  public Form GameForm;
  public Menu StartMenu;
  public bool MenuOpen = true;
  public bool LastState = false;
  public PointF CursorPos = new();
  public bool IsDown;
  public int ColectedPages { get; set; } = 0;
  public int Pages { get; set; } = 7;

  public Game()
  {
    Levels = new();
    Engine = new();
  }

  public void SelectLevel(int level)
    => currLevel = Levels[level];

  public void Run()
  {
    Menu menu = null;

    ApplicationConfiguration.Initialize();
    this.Pb = new PictureBox { Dock = DockStyle.Fill };
    this.timer = new Timer { Interval = 2 };
    this.GameForm = new Form
    {
      WindowState = FormWindowState.Maximized,
      // FormBorderStyle = FormBorderStyle.None,
      Controls = { Pb }
    };
    this.cursorReset = new Point(Pb.Width / 2, Pb.Height / 2);

    GameForm.Load += (o, e) =>
    {
      menu = new(Pb.Size);
      Cursor.Position = new Point(GameForm.Width / 2, GameForm.Height / 2);
      Bmp = new Bitmap(Pb.Width, Pb.Height);
      GameGraphics = Graphics.FromImage(Bmp);
      GameGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
      Pb.Image = Bmp;
      this.SelectLevel(0);
      timer.Start();

      cursorReset = new Point(Pb.Width / 2, Pb.Height / 2);
      Cursor.Position = cursorReset;
      Engine.VirtualCamera.VCamera = new(-79.70122f, 52.03208f, -111.0604f);
      Engine.VirtualCamera.Yaw = -0.6224661f;
      Engine.VirtualCamera.Pitch = -0.24f;
      currLevel.VirtualCamera.VCamera = new(-79.70122f, 52.03208f, -111.0604f);
      currLevel.VirtualCamera.Yaw = -0.6224661f;
      currLevel.VirtualCamera.Pitch = -0.24f;  
    };

    timer.Tick += (o, e) =>
    {
      GameGraphics.Clear(Color.Gray);
      // GameGraphics.DrawImage(Bmp, 0, 0, Pb.Width, Pb.Height);
      if (MenuOpen)
      {
        menu.Draw(GameGraphics, Pb.Size);
        Pb.Refresh();

        if (menu.ButtonStart.Rec.Contains(CursorPos) && IsDown)
        {
          menu.ButtonStart.Click = true;
          LastState = true;
        }
        else
          menu.ButtonStart.Click = false;

        if (LastState && !menu.ButtonStart.Click)
        {
          MenuOpen = false;
          Cursor.Hide();
          cursorReset = new Point(Pb.Width / 2, Pb.Height / 2);
          Cursor.Position = cursorReset;
          Engine.VirtualCamera.VCamera = new(-79.70122f, 52.03208f, -111.0604f);
          Engine.VirtualCamera.Yaw = -0.6224661f;
          Engine.VirtualCamera.Pitch = -0.24f;
          currLevel.VirtualCamera.VCamera = new(-79.70122f, 52.03208f, -111.0604f);
          currLevel.VirtualCamera.Yaw = -0.6224661f;
          currLevel.VirtualCamera.Pitch = -0.24f;
        }

        return;
      }
      RenderLevel(GameGraphics);
    };

    GameForm.KeyDown += (o, e) => 
    {
      this.currLevel.KeyboardMap.KeyDown(e, Engine, currLevel, currLevel.Amelia);
      if(e.KeyCode == Keys.Escape)
        GameForm.Close();
      if(e.KeyCode == Keys.Tab)
      {
        levelNumber = -levelNumber;
        this.SelectLevel((int) (levelNumber / 2f + 0.5f));
      }
      if(e.KeyCode == Keys.H)
        this.Engine.HideHitboxes = !this.Engine.HideHitboxes;

    };
    GameForm.KeyUp += (o, e)
      => this.currLevel.KeyboardMap.KeyUp(e);
    initMouseHandle();



    Application.Run(GameForm);
  }
  public bool AllColected()
  {
    if(this.Pages == this.ColectedPages)
      return true;
    return false;
  }
  public void RenderLevel(Graphics g)
  {
    Font font = new Font("Comic Sans MS", 0.015f * GameForm.Width, FontStyle.Bold);
    Engine.RefreshAspectRatio(GameForm.Width, GameForm.Height);

    Engine.VirtualCamera.VCamera = currLevel.VirtualCamera.VCamera;
    Engine.VirtualCamera.Yaw = currLevel.VirtualCamera.Yaw;
    Engine.VirtualCamera.Pitch = currLevel.VirtualCamera.Pitch;
    currLevel.VirtualCamera.RefreshVTarget();

    currLevel.Refresh(g, Pb, Engine, this);

    cursorReset = new Point(Pb.Width / 2, Pb.Height / 2);
    if (GameForm.Focused)
      Cursor.Position = cursorReset;
    RectangleF countPos = new(0.8f * Pb.Width, 0.89f * Pb.Height, 0.2f * Pb.Width, 0.1f * Pb.Height);
    g.DrawString($"Páginas Coletadas: {ColectedPages.ToString()} / {Pages.ToString()}", font, Brushes.White, countPos);
    // Mapper.Map(g, Engine.VirtualCamera);
    // Debugger.ShowOnScreen(g, new string[]{
    //   "ScreenSize = " + GameForm.Width + " | " + GameForm.Height,
    //   "CPos = " + Engine.VirtualCamera.VCamera.X + " | " + Engine.VirtualCamera.VCamera.Y + " | " + Engine.VirtualCamera.VCamera.Z,
    //   "CPitchYaw = " + Engine.VirtualCamera.Pitch + " | " + Engine.VirtualCamera.Yaw,
    //   "Cursor = " + Cursor.Position.X + " | " + Cursor.Position.Y,
    //   "A3D = " + currLevel.Amelia.Anchor3D.X + " | " + currLevel.Amelia.Anchor3D.Y + " | " + currLevel.Amelia.Anchor3D.Z,
    //   "A2D = " + currLevel.Amelia.X + " | " + currLevel.Amelia.Y,
    //   "Sprite = " + currLevel.Amelia.manager.SpriteIndex + " | " + currLevel.Amelia.manager.QuantSprite,
    //   "ASiz = " + currLevel.Amelia.Height + " | " + currLevel.Amelia.RealSize,
    //   "VCT = " + Engine.VirtualCamera.VTarget,
    //   "VCLD = " + Engine.VirtualCamera.VLookDirection,
    //   "VCLDL = " + currLevel.VirtualCamera.VLookDirection,
    //   "Yaw = " + Engine.VirtualCamera.Yaw,
    //   "Pit = " + Engine.VirtualCamera.Pitch,
    //   "LightSource = " + Engine.NLightDirection,
    //   "Darkest/Brightest = " + Engine.Darkest + " / " + Engine.Brightest,
    //   "Level = " + this.levelNumber,
    //   "Wall = " + (currLevel.CM.IsColliding(currLevel.Amelia).Count()),
    //   "Amelia = " + currLevel.Amelia.Hitbox.ToString(),
    // });
    
    Pb.Refresh();
  }
  public void RemoveBook(Book book)
    => currLevel.Entities.Remove(book);
  
}