using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Windows.Forms;
using EM3D;
using static EM3D.EMUtils.Utils;
using static EM3D.EMUtils.Geometry;
using static EM3D.EMUtils.Drawing;

Mesh spaceship = LoadObjectFile("example.obj");

// Mesh cube = new(
//   new Triangle[]
//   {
//     // SOUTH
//     ArrToTri(new(0f, 0f, 0f), new(0f, 1f, 0f), new(1f, 1f, 0f)),
//     ArrToTri(new(0f, 0f, 0f), new(1f, 1f, 0f), new(1f, 0f, 0f)),
//     // EAST
//     ArrToTri(new(1f, 0f, 0f), new(1f, 1f, 0f), new(1f, 1f, 1f)),
//     ArrToTri(new(1f, 0f, 0f), new(1f, 1f, 1f), new(1f, 0f, 1f)),
//     // NORTH
//     ArrToTri(new(1f, 0f, 1f), new(1f, 1f, 1f), new(0f, 1f, 1f)),
//     ArrToTri(new(1f, 0f, 1f), new(0f, 1f, 1f), new(0f, 0f, 1f)),
//     // WEST
//     ArrToTri(new(0f, 0f, 1f), new(0f, 1f, 1f), new(0f, 1f, 0f)),
//     ArrToTri(new(0f, 0f, 1f), new(0f, 1f, 0f), new(0f, 0f, 0f)),
//     // TOP
//     ArrToTri(new(0f, 1f, 0f), new(0f, 1f, 1f), new(1f, 1f, 1f)),
//     ArrToTri(new(0f, 1f, 0f), new(1f, 1f, 1f), new(1f, 1f, 0f)),
//     // BOTTOM
//     ArrToTri(new(1f, 0f, 1f), new(0f, 0f, 1f), new(0f, 0f, 0f)),
//     ArrToTri(new(1f, 0f, 1f), new(0f, 0f, 0f), new(1f, 0f, 0f)),
//   }
// );
Mesh cube = new(
  new Triangle[]
  {
    // SOUTH
    ArrToTri(new(-0.5f, -0.5f, -0.5f), new(-0.5f, 0.5f, -0.5f), new(0.5f, 0.5f, -0.5f)),
    ArrToTri(new(-0.5f, -0.5f, -0.5f), new(0.5f, 0.5f, -0.5f), new(0.5f, -0.5f, -0.5f)),
    // EAST
    ArrToTri(new(0.5f, -0.5f, -0.5f), new(0.5f, 0.5f, -0.5f), new(0.5f, 0.5f, 0.5f)),
    ArrToTri(new(0.5f, -0.5f, -0.5f), new(0.5f, 0.5f, 0.5f), new(0.5f, -0.5f, 0.5f)),
    // NORTH
    ArrToTri(new(0.5f, -0.5f, 0.5f), new(0.5f, 0.5f, 0.5f), new(-0.5f, 0.5f, 0.5f)),
    ArrToTri(new(0.5f, -0.5f, 0.5f), new(-0.5f, 0.5f, 0.5f), new(-0.5f, -0.5f, 0.5f)),
    // WEST
    ArrToTri(new(-0.5f, -0.5f, 0.5f), new(-0.5f, 0.5f, 0.5f), new(-0.5f, 0.5f, -0.5f)),
    ArrToTri(new(-0.5f, -0.5f, 0.5f), new(-0.5f, 0.5f, -0.5f), new(-0.5f, -0.5f, -0.5f)),
    // TOP
    ArrToTri(new(-0.5f, 0.5f, -0.5f), new(-0.5f, 0.5f, 0.5f), new(0.5f, 0.5f, 0.5f)),
    ArrToTri(new(-0.5f, 0.5f, -0.5f), new(0.5f, 0.5f, 0.5f), new(0.5f, 0.5f, -0.5f)),
    // BOTTOM
    ArrToTri(new(0.5f, -0.5f, 0.5f), new(-0.5f, -0.5f, 0.5f), new(-0.5f, -0.5f, -0.5f)),
    ArrToTri(new(0.5f, -0.5f, 0.5f), new(-0.5f, -0.5f, -0.5f), new(0.5f, -0.5f, -0.5f)),
  }
);

Bitmap bmp = null;
Graphics g = null;
float thetaX = 0, thetaY = 0, thetaZ = 0;
float transX = 0, transY = 0, transZ = 0;
Matrix4x4 mrx, mry, mrz;
Mesh[] meshesToRender = new Mesh[] { spaceship }; 

PictureBox pb = new PictureBox { Dock = DockStyle.Fill };

var timer = new Timer { Interval = 20 };

var form = new Form {
  WindowState = FormWindowState.Maximized,
  Controls = { pb }
};
var eng = new EMEngine(form.Width, form.Height);

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
timer.Tick += (o, e) =>
{
  g.Clear(Color.Black);
  g.DrawString("Cube v0.0.4", SystemFonts.DefaultFont, Brushes.White, new PointF(0f, 0f));

  mrx = GetRotateInXMatrix(thetaX);
  mry = GetRotateInYMatrix(thetaY);
  mrz = GetRotateInZMatrix(thetaZ);

  mrx *= mry * mrz;

  Pen p = new Pen(Color.FromArgb(255, 0, 0, 0), 2 * form.Width/form.Height);

  foreach (var mesh in meshesToRender)
  {
    foreach (var tri in mesh.t)
    {
      var moddedTri = (Triangle) tri.Clone();
      moddedTri = TranslateTriangle3D(moddedTri, transX, transY, transZ);
      moddedTri = RotateTriangle3D(moddedTri, mrx);
      // moddedTri = RotateTriangle3D(moddedTri, mry);
      // moddedTri = RotateTriangle3D(moddedTri, mrz);

      var (triProj, lightInt) = ProjectTriangle(
        moddedTri,
        eng.LightDirection,
        eng.VCamera,
        eng.matProj,
        (form.Width, form.Height)
      );
      if (triProj is null)
        continue;
      int[] rgb = new int[] { 255, 255, 255 };
      if(lightInt < 0 || lightInt > 1)
        lightInt = 0.5f;

      SolidBrush b =
        new(
          Color.FromArgb(
            (int)(rgb[0] * lightInt),
            (int)(rgb[1] * lightInt),
            (int)(rgb[2] * lightInt)
          )
        );

      FillTriangleWithGraphics(b, g, triProj);
      DrawTriangleWithGraphics(p, g, triProj);
      thetaX += 0.01f;
      thetaZ += 0.005f;
    }
  }

  pb.Refresh();
};

// OnKey
float speed = 0.05f;
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
