using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Windows.Forms;
using EM3D;
using static EM3D.EMUtils.Utils;
using static EM3D.EMUtils.Geometry;
using static EM3D.EMUtils.Drawing;
using System.Collections.Generic;
using System.Linq;

Mesh spaceship = LoadObjectFile("example.obj");

Mesh cube = new(
  new Triangle[]
  {
    // SOUTH
    ArrToTri(new(0f, 0f, 0f), new(0f, 1f, 0f), new(1f, 1f, 0f)),
    ArrToTri(new(0f, 0f, 0f), new(1f, 1f, 0f), new(1f, 0f, 0f)),
    // EAST
    ArrToTri(new(1f, 0f, 0f), new(1f, 1f, 0f), new(1f, 1f, 1f)),
    ArrToTri(new(1f, 0f, 0f), new(1f, 1f, 1f), new(1f, 0f, 1f)),
    // NORTH
    ArrToTri(new(1f, 0f, 1f), new(1f, 1f, 1f), new(0f, 1f, 1f)),
    ArrToTri(new(1f, 0f, 1f), new(0f, 1f, 1f), new(0f, 0f, 1f)),
    // WEST
    ArrToTri(new(0f, 0f, 1f), new(0f, 1f, 1f), new(0f, 1f, 0f)),
    ArrToTri(new(0f, 0f, 1f), new(0f, 1f, 0f), new(0f, 0f, 0f)),
    // TOP
    ArrToTri(new(0f, 1f, 0f), new(0f, 1f, 1f), new(1f, 1f, 1f)),
    ArrToTri(new(0f, 1f, 0f), new(1f, 1f, 1f), new(1f, 1f, 0f)),
    // BOTTOM
    ArrToTri(new(1f, 0f, 1f), new(0f, 0f, 1f), new(0f, 0f, 0f)),
    ArrToTri(new(1f, 0f, 1f), new(0f, 0f, 0f), new(1f, 0f, 0f)),
  }
);

Bitmap bmp = null;
Graphics g = null;
float thetaX = 0, thetaY = 0, thetaZ = 0;
float transX = 0, transY = 0, transZ = 0;
Matrix4x4 mrx, mry, mrz;
Mesh[] meshesToRender = new Mesh[] { spaceship, cube }; 

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
int[] rgb = new int[] { 128, 128, 255 };
int damageTaken = 0;
timer.Tick += (o, e) =>
{
  g.Clear(Color.Black);
  g.DrawString("EM3D v0.0.6", SystemFonts.DefaultFont, Brushes.White, new PointF(0f, 0f));

  mrx = GetRotateInXMatrix(thetaX);
  mry = GetRotateInYMatrix(thetaY);
  mrz = GetRotateInZMatrix(thetaZ);

  mrx *= mry * mrz;

  Pen p = new Pen(Color.FromArgb(255, 0, 0, 0), 2 * form.Width/form.Height);

  List<Triangle> trianglesToRaster = new();
  foreach (var mesh in meshesToRender)
  {
    foreach (var meshTri in mesh.t)
    {
      if (meshTri is null)
        continue;
      var moddedTri = (Triangle) meshTri.Clone();
      moddedTri = RotateTriangle3D(moddedTri, mrx);
      moddedTri = TranslateTriangle3D(moddedTri, transX, transY, transZ);

      var triProj = ProjectTriangle(
        moddedTri,
        eng.LightDirection,
        eng.VCamera,
        eng.matProj,
        (form.Width, form.Height)
      );
      if (triProj is null)
        continue;
      trianglesToRaster.Add(triProj);
    }

    // trianglesToRaster.Sort(( ));
    trianglesToRaster = trianglesToRaster.OrderBy( t => t.zPos ).ToList();

    foreach (var triangle in trianglesToRaster)
    {
      SolidBrush b =
        new(
          Color.FromArgb(
            (int)(rgb[0] * triangle.lightIntensity),
            (int)(rgb[1] * triangle.lightIntensity),
            (int)(rgb[2] * triangle.lightIntensity)
          )
        );
      FillTriangleWithGraphics(b, g, triangle);
      // DrawTriangleWithGraphics(p, g, triangle);
    }

    rgb = new int[]{128, 128, 255};
    if(damageTaken > 0 )
    {
      damageTaken--;
      if(damageTaken % 8 > 3)
      rgb = new int[]{255, 128, 128};
    }
  }

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
    case Keys.Space:
      damageTaken = 40;
      break;
    default:
      break;
  }
};



Application.Run(form);
