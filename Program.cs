using System.Drawing;
using System.Windows.Forms;
using FastEM3D;
using FastEM3D.EMUtils;

Mesh obj3D = EMFile.LoadObjectFile("axis.obj");

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
  form.WindowState = FormWindowState.Normal;
  timer.Start();
};

// OnFrame
bool showMesh = false;
timer.Tick += (o, e) =>
{
  g.Clear(Color.Black);
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
  g.DrawString("T = " + eng.VirtualCamera.VCamera.X + " | " + eng.VirtualCamera.VCamera.Y + " | " + eng.VirtualCamera.VCamera.Z, SystemFonts.DefaultFont, Brushes.White, 0, 30);
  g.DrawString("R = " + thetaX + " | " + eng.VirtualCamera.Yaw + " | " + thetaZ, SystemFonts.DefaultFont, Brushes.White, 0, 40);
  g.DrawString("F = " + form.Width + " | " + form.Height, SystemFonts.DefaultFont, Brushes.White, 0, 50);
  
  // thetaY += 0.001f;
  pb.Refresh();
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
    // case Keys.W:
    //   thetaY -= speed;
    //   break;
    // case Keys.S:
    //   thetaY += speed;
    //   break;
    // case Keys.E:
    //   thetaZ += speed;
    //   break;
    // case Keys.Q:
    //   thetaZ -= speed;
    //   break;

    case Keys.J:
      eng.VirtualCamera.MoveRight(tspeed);
      break;
    case Keys.L:
      eng.VirtualCamera.MoveLeft(tspeed);
      break;
    case Keys.I:
      eng.VirtualCamera.MoveFront(tspeed);
      break;
    case Keys.K:
      eng.VirtualCamera.MoveBack(tspeed);
      break;
    case Keys.O:
      eng.VirtualCamera.MoveDown(tspeed);
      break;
    case Keys.U:
      eng.VirtualCamera.MoveUp(tspeed);
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
