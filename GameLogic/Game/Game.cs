
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using EM3D;
using EM3D.EMUtils;
using GameLogic;

public partial class Game
{
  public List<Level> Levels;
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
  public Amelia currPlayer;


  public Game()
  {
    this.Pb = new PictureBox { Dock = DockStyle.Fill };
    this.timer = new Timer { Interval = 2 };
    this.GameForm = new Form
    {
      WindowState = FormWindowState.Maximized,
      // FormBorderStyle = FormBorderStyle.None,
      Controls = { Pb }
    };
    this.cursorReset = new Point(Pb.Width / 2, Pb.Height / 2);
  }


  public void RenderLevel(Graphics g, Level level)
  {
    g.Clear(Color.Gray);

    if (MenuOpen)
    {
      StartMenu.Draw(g, Pb.Width, Pb.Height);
      Pb.Refresh();

      if (StartMenu.ButtonStart.ButtonPosition.Contains(CursorPos) && IsDown)
      {
        StartMenu.ButtonStart.Click = true;
        LastState = true;
      }
      else
        StartMenu.ButtonStart.Click = false;

      if (LastState && !StartMenu.ButtonStart.Click)
      {
        MenuOpen = false;
        Cursor.Hide();
      }

      return;
    }

    Engine.RefreshAspectRatio(GameForm.Width, GameForm.Height);
        Debugger.ShowOnScreen(g, new string[]{
      "ScreenSize = " + GameForm.Width + " | " + GameForm.Height,
      "CPos = " + Engine.VirtualCamera.VCamera.X + " | " + Engine.VirtualCamera.VCamera.Y + " | " + Engine.VirtualCamera.VCamera.Z,
      "CPitchYaw = " + Engine.VirtualCamera.Pitch + " | " + Engine.VirtualCamera.Yaw,
      "Cursor = " + Cursor.Position.X + " | " + Cursor.Position.Y,
      "A3D = " + level.Amelia.Anchor3D.X + " | " + level.Amelia.Anchor3D.Y + " | " + level.Amelia.Anchor3D.Z,
      "A2D = " + level.Amelia.X + " | " + level.Amelia.Y,
      "Sprite = " + level.Amelia.manager.SpriteIndex + " | " + level.Amelia.manager.QuantSprite,
      "ASiz = " + level.Amelia.Height + " | " + level.Amelia.RealSize,
      "VCT = " + Engine.VirtualCamera.VTarget,
      "VCLD = " + Engine.VirtualCamera.VLookDirection,
      "Yaw = " + Engine.VirtualCamera.Yaw,
      "Pit = " + Engine.VirtualCamera.Pitch,
      "LightSource = " + Engine.NLightDirection,
      "Darkest/Brightest = " + Engine.Darkest + " / " + Engine.Brightest,
    });
  }
}