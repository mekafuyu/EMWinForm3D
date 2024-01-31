using System;
using System.Drawing;
using System.Windows.Forms;
using EM3D;
using EM3D.EMUtils;
using GameLogic;

// Mesh obj3D = EMFile.LoadObjectFile("mountains.obj");
// Mesh spc = EMFile.LoadObjectFile("example.obj");
Mesh cube1 = EMFile.LoadObjectFile("./assets/models/longRoom.obj");
// Mesh.TranslateMesh(cube1, 5, 0, 0);
Mesh.Scale(cube1, 0.1f);
// Mesh.Translate(cube1, 0, -0.1f * 128f, 0);
cube1.Color = new byte[]{255, 255, 255};

Mesh cube2 = EMFile.LoadObjectFile("./assets/models/cube.obj");
// Mesh.Replace(cube2, 5, 0, 0);
cube2.Color = new byte[]{0, 255, 0};

Bitmap bmp = null;
Graphics g = null;
Amelia amelia = null;
Wall parede = null;
Door porta = null;
Portal portal = null;
Portal portal2 = null;
Menu menu = null;
Floor floor = null;

PointF cursor = new();
bool isDown = false;

ColissionManager.Current.Reset();
float thetaX = 0, thetaY = 0, thetaZ = 0;
float transX = 0, transY = 0, transZ = 0;

Mesh[] meshesToRender = new Mesh[] { cube1, cube2 };

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
  parede = new Wall(0, 0, -20, 10, 10);
  amelia = new Amelia(0, 0, -5, 1, 10, 1);
  porta = new Door(-14, 0, 20, 5, 10, false);
  portal = new Portal(0, 0, 20, 5, 10, false);
  portal2 = new Portal(-14, 0, -20, 5, 10, false);
  floor = new Floor(-64f * 0.1f, 0, -256f * 0.1f, 256 * 0.1f, 128f * 0.1f);
  
  menu = new(pb.Size);
  portal.destiny = portal2;
  portal2.destiny = portal;
  
  ColissionManager.Current.AddEntity(amelia);
  ColissionManager.Current.AddEntity(parede);
  ColissionManager.Current.AddEntity(porta);
  ColissionManager.Current.AddEntity(portal);
  ColissionManager.Current.AddEntity(portal2);
  ColissionManager.Current.AddEntity(floor);
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
Image bg = Image.FromFile("./assets/imgs/bg.png");
bool menuOpen = true;
bool lastState = false;
timer.Tick += (o, e) =>
{
  g.Clear(Color.Gray);
  g.DrawImage(bg, 0, 0, form.Width, form.Height);
  if(menuOpen)
  {
    menu.Draw(g, pb.Width, pb.Height);
    pb.Refresh();

    if(menu.ButtonStart.ButtonPosition.Contains(cursor) && isDown)
    {
      menu.ButtonStart.Click = true;
      lastState = true;
    }
    else
      menu.ButtonStart.Click = false;

    if(lastState && !menu.ButtonStart.Click)
      menuOpen = false;

    return;
  }

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
    "Yaw = " + eng.VirtualCamera.Yaw,
    "Pit = " + eng.VirtualCamera.Pitch,
    "LightSource = " + eng.NLightDirection,
    "Darkest/Brightest = " + eng.Darkest + " / " + eng.Brightest,
  });
  cursorReset = new Point(pb.Width / 2, pb.Height / 2);
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
  cursor = e.Location;
};
pb.MouseDown += (o, e) =>
  isDown = true;
pb.MouseUp += (o, e) =>
  isDown = false;

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
    
    case Keys.Z:
      MessageBox.Show(eng.VirtualCamera.VCamera.ToString());
      break;
    case Keys.X:
      eng.VirtualCamera.VCamera = new(0, 0, 75);
      eng.VirtualCamera.VLookDirection = new(0, 0, -1);
      eng.VirtualCamera.Yaw = 9.5f;
      eng.VirtualCamera.Pitch = 0f;
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
