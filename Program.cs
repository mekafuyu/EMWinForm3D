using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Windows.Forms;
using EM3D;
using static EM3D.EMUtils;


Mesh cube = new(
    new Triangule[]{

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
Matrix4x4 matRotZ = new(), matRotX = new();
timer.Tick += (o, e) => {
    g.Clear(Color.White);

    matRotZ[0,0] = MathF.Cos(theta);
    matRotZ[0,1] = MathF.Sin(theta);
    matRotZ[1,0] = -MathF.Sin(theta);
    matRotZ[1,1] = MathF.Cos(theta);
    matRotZ[2,2] = 1;
    matRotZ[3,3] = 1;

    matRotX[0,0] = 1;
    matRotX[1,1] = MathF.Cos(theta * 0.5f);
    matRotX[1,2] = MathF.Sin(theta * 0.5f);
    matRotX[2,1] = -MathF.Sin(theta * 0.5f);
    matRotX[2,2] = MathF.Cos(theta * 0.5f);
    matRotX[3,3] = 1;

    foreach (var tri in cube.t)
    {
        var rotatedTri = RotateTriangleWithMatrix(tri, matRotZ);
        rotatedTri = RotateTriangleWithMatrix(rotatedTri, matRotX);
        var (triProj, lightInt) = ProjectTriangle(rotatedTri, eng.LightDirection, eng.VCamera, eng.matProj, (form.Width, form.Height));
        if(triProj is null)
            continue;
        int[] rgb = new int[]{3, 252, 211};

        SolidBrush b = new(Color.FromArgb( (int) (rgb[0] * lightInt), (int) (rgb[1] * lightInt), (int) (rgb[2] * lightInt)));

        FillTriangleWithGraphics(
            b,
            g,
            triProj
        );
    }
    
    theta += 0.05f;
    
    pb.Refresh();
};



Application.Run(form);