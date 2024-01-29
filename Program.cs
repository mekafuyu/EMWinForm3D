using System;
using System.Drawing;
using System.Windows.Forms;
using EM3D;
using EM3D.EMUtils;

// Mesh obj3D = EMFile.LoadObjectFile("mountains.obj");
// Mesh spc = EMFile.LoadObjectFile("example.obj");
Mesh floor = EMFile.LoadObjectFile("./assets/models/chao.obj");

Bitmap bmp = null;
Graphics g = null;
Amelia amelia = null;
Wall parede = null;
Door porta = null;
Portal portal = null;
Portal portal2 = null;
ColissionManager.Current.Reset();
float thetaX = 0, thetaY = 0, thetaZ = 0;
float transX = 0, transY = 0, transZ = 0;

Mesh[] meshesToRender = new Mesh[] { floor };

PictureBox pb = new PictureBox { Dock = DockStyle.Fill };

var timer = new Timer { Interval = 2 };

var form = new Form
{
  WindowState = FormWindowState.Maximized,
  // FormBorderStyle = FormBorderStyle.None,
  Controls = { pb }
};
var eng = new EMEngine(form.Width, form.Height);

// OnStart
form.Load += (o, e) =>
{
  parede = new Wall(0, -1, -20, 10, 10);
  amelia = new Amelia(0, -1, -5, 1, 10, 1);
  porta = new Door(-14,-1, 20, 5, 10, false);
  portal = new Portal(0,-1, 20, 5, 10, false);
  portal2 = new Portal(-14,-1, -20, 5, 10, false);
  portal.destiny = portal2;
  portal2.destiny = portal;
  
  ColissionManager.Current.AddEntity(amelia);
  ColissionManager.Current.AddEntity(parede);
  ColissionManager.Current.AddEntity(porta);
  ColissionManager.Current.AddEntity(portal);
  ColissionManager.Current.AddEntity(portal2);
  // Cursor.Hide();
  Cursor.Position = new Point(form.Width / 2, form.Height / 2);
  bmp = new Bitmap(pb.Width, pb.Height);
  g = Graphics.FromImage(bmp);
  g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
  pb.Image = bmp;
  timer.Start();
};

// OnFrame
bool showMesh = false;
float pitchMove = 0;
float yawMove = 0;
Point cursorReset = new Point(pb.Width / 2, pb.Height / 2);
Vertex bolinha = new(0, -1, -5);
// Image bg = Image.FromFile("./assets/imgs/paper.jpg");
timer.Tick += (o, e) =>
{
  g.Clear(Color.Gray);
  // g.DrawImage(bg, 0, 0, form.Width, form.Height);
  eng.RefreshAspectRatio(form.Width, form.Height);

  amelia.Move(0, pb.Width, 0, pb.Height);
  eng.GetFrame(
    (form.Width, form.Height),
    g,
    meshesToRender,
    (thetaX, thetaY, thetaZ),
    (transX, transY, transZ),
    true,
    showMesh,
    ColissionManager.Current.entities
  );

  Debugger.ShowOnScreen(g, new string[]{
    "ScreenSize = " + form.Width + " | " + form.Height,
    "CPos = " + eng.VirtualCamera.VCamera.X + " | " + eng.VirtualCamera.VCamera.Y + " | " + eng.VirtualCamera.VCamera.Z,
    "CPitchYaw = " + eng.VirtualCamera.Pitch + " | " + eng.VirtualCamera.Yaw,
    "Cursor = " + Cursor.Position.X + " | " + Cursor.Position.Y,
    "A3D = " + amelia.Anchor3D.X + " | " + amelia.Anchor3D.Y + " | " + amelia.Anchor3D.Z,
    "A2D = " + amelia.X + " | " + amelia.Y, 
    "Sprite = " + amelia.manager.SpriteIndex + " | " + amelia.manager.QuantSprite, 
    "ASiz = " + amelia.Height + " | " + amelia.RealSize, 
    "WPos = " + parede.Anchor3D.X + " | " + parede.Anchor3D.Y + " | " + parede.Anchor3D.Z, 
    "VCT = " + eng.VirtualCamera.VTarget, 
    "VCLD = " + eng.VirtualCamera.VLookDirection, 
    "YawM = " + eng.VirtualCamera.YawMove, 
    "PitM = " + eng.VirtualCamera.PitchMove,
    "LightSource = " + eng.NLightDirection,
    "Darkest/Brightest = " + eng.Darkest + " / " + eng.Brightest,
  });
  cursorReset = new Point(form.Width / 2, form.Height / 2);
  if (form.Focused)
    Cursor.Position = cursorReset;

  pb.Refresh();
};

// OnMouseMove
float sense = 0.001f;
pb.MouseMove += (o, e) =>
{
  pitchMove = (e.Location.Y - cursorReset.Y) * sense + 23 * sense;
  yawMove = (e.Location.X - cursorReset.X) * sense;
  eng.VirtualCamera.Yaw += yawMove;
  eng.VirtualCamera.Pitch -= pitchMove;
};

// OnKey
float speed = 0.01f;
float tspeed = 0.5f;
int xSpeed = 0;
int zSpeed = 0;
form.KeyDown += (o, e) =>
{
  switch (e.KeyCode)
  {
    case Keys.W:
      amelia.StartUp();
      if (zSpeed > 0)
        amelia.StartRight();
      if (zSpeed < 0)
        amelia.StartLeft();

      xSpeed = 20;
      break;

    case Keys.A:
      amelia.StartLeft();

      if (xSpeed > 0)
        amelia.StartUp();
      if (xSpeed < 0)
        amelia.StartDown();

      zSpeed = -20;
      break;

    case Keys.S:
      amelia.StartDown();

      if (zSpeed > 0)
        amelia.StartRight();
      if (zSpeed < 0)
        amelia.StartLeft();

      xSpeed = -20;
      break;

    case Keys.D:
      amelia.StartRight();

      if (xSpeed > 0)
        amelia.StartUp();
      if (xSpeed < 0)
        amelia.StartDown();

      zSpeed = 20;
      break;

    case Keys.J:
      eng.VirtualCamera.MoveLeft(tspeed);
      break;
    case Keys.L:
      eng.VirtualCamera.MoveRight(tspeed);
      break;
    case Keys.U:
      eng.VirtualCamera.Pitch -= speed;
      break;
    case Keys.O:
      eng.VirtualCamera.Pitch += speed;
      break;
    case Keys.I:
      eng.VirtualCamera.MoveFront(tspeed);
      break;
    case Keys.K:
      eng.VirtualCamera.MoveBack(tspeed);
      break;


    case Keys.Right:
      eng.VirtualCamera.MoveRight(tspeed);
      break;
    case Keys.Left:
      eng.VirtualCamera.MoveLeft(tspeed);
      break;
    case Keys.Space:
      eng.VirtualCamera.MoveUp(tspeed);
      break;
    case Keys.ShiftKey:
      eng.VirtualCamera.MoveDown(tspeed);
      break;


    case Keys.Escape:
      form.Close();
      break;
    case Keys.Enter:
      showMesh = !showMesh;
      break;
    case Keys.Tab:
      porta.ToggleDoor();
      break;
    case Keys.M:
      portal.TogglePortal();
      portal2.TogglePortal();
      break;
    case Keys.NumPad8:
      eng.VirtualCamera.VCamera = new(0, 0, 0);
      eng.VirtualCamera.VLookDirection = new(0, 0, 1);
      eng.VirtualCamera.Yaw = 0;
      eng.VirtualCamera.Pitch = 0;
      break;
    case Keys.NumPad2:
      eng.VirtualCamera.VCamera = new(0, 0, 0);
      eng.VirtualCamera.VLookDirection = new(0, 0, -1);
      eng.VirtualCamera.Yaw = MathF.PI;
      eng.VirtualCamera.Pitch = 0;
      break;
    case Keys.NumPad4:
      eng.VirtualCamera.VCamera = new(0, 0, 0);
      eng.VirtualCamera.VLookDirection = new(1, 0, 0);
      eng.VirtualCamera.Yaw = MathF.PI / 2;
      eng.VirtualCamera.Pitch = 0;
      break;
    case Keys.NumPad6:
      eng.VirtualCamera.VCamera = new(0, 0, 0);
      eng.VirtualCamera.VLookDirection = new(-1, 0, 0);
      eng.VirtualCamera.Yaw = 3 * MathF.PI / 2;
      eng.VirtualCamera.Pitch = 0;
      break;

    default:
      break;
  }
};

form.KeyUp += (o, e) =>
{
    switch (e.KeyCode)
    {
        case Keys.W:
            xSpeed = 0;
            break;
        case Keys.A:
            zSpeed = 0;
            break;

        case Keys.S:
            xSpeed = 0;
            break;

        case Keys.D:
            zSpeed = 0;
            break;

        default:
            break;
    }
};
  
Application.Run(form);
