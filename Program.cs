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
  g.DrawString("T = " + transX + " | " + transY + " | " + transZ, SystemFonts.DefaultFont, Brushes.White, 0, 30);
  g.DrawString("R = " + thetaX + " | " + thetaY + " | " + thetaZ, SystemFonts.DefaultFont, Brushes.White, 0, 40);
  g.DrawString("F = " + form.Width + " | " + form.Height, SystemFonts.DefaultFont, Brushes.White, 0, 50);
  
  // thetaY += 0.001f;
  pb.Refresh();
};

// OnKey
float speed = 0.05f;
float tspeed = 1f;
form.KeyDown += (o, e) =>
{
   switch (e.KeyCode)
  {
    case Keys.D:
      thetaY += speed;
      break;
    case Keys.A:
      thetaY -= speed;
      break;
    case Keys.S:
      thetaX += speed;
      break;
    case Keys.W:
      thetaX -= speed;
      break;
    case Keys.E:
      thetaZ += speed;
      break;
    case Keys.Q:
      thetaZ -= speed;
      break;

    case Keys.J:
      eng.VCamera.X += tspeed;
      break;
    case Keys.L:
      eng.VCamera.X -= tspeed;
      break;
    case Keys.I:
      eng.VCamera.Z += tspeed;
      break;
    case Keys.K:
      eng.VCamera.Z -= tspeed;
      break;
    case Keys.O:
      eng.VCamera.Y -= tspeed;
      break;
    case Keys.U:
      eng.VCamera.Y += tspeed;
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
