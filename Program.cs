using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using EM3D;
using static EM3D.EMUtils;

float[] f = {1f, 2f, 3f};
Vec3D[] v = new Vec3D[]{(Vec3D) f};
Mesh cube = new(
    new Triangule[]{

        // SOUTH
        ArrToTri((0f, 0f, 0f), (0f, 1f, 0f), (1f, 1f, 0f)),
        ArrToTri((0f, 0f, 0f), (1f, 1f, 0f), (1f, 0f, 0f)),

        // EAST
        ArrToTri((1f, 0f, 0f), (1f, 1f, 0f), (1f, 1f, 1f)),
        ArrToTri((1f, 0f, 0f), (1f, 1f, 1f), (1f, 0f, 1f)),

        // NORTH
        ArrToTri((1f, 0f, 1f), (1f, 1f, 1f), (0f, 1f, 1f)),
        ArrToTri((1f, 0f, 1f), (0f, 1f, 1f), (0f, 0f, 1f)),

        // WEST
        ArrToTri((0f, 0f, 1f), (0f, 1f, 1f), (0f, 1f, 0f)),
        ArrToTri((0f, 0f, 1f), (0f, 1f, 0f), (0f, 0f, 0f)),

        // TOP
        ArrToTri((0f, 1f, 0f), (0f, 1f, 1f), (1f, 1f, 1f)),
        ArrToTri((0f, 1f, 0f), (1f, 1f, 1f), (1f, 1f, 0f)),

        // BOTTOM
        ArrToTri((1f, 0f, 1f), (0f, 0f, 1f), (0f, 0f, 0f)),
        ArrToTri((1f, 0f, 1f), (0f, 0f, 0f), (1f, 0f, 0f)),
    }
);

Bitmap bmp = null;
Graphics g = null;

PictureBox pb = new PictureBox{
    Dock = DockStyle.Fill
};

var timer = new Timer{
    Interval = 20
};

var form = new Form
{
    WindowState = FormWindowState.Maximized,
    Controls = { pb }
};
var eng = new EMEngine(form.Width, form.Height);

form.Load += (o, e) =>
{
    bmp = new Bitmap(
        pb.Width,
        pb.Height
    );
    g = Graphics.FromImage(bmp);
    pb.Image = bmp;
    timer.Start();
};


float theta = 0;
Mat4x4 matRotZ = new(), matRotX = new();
var triColor = Brushes.Gray;
timer.Tick += (o, e) => {
    g.Clear(Color.White);

    matRotZ.m[0,0] = MathF.Cos(theta);
    matRotZ.m[0,1] = MathF.Sin(theta);
    matRotZ.m[1,0] = -MathF.Sin(theta);
    matRotZ.m[1,1] = MathF.Cos(theta);
    matRotZ.m[2,2] = 1;
    matRotZ.m[3,3] = 1;

    matRotX.m[0,0] = 1;
    matRotX.m[1,1] = MathF.Cos(theta * 0.5f);
    matRotX.m[1,2] = MathF.Sin(theta * 0.5f);
    matRotX.m[2,1] = -MathF.Sin(theta * 0.5f);
    matRotX.m[2,2] = MathF.Cos(theta * 0.5f);
    matRotX.m[3,3] = 1;

    foreach (var tri in cube.t)
    {
        var rotatedTri = EMEngine.RotateTriangle(tri, matRotZ);
        rotatedTri = EMEngine.RotateTriangle(rotatedTri, matRotX);
        var (triProj, lightInt) = EMEngine.ProjectTriangle(rotatedTri, eng.LightDirection, eng.VCamera, eng.matProj, (form.Width, form.Height));
        if(triProj is null)
            continue;
        int[] rgb = new int[]{143, 180, 255};

        SolidBrush b = new(Color.FromArgb( (int) (rgb[0] * lightInt), (int) (rgb[1] * lightInt), (int) (rgb[2] * lightInt)));
        EMEngine.FillTriangleWithGraphics(
            b,
            g,
            triProj
        );
    }
    
    theta += 0.05f;
    
    pb.Refresh();
};



Application.Run(form);