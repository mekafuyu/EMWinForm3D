using System.Drawing;
using System.Windows.Forms;
using FastEM3D;
using FastEM3D.EMUtils;

Mesh obj3D = EMFile.LoadObjectFile("mountains.obj");

Bitmap bmp = null;
Graphics g = null;
float thetaX = 0, thetaY = 0, thetaZ = 0;
float transX = 0, transY = 0, transZ = 0;

Mesh[] meshesToRender = new Mesh[] { obj3D }; 

PictureBox pb = new PictureBox { Dock = DockStyle.Fill };

var timer = new Timer { Interval = 1};

var form = new Form {
  WindowState = FormWindowState.Maximized,
  Controls = { pb },
};
var eng = new FastEMEngine(form.Width, form.Height);

// OnStart
form.Load += (o, e) =>
{
  bmp = new Bitmap(pb.Width, pb.Height);
  g = Graphics.FromImage(bmp);
  pb.Image = bmp;
  timer.Start();
};

// OnFrame
bool showMesh = false;
float pitchMove = 0;
float yawMove = 0;
Point cursorReset = new Point(pb.Width / 2, pb.Height / 2);
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
    showMesh
  );
  // Point mouseP = form.Cursor.Position;
  g.DrawString("T = " + eng.VirtualCamera.VCamera.X + " | " + eng.VirtualCamera.VCamera.Y + " | " + eng.VirtualCamera.VCamera.Z, SystemFonts.DefaultFont, Brushes.White, 0, 30);
  g.DrawString("R = " + thetaX + " | " + eng.VirtualCamera.Yaw + " | " + thetaZ, SystemFonts.DefaultFont, Brushes.White, 0, 40);
  g.DrawString("F = " + form.Width + " | " + form.Height, SystemFonts.DefaultFont, Brushes.White, 0, 50);
  g.DrawString("MM = " + pitchMove + " | " + yawMove, SystemFonts.DefaultFont, Brushes.White, 0, 60);
  g.DrawString("MP = " + Cursor.Position.X + " | " + Cursor.Position.Y, SystemFonts.DefaultFont, Brushes.White, 0, 70);
  g.DrawString("CP = " + cursorReset.X + " | " + cursorReset.Y, SystemFonts.DefaultFont, Brushes.White, 0, 80);
  cursorReset = new Point(form.Width / 2, form.Height / 2);
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
    case Keys.A:
      eng.VirtualCamera.Yaw -= speed;
      break;
    case Keys.D:
      eng.VirtualCamera.Yaw += speed;
      break;
    case Keys.Q:
      eng.VirtualCamera.Pitch -= speed;
      break;
    case Keys.E:
      eng.VirtualCamera.Pitch += speed;
      break;
    case Keys.W:
      eng.VirtualCamera.MoveFront(tspeed);
      break;
    case Keys.S:
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
