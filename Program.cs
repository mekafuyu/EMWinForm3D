using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Windows.Forms;
using EM3D;
using static EM3D.EMUtils.Utils;
using static EM3D.EMUtils.Geometry;
using static EM3D.EMUtils.Drawing;

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
    ArrToTri(new(-1f, -1f, -1f), new(-1f, 1f, -1f), new(1f, 1f, -1f)),
    ArrToTri(new(-1f, -1f, -1f), new(1f, 1f, -1f), new(1f, -1f, -1f)),
    // EAST
    ArrToTri(new(1f, -1f, -1f), new(1f, 1f, -1f), new(1f, 1f, 1f)),
    ArrToTri(new(1f, -1f, -1f), new(1f, 1f, 1f), new(1f, -1f, 1f)),
    // NORTH
    ArrToTri(new(1f, -1f, 1f), new(1f, 1f, 1f), new(-1f, 1f, 1f)),
    ArrToTri(new(1f, -1f, 1f), new(-1f, 1f, 1f), new(-1f, -1f, 1f)),
    // WEST
    ArrToTri(new(-1f, -1f, 1f), new(-1f, 1f, 1f), new(-1f, 1f, -1f)),
    ArrToTri(new(-1f, -1f, 1f), new(-1f, 1f, -1f), new(-1f, -1f, -1f)),
    // TOP
    ArrToTri(new(-1f, 1f, -1f), new(-1f, 1f, 1f), new(1f, 1f, 1f)),
    ArrToTri(new(-1f, 1f, -1f), new(1f, 1f, 1f), new(1f, 1f, -1f)),
    // BOTTOM
    ArrToTri(new(1f, -1f, 1f), new(-1f, -1f, 1f), new(-1f, -1f, -1f)),
    ArrToTri(new(1f, -1f, 1f), new(-1f, -1f, -1f), new(1f, -1f, -1f)),
  }
);

Bitmap bmp = null;
Graphics g = null;
float thetaX = 0, thetaY = 0, thetaZ = 0;
float transX = 0, transY = 0, transZ = 0;
Matrix4x4 mrx, mry, mrz;
Mesh[] meshesToRender = new Mesh[] { cube }; 

PictureBox pb = new PictureBox { Dock = DockStyle.Fill };

var timer = new Timer { Interval = 20 };

var form = new Form { WindowState = FormWindowState.Maximized, Controls = { pb } };
var eng = new EMEngine(form.Width, form.Height);

// OnStart
form.Load += (o, e) =>
{
  bmp = new Bitmap(pb.Width, pb.Height);
  g = Graphics.FromImage(bmp);
  pb.Image = bmp;
  timer.Start();
};

// OnFrame
timer.Tick += (o, e) =>
{
  g.Clear(Color.White);

  mrx = GetRotateInXMatrix(thetaX);
  mry = GetRotateInYMatrix(thetaY);
  mrz = GetRotateInZMatrix(thetaZ);

  foreach (var mesh in meshesToRender)
  {
    foreach (var tri in cube.t)
    {
      var moddedTri = (Triangle) tri.Clone();
      moddedTri = TranslateTriangle3D(moddedTri, transX, transY, transZ);
      moddedTri = RotateTriangle3D(moddedTri, mrx);
      moddedTri = RotateTriangle3D(moddedTri, mry);
      moddedTri = RotateTriangle3D(moddedTri, mrz);

      var (triProj, lightInt) = ProjectTriangle(
        moddedTri,
        eng.LightDirection,
        eng.VCamera,
        eng.matProj,
        (form.Width, form.Height)
      );
      if (triProj is null)
        continue;
      int[] rgb = new int[] { 3, 252, 211 };

      SolidBrush b =
        new(
          Color.FromArgb(
            (int)(rgb[0] * lightInt),
            (int)(rgb[1] * lightInt),
            (int)(rgb[2] * lightInt)
          )
        );

      FillTriangleWithGraphics(b, g, triProj);
    }
  }

  pb.Refresh();
};

// OnKey
form.KeyDown += (o, e) =>
{
  switch (e.KeyCode)
  {
    case Keys.D:
      thetaY += 0.01f;
      break;
    case Keys.A:
      thetaY -= 0.01f;
      break;
    case Keys.S:
      thetaX += 0.01f;
      break;
    case Keys.W:
      thetaX -= 0.01f;
      break;
    case Keys.E:
      thetaZ += 0.01f;
      break;
    case Keys.Q:
      thetaZ -= 0.01f;
      break;

    case Keys.J:
      transX += 0.01f;
      break;
    case Keys.L:
      transX -= 0.01f;
      break;
    case Keys.I:
      transY += 0.01f;
      break;
    case Keys.K:
      transY -= 0.01f;
      break;
    case Keys.O:
      transZ += 0.01f;
      break;
    case Keys.U:
      transZ -= 0.01f;
      break;

    default:
      break;
  }
};



Application.Run(form);
