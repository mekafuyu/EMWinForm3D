using System.Drawing;
using System.Windows.Forms;
using EM3D;
using EM3D.EMUtils;

Mesh obj3D = EMFile.LoadObjectFile("mountains.obj");
Mesh spc = EMFile.LoadObjectFile("example.obj");
Mesh floor = EMFile.LoadObjectFile("chao.obj");

Bitmap bmp = null;
Graphics g = null;
Amelia amelia = null;
float thetaX = 0, thetaY = 0, thetaZ = 0;
float transX = 0, transY = 0, transZ = 0;

Mesh[] meshesToRender = new Mesh[] { floor };

PictureBox pb = new PictureBox { Dock = DockStyle.Fill };

var timer = new Timer { Interval = 1};

var form = new Form {
  WindowState = FormWindowState.Maximized,
  Controls = { pb },
};
var eng = new EMEngine(form.Width, form.Height);

// OnStart
form.Load += (o, e) =>
{
  amelia = new Amelia(pb.Width / 2);
  amelia.Pos3D = new(0, -1, -5);
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

timer.Tick += (o, e) =>
{
  g.Clear(Color.Gray);
  eng.RefreshAspectRatio(form.Width, form.Height);

  eng.GetFrame(
    (form.Width, form.Height),
    g,
    meshesToRender,
    (thetaX, thetaY, thetaZ),
    (transX, transY, transZ),
    true,
    showMesh,
    bolinha,
    amelia
  );
  
  //amelia 
  // amelia.Move(0 , pb.Width, 0, pb.Height);
  // amelia.Draw(g);

  // Point mouseP = form.Cursor.Position;
  g.DrawString("T = " + eng.VirtualCamera.VCamera.X + " | " + eng.VirtualCamera.VCamera.Y + " | " + eng.VirtualCamera.VCamera.Z, SystemFonts.DefaultFont, Brushes.White, 0, 30);
  g.DrawString("R = " + thetaX + " | " + eng.VirtualCamera.Yaw + " | " + thetaZ, SystemFonts.DefaultFont, Brushes.White, 0, 40);
  g.DrawString("F = " + form.Width + " | " + form.Height, SystemFonts.DefaultFont, Brushes.White, 0, 50);
  g.DrawString("MM = " + pitchMove + " | " + yawMove, SystemFonts.DefaultFont, Brushes.White, 0, 60);
  g.DrawString("MP = " + Cursor.Position.X + " | " + Cursor.Position.Y, SystemFonts.DefaultFont, Brushes.White, 0, 70);
  g.DrawString("CP = " + cursorReset.X + " | " + cursorReset.Y, SystemFonts.DefaultFont, Brushes.White, 0, 80);
  g.DrawString("PB = " + bolinha.X + " | " + bolinha.Y, SystemFonts.DefaultFont, Brushes.White, 0, 90);
  g.DrawString("PA = " + amelia.Pos3D.X + " | " + amelia.Pos3D.Y, SystemFonts.DefaultFont, Brushes.White, 0, 100);
  cursorReset = new Point(form.Width / 2, form.Height / 2);

  if(form.Focused)
    Cursor.Position = cursorReset;
  
  // thetaY += 0.001f;
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
form.KeyDown += (o, e) =>
{
   switch (e.KeyCode)
  {
    case Keys.W:
      bolinha.Z += 0.1f;
      amelia.StartUp();
      break;

    case Keys.A:
      bolinha.X += 0.1f;
      amelia.StartLeft();
      break;

    case Keys.S:
      bolinha.Z -= 0.1f;
      amelia.StartDown();
      break;

    case Keys.D:
      bolinha.X -= 0.1f;
      amelia.StartRight();
      break;

    case Keys.J:
      eng.VirtualCamera.Yaw -= speed;
      break;
    case Keys.L:
      eng.VirtualCamera.Yaw += speed;
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

    default:
      break;
  }
};



Application.Run(form);
