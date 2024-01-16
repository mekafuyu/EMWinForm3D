using System.Drawing;
using System.Numerics;
using System.Windows.Forms;
using FastEM3D;
using static EM3D.EMUtils.Utils;


Triangle tr1 = new((Vector4.One, Vector4.One, Vector4.One));
Triangle tr2 = (Triangle) tr1.Clone();
tr1.P.l1.X = 4f;


// Mesh spaceship = LoadObjectFile("axis.obj");

Bitmap bmp = null;
Graphics g = null;
float thetaX = 0, thetaY = 0, thetaZ = 0;
float transX = 0, transY = 0, transZ = 0;
// Mesh[] meshesToRender = new Mesh[] { spaceship }; 

PictureBox pb = new PictureBox { Dock = DockStyle.Fill };

var timer = new Timer { Interval = 1};

var form = new Form {
  WindowState = FormWindowState.Maximized,
  Controls = { pb },
};
// var eng = new EMEngine(form.Width, form.Height);

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
int[] rgb = new int[] { 128, 128, 255 };
// ObjExample toLoop = new(){
//   Eng = eng,
//   rgb = rgb 
// };
timer.Tick += (o, e) =>
{
  g.Clear(Color.Black);
  g.DrawString("T1 = " + tr1.P.l1, SystemFonts.DefaultFont, Brushes.White, 0, 10);
  g.DrawString("T2 = " + tr2.P.l1, SystemFonts.DefaultFont, Brushes.White, 0, 20);
  // toLoop.renderFrame(
  //   pb,
  //   form,
  //   g,
  //   meshesToRender,
  //   (thetaX, thetaY, thetaZ),
  //   (transX, transY, transZ)
  // );

  // thetaY += 0.001f;
  pb.Refresh();
};

// OnKey
float speed = 0.1f;
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
      transX += speed;
      break;
    case Keys.L:
      transX -= speed;
      break;
    case Keys.I:
      transY += speed;
      break;
    case Keys.K:
      transY -= speed;
      break;
    case Keys.O:
      transZ += speed;
      break;
    case Keys.U:
      transZ -= speed;
      break;
    case Keys.Escape:
      form.Close();
      break;
    default:
      break;
  }
};



Application.Run(form);
